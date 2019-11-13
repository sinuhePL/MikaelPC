using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardState
{
    private List<Unit> units;
    private List<KeyField> keyFields;
    private Army player1;
    private Army player2;

    public BoardState(Army army1, Army army2)
    {
        units = new List<Unit>();
        keyFields = new List<KeyField>();
        player1 = army1;
        player2 = army2;
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
        // ustawia referenceje celów ataków
        foreach(Unit _unit in units)
        {
            _unit.SetAttacksTargets(units);
        }
    }

    public List<StateChange> GetPossibleChanges(int attackingPlayer)   // zwraca wszystkie możliwe wyniki wszystkich aktywnych ataków gracza który 
    {
        List<StateChange> resultList = new List<StateChange>();
        foreach(Unit _unit in units)
        {
            if(_unit.GetArmyId() == attackingPlayer) resultList.AddRange(_unit.GetAttackOutcomes());
        }
        return resultList;
    }

    public Unit GetUnit(int idUnit)
    {
        foreach(Unit u in units)
        {
            if (u.MyId(idUnit)) return u;
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
        // wprowadza rezultat ataku do oddziału atakującego
        u = GetUnit(change.attackerId); 
        u.ChangeStrength(change.attackerStrengthChange);
        u.ChangeMorale(change.attackerMoraleChanged);
        u.ChangeArmyTestModifier(change.attackerRouteTestModifierChange);
        foreach(int i in change.activatedAttacks)   // przesyła wszystkie aktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
        {
            u.ActivateAttack(i);
        }
        foreach(int i in change.deactivatedAttacks) // przesyła wszystkie deaktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
        {
            u.DeactivateAttack(i);
        }
        // wprowadza rezultat ataku do oddziału zaatakowanego
        u = GetUnit(change.defenderId);
        u.ChangeStrength(change.defenderStrengthChange);
        u.ChangeMorale(change.defenderMoraleChanged);
        u.ChangeArmyTestModifier(change.defenderRouteTestModifierChange);
        foreach (int i in change.activatedAttacks)  // przesyła wszystkie aktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
        {
            u.ActivateAttack(i);
        }
        foreach (int i in change.deactivatedAttacks)    // przesyła wszystkie deaktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
        {
            u.DeactivateAttack(i);
        }
        foreach(KeyField kf in keyFields)   // wprowadza zmianę własiciela pola kluczowego
        {
            if (kf.GetFieldId() == change.keyFieldChangeId) kf.SetOccupant(change.keyFieldNewOccupantId);
        }
        return change.winnerId;
    }

    public void AddUnit(Unit u)
    {
        units.Add(u);
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

    public void DeactivateAttacksOnUnit(int unitId)
    { 
        foreach(Unit u in units)
        {
            u.DeactivateAttackOnUnit(unitId);
        }
    }
}
