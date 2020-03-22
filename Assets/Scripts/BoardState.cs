using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    private List<Unit> units;
    private List<KeyField> keyFields;
    private Army player1;
    private Army player2;
    private int winnerId;

    private void DeactivateAttacksOnUnit(int unitId)
    {
        foreach (Unit u in units)
        {
            u.DeactivateAttackOnUnit(unitId);
        }
    }

    public Unit GetForwardAttacker(int unitId)
    {
        foreach (Unit u in units)
        {
            if (u.CheckIfForwardAttackOn(unitId)) return u;
        }
        return null;
    }

    public BoardState(Army army1, Army army2)
    {
        units = new List<Unit>();
        keyFields = new List<KeyField>();
        player1 = army1;
        player2 = army2;
        winnerId = 0;
    }

    public BoardState(BoardState pattern)   // konstruktor kopiujący
    {
        units = new List<Unit>();
        keyFields = new List<KeyField>();
        // kopiuje dane armii
        player1 = new Army(pattern.player1);
        player2 = new Army(pattern.player2);
        // kopiuje listę jednostek
        foreach (Unit _unit in pattern.units)
        {
            if (player1.GetArmyId() == _unit.GetArmyId()) units.Add(new Unit(_unit, player1));  // przekazuje do jednostki referencję na jego armię
            else units.Add(new Unit(_unit, player2));
        }
        // kopiuje listę pól kluczowych
        foreach(KeyField _keyField in pattern.keyFields)   
        {
            keyFields.Add(new KeyField(_keyField));
        }
        winnerId = pattern.winnerId;
        // ustawia referenceje celów ataków
        /*foreach(Unit _unit in units)
        {
            _unit.SetAttacksTargets(units);
        }*/
    }

    public List<int> GetPossibleAttacks(int attackingPlayer)
    {
        List<int> resultList = new List<int>();
        if (winnerId != 0) return resultList;
        else
        {
            foreach (Unit _unit in units)
            {
                if (_unit.GetArmyId() == attackingPlayer && _unit.IsAvialable) resultList.AddRange(_unit.GetActiveAttacksIds());
            }
            return resultList;
        }
    }

    public List<StateChange> GetPossibleOutcomes(int attackingPlayer)   // zwraca wszystkie możliwe wyniki wszystkich aktywnych ataków gracza 
    {
        List<StateChange> resultList = new List<StateChange>();
        if (winnerId != 0) return resultList;
        else
        {
            foreach (Unit _unit in units)
            {
                if (_unit.GetArmyId() == attackingPlayer) resultList.AddRange(_unit.GetAttackOutcomes());
            }
            return resultList;
        }
    }

    public Unit GetUnit(int idUnit)
    {
        foreach(Unit u in units)
        {
            if (u.MyId(idUnit)) return u;
        }
        return null;
    }

    public KeyField GetKeyField(int idField)
    {
        foreach (KeyField k in keyFields)
        {
            if (k.GetFieldId() == idField) return k;
        }
        return null;
    }

    public Attack GetAttack(int idAttack)
    {
        Attack tempAttack = null;
        foreach(Unit u in units)
        {
            tempAttack = u.GetAttack(idAttack);
            if (tempAttack != null) return tempAttack;
        }
        return null;
    }

    public int ChangeState(StateChange change)  // zmienia stan planszy zgodnie z definicją zmiany
    {
        Unit u;
        KeyField kf;
        Attack a;
        int army1ActiveCount, army2ActiveCount;
        // wprowadza rezultat ataku do oddziału atakującego
        u = GetUnit(change.attackerId); 
        u.ChangeStrength(change.attackerStrengthChange);
        u.ChangeMorale(change.attackerMoraleChanged);
        if (!u.IsAvialable)
        {
            DeactivateAttacksOnUnit(u.GetUnitId());
            GetForwardAttacker(u.GetUnitId()).ActivateNotForwardAttacks();
        }
        if (change.activatedAttacks != null)
        {
            foreach (int i in change.activatedAttacks)   // przesyła wszystkie aktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
            {
                u.ActivateAttack(i);
            }
        }
        if (change.deactivatedAttacks != null)
        {
            foreach (int i in change.deactivatedAttacks) // przesyła wszystkie deaktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
            {
                u.DeactivateAttack(i);
            }
        }
        if(change.keyFieldChangeId !=0)
        {
            kf = GetKeyField(change.keyFieldChangeId);
            kf.SetOccupant(u.GetArmyId());
            a = u.GetAttackOnKeyField(change.keyFieldChangeId);
            a.keyFieldTaken = true;
            a.IncreaseAttack();
        }
        // wprowadza rezultat ataku do oddziału zaatakowanego
        u = GetUnit(change.defenderId);
        u.ChangeStrength(change.defenderStrengthChange);
        u.ChangeMorale(change.defenderMoraleChanged);
        if (!u.IsAvialable)
        {
            DeactivateAttacksOnUnit(u.GetUnitId());
            GetForwardAttacker(u.GetUnitId()).ActivateNotForwardAttacks();
        }
        if (change.activatedAttacks != null)
        {
            foreach (int i in change.activatedAttacks)  // przesyła wszystkie aktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
            {
                u.ActivateAttack(i);
            }
        }
        if (change.deactivatedAttacks != null)
        {
            foreach (int i in change.deactivatedAttacks)    // przesyła wszystkie deaktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
            {
                u.DeactivateAttack(i);
            }
        }
        if (change.keyFieldChangeId != 0)
        {
            a = u.GetAttackOnKeyField(change.keyFieldChangeId);
            a.keyFieldTaken = false;
            a.DecreaseAttack();
        }
        army1ActiveCount = 0;
        army2ActiveCount = 0;
        if (units != null)
        {
            foreach (Unit un in units)
            {
                if (un.IsAvialable && un.GetArmyId() == 1) army1ActiveCount++;
                if (un.IsAvialable && un.GetArmyId() == 2) army2ActiveCount++;
            }
        }
        if (army1ActiveCount == 0 && army2ActiveCount == 0) winnerId = -1;
        else if (army1ActiveCount == 0) winnerId = 2;
        else if (army2ActiveCount == 0) winnerId = 1;
        return winnerId;
    }

    public void AddUnit(Unit u)
    {
        units.Add(u);
    }

    public void AddKeyField(KeyField kf)
    {
        keyFields.Add(kf);
    }

    public int GetArmyMorale(int armyId)
    {
        if (armyId == 1) return player1.GetMorale();
        else if (armyId == 2) return player2.GetMorale();
        else return 0;
    }

    public Army GetArmy(int armyId)
    {
        if (armyId == 1) return player1;
        else if (armyId == 2) return player2;
        else return null;
    }

    public float EvaluateBoard(int armyId)
    {
        float score;

        score = 0.0f;
        foreach(Unit u in units)
        {
            if (u.GetArmyId() == armyId) score += 4.0f;
            else score -= 4.0f;
        }
        if(armyId == 1)
        {
            score += player1.GetMorale();
            score -= player2.GetMorale();
        }
        else
        {
            score -= player1.GetMorale();
            score += player2.GetMorale();
        }
        
        return score;
    }

    public string GetArmyName(int armyId)
    {
        if (armyId == 1) return player1.GetArmyName();
        else return player2.GetArmyName();
    }

    public string GetKeyFieldName(int keyFieldId)
    {
        foreach(KeyField kf in keyFields)
        {
            if (kf.GetFieldId() == keyFieldId) return kf.GetFieldName();
        }
        return null;
    }
}
