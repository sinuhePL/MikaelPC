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

    private void DeleteAttacksOnUnit(int unitId)
    {
        foreach (Unit u in units)
        {
            u.DeleteAttackOnUnit(unitId);
        }
    }

    private void PromoteAttacksOnUnit(int unitId)
    {
        foreach (Unit u in units)
        {
            u.PromoteAttackOnUnit(unitId);
        }
    }

    private Unit GetUnitByTileId(int tId)
    {
        foreach(Unit u in units)
        {
            if (u.GetTileId() == tId) return u;
        }
        return null;
    }

    public List<Attack> GetAttacksByArrowId(int arrowId)
    {
        List<Attack> result;

        result = new List<Attack>();
        foreach (Unit u in units)
        {
            result.AddRange(u.GetAttacksByArrowId(arrowId));
            result.AddRange(u.GetAdditionalAttacksByArrowId(arrowId));
        }
        return result;
    }

    /*public Unit GetForwardAttacker(int unitId)
    {
        foreach (Unit u in units)
        {
            if (u.CheckIfForwardAttackOn(unitId)) return u;
        }
        return null;
    }*/

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
                if (_unit.GetArmyId() == attackingPlayer && _unit.IsAlive) resultList.AddRange(_unit.GetActiveAttacksIds());
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

    public Attack GetAttackById(int idAttack)
    {
        Attack tempAttack = null;
        foreach(Unit u in units)
        {
            tempAttack = u.GetAttackById(idAttack);
            if (tempAttack != null) return tempAttack;
        }
        return null;
    }

    public int ChangeState(StateChange change)  // zmienia stan planszy zgodnie z definicją zmiany
    {
        Unit u, u2;
        KeyField kf;
        Attack a;
        int army1ActiveCount, army2ActiveCount, oldKeyFieldOwner = 0, unitToMove = 0, tempUnitId, opposingUnitId;
        // deactivated side attacks not used this turn
        foreach (Unit un in units)
        {
            un.DeactivateSideAttacks();
        }
        // activates and deactivates attacks
        if (change.activatedAttacks.Count > 0)
        {
            foreach (Unit un in units)
            {
                foreach (int i in change.activatedAttacks)   // przesyła wszystkie aktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
                {
                    if (un.UnitMoved())
                    {
                        if(change.defenderId == un.GetUnitId()) un.UnblockAttack(i, change.attackerId);
                        un.ActivateAttack(i);
                    }
                }
            }
        }
        if (change.deactivatedAttacks.Count > 0)
        {
            foreach (Unit un in units)
            {
                foreach (int i in change.deactivatedAttacks) // przesyła wszystkie deaktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
                {
                    un.DeactivateAttack(i);
                }
            }
        }
        //blocks attacks
        if (change.blockedAttacks.Count > 0)
        {
            foreach (Unit un in units)
            {
                foreach (int i in change.blockedAttacks) // przesyła wszystkie deaktywowane ataki do klasy jednostki, jednostka odrzuci ataki do niej nienależące
                {
                    un.BlockAttack(i, change.attackerId);
                }
            }
        }

        // wprowadza rezultat ataku do oddziału atakującego
        u = GetUnit(change.attackerId);
        // increase morale of heavy cavalry neighbours after killing enemy squad
        if((u.GetUnitType() == "Gendarmes" || u.GetUnitType() == "Imperial Cavalery") && change.defenderStrengthChange < 0)
        {
            u2 = GetUnitByTileId(u.GetTileId() + BattleManager.Instance.boardHeight);
            if (u2 != null) u2.ChangeMorale(1);
            u2 = GetUnitByTileId(u.GetTileId() - BattleManager.Instance.boardHeight);
            if (u2 != null) u2.ChangeMorale(1);
        }
        unitToMove = u.ChangeStrength(change.attackerStrengthChange);
        if (!u.IsAlive)
        {
            foreach (Unit un in units)
            {
                un.UnblockAttacks(u.GetUnitId());
            }
            tempUnitId = u.GetNotBlockedTarget();
            if(tempUnitId != 0)
            {
                u2 = GetUnit(tempUnitId);
                u2.UnblockAttacks(u2.GetUnitId());    // applies to pikeman targets
            }
            DeactivateAttacksOnUnit(u.GetUnitId());
            if (unitToMove != 0)
            {
                u2 = GetUnit(unitToMove);   // unit that moves from second to first line
                u2.MoveToFrontLine();
                PromoteAttacksOnUnit(unitToMove);
            }
            else
            {
                opposingUnitId = BattleManager.Instance.GetOpposingUnitId(change.attackerId);
                if (opposingUnitId != 0)
                {
                    u2 = GetUnit(opposingUnitId);
                    if (u2.GetTileId() != 6 && u2.GetTileId() != 8) u2.ActivateOtherAttacks(change.attackerId);
                }
            }
        }
        else
        {
            unitToMove = u.ChangeMorale(change.attackerMoraleChanged);
            if (!u.IsAlive)
            {
                foreach (Unit un in units)
                {
                    un.UnblockAttacks(u.GetUnitId());
                }
                tempUnitId = u.GetNotBlockedTarget();
                if (tempUnitId != 0)
                {
                    u2 = GetUnit(tempUnitId);
                    u2.UnblockAttacks(u2.GetUnitId());    // applies to pikeman targets
                }
                DeactivateAttacksOnUnit(u.GetUnitId());
                if (unitToMove != 0)
                {
                    u2 = GetUnit(unitToMove);   // unit that moves from second to first line
                    u2.MoveToFrontLine();
                    PromoteAttacksOnUnit(unitToMove);
                }
                else
                {
                    opposingUnitId = BattleManager.Instance.GetOpposingUnitId(change.attackerId);
                    if (opposingUnitId != 0)
                    {
                        u2 = GetUnit(opposingUnitId);
                        if (u2.GetTileId() != 6 && u2.GetTileId() != 8) u2.ActivateOtherAttacks(change.attackerId);
                    }
                }
            }
        }
        if(change.keyFieldChangeId !=0)
        {
            kf = GetKeyField(change.keyFieldChangeId);
            oldKeyFieldOwner = kf.GetOccupant();
            kf.SetOccupant(u.GetArmyId());
            foreach(Unit loopUnit in units)
            {
                if(loopUnit.GetArmyId() == u.GetArmyId())
                {
                    a = null;
                    a = loopUnit.GetAttackOnKeyField(change.keyFieldChangeId);
                    if(a != null && a.GetName() != "Capture" && a.GetName() != "Aim")
                    {
                        a.keyFieldTaken = true;
                        a.ChangeAttack(1);
                    }
                }
            }
        }
        // wprowadza rezultat ataku do oddziału zaatakowanego
        if (change.defenderId != 0)
        {
            u = GetUnit(change.defenderId);
            // increase morale of heavy cavalry neighbours after killing enemy squad
            if ((u.GetUnitType() == "Gendarmes" || u.GetUnitType() == "Imperial Cavalery") && change.attackerStrengthChange < 0)
            {
                u2 = GetUnitByTileId(u.GetTileId() + BattleManager.Instance.boardHeight);
                if (u2 != null) u2.ChangeMorale(1);
                u2 = GetUnitByTileId(u.GetTileId() - BattleManager.Instance.boardHeight);
                if (u2 != null) u2.ChangeMorale(1);
            }
            unitToMove = u.ChangeStrength(change.defenderStrengthChange);
            if (!u.IsAlive)
            {
                foreach (Unit un in units)
                {
                    un.UnblockAttacks(u.GetUnitId());
                }
                tempUnitId = u.GetNotBlockedTarget();
                if (tempUnitId != 0)
                {
                    u2 = GetUnit(tempUnitId);
                    u2.UnblockAttacks(u2.GetUnitId());    // applies to pikeman targets
                }
                DeactivateAttacksOnUnit(u.GetUnitId());
                if (unitToMove != 0)
                {
                    u2 = GetUnit(unitToMove);   // unit that moves from second to first line
                    u2.MoveToFrontLine();
                    PromoteAttacksOnUnit(unitToMove);
                }
                else
                {
                    opposingUnitId = BattleManager.Instance.GetOpposingUnitId(change.defenderId);
                    if (opposingUnitId != 0)
                    {
                        u2 = GetUnit(opposingUnitId);
                        if (u2.GetTileId() != 6 && u2.GetTileId() != 8) u2.ActivateOtherAttacks(change.attackerId);
                    }
                }
            }
            else
            {
                unitToMove = u.ChangeMorale(change.defenderMoraleChanged);
                if (!u.IsAlive)
                {
                    foreach (Unit un in units)
                    {
                        un.UnblockAttacks(u.GetUnitId());
                    }
                    tempUnitId = u.GetNotBlockedTarget();
                    if (tempUnitId != 0)
                    {
                        u2 = GetUnit(tempUnitId);
                        u2.UnblockAttacks(u2.GetUnitId());    // applies to pikeman targets
                    }
                    DeactivateAttacksOnUnit(u.GetUnitId());
                    if (unitToMove != 0)
                    {
                        u2 = GetUnit(unitToMove);   // unit that moves from second to first line
                        u2.MoveToFrontLine();
                        PromoteAttacksOnUnit(unitToMove);
                    }
                    else
                    {
                        opposingUnitId = BattleManager.Instance.GetOpposingUnitId(change.defenderId);
                        if (opposingUnitId != 0)
                        {
                            u2 = GetUnit(opposingUnitId);
                            if (u2.GetTileId() != 6 && u2.GetTileId() != 8) u2.ActivateOtherAttacks(change.attackerId);
                        }
                    }
                }
            }
            if (change.keyFieldChangeId != 0)
            {
                if (oldKeyFieldOwner != 0)
                {
                    foreach (Unit loopUnit in units)
                    {
                        if (loopUnit.GetArmyId() == oldKeyFieldOwner)
                        {
                            a = null;
                            a = loopUnit.GetAttackOnKeyField(change.keyFieldChangeId);
                            if (a != null && a.GetName() != "Capture" && a.GetName() != "Aim")
                            {
                                a.keyFieldTaken = false;
                                a.ChangeAttack(-1);
                            }
                        }
                    }
                }
            }
        }
        army1ActiveCount = 0;
        army2ActiveCount = 0;
        if (units != null)
        {
            foreach (Unit un in units)
            {
                if (un.IsAlive && un.GetArmyId() == 1) army1ActiveCount++;
                if (un.IsAlive && un.GetArmyId() == 2) army2ActiveCount++;
            }
        }
        if (army1ActiveCount == 0 && army2ActiveCount == 0) winnerId = -1;
        else if (army1ActiveCount == 0) winnerId = 1;
        else if (army2ActiveCount == 0) winnerId = 2;
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

    public List<int> GetAttacksActivating(int attackId)
    {
        List<int> result = new List<int>();
        foreach(Unit u in units)
        {
            result.AddRange(u.GetAttacksActivating(attackId));
        }
        return result;
    }
}
