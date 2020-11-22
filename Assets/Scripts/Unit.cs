using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Unit
{
    private int unitId;
    private string unitType;
    private string unitCommander;
    private int initialStrength;
    private int _strength;
    private int initialMorale;
    private int _morale;
    private int testModifier;
    private bool isAlive;
    private int battleLine;
    private List<Attack> additionalAttacks;
    private List<Attack> unitAttacks;
    private Army owner;
    private int _supportLineUnitId;
    private int tileId;

    private Attack FindAttack(int aId)
    {
        Attack tempA = null;

        foreach (Attack a in unitAttacks)    // szuka ataku o wskazanym Id
        {
            if (a.GetId() == aId) tempA = a;
        }
        return tempA;
    }

    public int strength
    {
        get {return _strength;}
        set {_strength = value;}
    }

    public void SetUnitInSupportLine()
    {
        battleLine = 2;
    }

    public void SetUnitOutsideOfBattlefield()
    {
        battleLine = 3;
    }

    public int morale
    {
        get { return _morale; }
        set { _morale = value; }
    }

    public bool IsAlive
    {
        get => isAlive;
        set => isAlive = value;
    }

    public int supportLineUnitId
    {
        get => _supportLineUnitId;
        set => _supportLineUnitId = value;
    }

    public string GetUnitType()
    {
        return unitType;
    }

    public Unit(Unit pattern, Army a)   // copying constructor
    {
        unitId = pattern.unitId;
        unitType = pattern.unitType;
        unitCommander = pattern.unitCommander;
        initialStrength = pattern.initialStrength;
        strength = pattern.strength;
        initialMorale = pattern.initialMorale;
        morale = pattern.morale;
        testModifier = pattern.testModifier;
        isAlive = pattern.isAlive;
        owner = a;
        supportLineUnitId = pattern.supportLineUnitId;
        battleLine = pattern.battleLine;
        tileId = pattern.tileId;
        unitAttacks = new List<Attack>();
        additionalAttacks = new List<Attack>();
        foreach (Attack at in pattern.unitAttacks)
        {
            unitAttacks.Add(at.GetCopy(this));
        }
        foreach (Attack at in pattern.additionalAttacks)
        {
            additionalAttacks.Add(at.GetCopy(this));
        }
    }

    public Unit(int uId, string uType, int iStrength, int iMorale, Army a, string commander, int tId)  // konstruktor
    {
        unitId = uId;
        unitType = uType;
        initialStrength = iStrength;
        strength = iStrength;
        initialMorale = iMorale;
        morale = iMorale;
        testModifier = 0;           // na razie nie wiem czego będzie dotyczył
        isAlive = true;
        unitAttacks = new List<Attack>();
        additionalAttacks = new List<Attack>();
        owner = a;
        supportLineUnitId = 0;
        battleLine = 1;
        unitCommander = commander;
        tileId = tId;
    }

    /*public StateChange MakeAttack(int aId) // wykonuje atak wskazany przez id ataku przekazane do metody
    {
        StateChange tempCS = new StateChange();
        Attack myAttack = null;

        myAttack = FindAttack(aId);
        if (myAttack != null) return myAttack.MakeAttack();   // zwraca opis zmiany stanu planszy wynikający z przeprowadzonego ataku
        else return tempCS;
    }*/

    public int ChangeStrength(int sc)  //zmienia siłę jednostki o wskazaną wartość i zwraca identyfikator jednostki wsparcia który ją zastępuje gdy została zabita
    {
        Unit tempUnit;

        strength += sc;
        if (strength <= 0)
        {
            isAlive = false;
            owner.ChangeMorale(-morale);
            morale = 0;
            foreach(Attack a in unitAttacks)
            {
                if(a.IsActive()) a.Deactivate();
            }
            if (_supportLineUnitId == 0) return 0;
            else
            {
                tempUnit = BattleManager.Instance.GetUnit(_supportLineUnitId);
                if (tempUnit.battleLine == 2) return _supportLineUnitId;
                else return 0;
            }
        }
        return 0;
    }

    public int ChangeMorale(int mc)    //zmienia morale jednostki o wskazaną wartość i zwraca identyfikator jednostki wsparcia który ją zastępuje gdy została zabita
    {
        Unit tempUnit;

        morale += mc;
        if (morale > initialMorale) morale = initialMorale;
        else owner.ChangeMorale(mc);
        if (morale <= 0)
        {
            isAlive = false;
            strength = 0;
            foreach (Attack a in unitAttacks)
            {
                if(a.IsActive()) a.Deactivate();
            }
            if (_supportLineUnitId == 0) return 0;
            else
            {
                tempUnit = BattleManager.Instance.GetUnit(_supportLineUnitId);
                if (tempUnit.battleLine == 2) return _supportLineUnitId;
                else return 0;
            }
        }
        return 0;
    }

    public void MoveToFrontLine()
    {
        Unit rearGuard;
        battleLine = 1;
        foreach (Attack a in unitAttacks)
        {
            if (a.GetName() == "Charge!" && a.Forward || a.GetName() == "Bombard" || a.GetName() == "Aim") a.Activate();
            if (a.GetName() == "Skirmish") a.Deactivate();
        }
        if (tileId == 11 && _supportLineUnitId != 0)
        {
            rearGuard = BattleManager.Instance.GetUnit(_supportLineUnitId);
            rearGuard.GetAttacksByArrowId(144)[0].Activate(); // attack that moves rear guard to battlefield
        }
    }

    public List<StateChange> GetAttackOutcomes()    // zwraca wszystkie rezultaty wszystkich aktywnych ataków jednostki.
    {
        List<StateChange> tempSTList = new List<StateChange>();

        foreach(Attack a in unitAttacks)    // szuka aktywnych ataków
        {
            if(a.IsActive()) tempSTList.AddRange(a.GetOutcomes());
        }
        return tempSTList;
    }

    public void ActivateAttack(int a)   // aktywuje atak o podanym Id
    {
        Attack tempAttack;

        tempAttack = FindAttack(a);
        if(tempAttack != null && !tempAttack.IsActive()) tempAttack.Activate();
    }

    public void BlockAttack(int at, int blockerId)
    {
        Attack tempAttack;

        tempAttack = FindAttack(at);
        if (tempAttack != null && battleLine == 1) tempAttack.Block(blockerId);
    }

    public void UnblockAttack(int a, int attackerId)
    {
        Attack tempAttack;

        tempAttack = FindAttack(a);
        if (tempAttack != null && tempAttack.GetTargetId() == attackerId && tempAttack.GetName() == "Charge!") tempAttack.UnBlock();
    }

    public void DeactivateAttack(int a)     // deaktywuje atak o podanym Id
    {
        Attack tempAttack;

        tempAttack = FindAttack(a);
        if(tempAttack != null && tempAttack.IsActive()) tempAttack.Deactivate();
    }

    public void AddAttack(Attack newAttack)  // dodaje nowy atak
    {
        unitAttacks.Add(newAttack);
    }

    public void AddAdditionalAttack(Attack newAttack)  // dodaje nowy atak
    {
        additionalAttacks.Add(newAttack);
    }

    public int GetArmyMorale()
    {
        return owner.GetMorale();
    }

    public int GetArmyRouteTestModifier()
    {
        return owner.GetRouteTestModifier();
    }

    public bool ArmyTestSuccessful()
    {
        return owner.MoraleTestSuccessful();
    }

    public bool MyId(int id)    // sprawdza czy jest jednostką i podanym Id
    {
        if (id == unitId) return true;
        else return false;
    }

    public int GetTileId()
    {
        return tileId;
    }

    public int GetArmyId()
    {
        return owner.GetArmyId();
    }

    public int GetUnitId()
    {
        return unitId;
    }

    public string GetCommander()
    {
        return unitCommander;
    }

    public Attack GetAttackById(int idA)
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetId() == idA) return a;
        }
        return null;
    }

    public List<Attack> GetAttacksByArrowId(int idArrow)
    {
        List<Attack> result;

        result = new List<Attack>();
        foreach (Attack a in unitAttacks)
        {
            if (a.GetArrowId() == idArrow) result.Add(a);
        }
        return result;
    }

    public List<Attack> GetAdditionalAttacksByArrowId(int idArrow)
    {
        List<Attack> result;

        result = new List<Attack>();
        foreach (Attack a in additionalAttacks)
        {
            if (a.GetArrowId() == idArrow) result.Add(a);
        }
        return result;
    }

    public void DeactivateAttackOnUnit(int uId)
    {
        foreach(Attack a in unitAttacks)
        {
            if (a.GetTargetId() == uId && a.IsActive()) a.Deactivate();
        }
    }

    public void ActivateAttackOnUnit(int uId)
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetTargetId() == uId && !a.IsActive()) a.Activate();
        }
    }

    public List<int> GetActiveAttacksIds()
    {
        List<int> resultList = new List<int>();
        foreach(Attack a in unitAttacks)
        {
            if (a.IsActive()) resultList.Add(a.GetId());
        }
        return resultList;
    }

    public void ActivateOtherAttacks(int uId) // called when opposing unit destroyed/fled
    {
        Unit u;

        foreach (Attack a in unitAttacks)
        {
            u = BattleManager.Instance.GetUnit(a.GetTargetId());
            if (a.GetName() == "Charge!" && a.GetTargetId() != uId && a.GetTargetId() != 130 && u.IsAlive)
            {
                a.Activate();
                a.ChangeAttack(1);
            }
        }
        foreach (Attack a in additionalAttacks)
        {
            u = BattleManager.Instance.GetUnit(a.GetTargetId());
            if (a.GetName() == "Charge!" && a.GetTargetId() != uId && a.GetTargetId() != 130 && u.IsAlive)
            {
                a.Activate();
                a.ChangeAttack(1);
            }
        }
    }

    public Attack GetAttackOnKeyField(int keyFieldId)
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetKeyFieldId() == keyFieldId) return a;
        }
        return null;
    }

    public List<int> GetAttacksActivating(int attackId)
    {
        List<int> result = new List<int>();
        
        foreach(Attack a in unitAttacks)
        {
            if (a.CheckIfActivatesAttack(attackId)) result.Add(a.GetId());
        }
        foreach (Attack a in additionalAttacks)
        {
            if (a.CheckIfActivatesAttack(attackId)) result.Add(a.GetId());
        }
        return result;
    }

    public void DeactivateSideAttacks()
    {
        foreach(Attack a in unitAttacks)
        {
            if (a.GetName() == "Counter Attack" || a.GetName() == "Capture" || a.GetName() == "Charge!" || (a.GetName() == "Move" && battleLine != 3)) a.DeactivateAsSideAttack();
        }
    }

    public bool UnitMoved()
    {
        if (battleLine == 1) return true;
        else return false;
    }

    public bool UnitArrived()
    {
        if (battleLine == 3) return false;
        else return true;
    }

    public void PromoteAttackOnUnit(int uId)
    {
        foreach (Attack a in additionalAttacks)
        {
            if (a.GetTargetId() == uId)
            {
                if(battleLine == 1 && (a.Forward || a.GetName() == "Aim" || a.GetName() == "Bombard" || a.GetName() == "Skirmish")) a.Activate();
                unitAttacks.Add(a);
            }
        }
    }

    public void DeleteAttackOnUnit(int uId)
    {
        Attack tempAttack;
        do
        {
            tempAttack = null;
            foreach (Attack a in unitAttacks)
            {
                if (a.GetTargetId() == uId) tempAttack = a;
            }
            if (tempAttack != null) unitAttacks.Remove(tempAttack);
        }
        while (tempAttack != null);
    }

    public void OtherAttacksSpecialAction(int myAttackId)
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetId() != myAttackId) a.SpecialAction();
        }
    }

    public void SetOwnAttacksDeactivation()
    {
        foreach (Attack a in unitAttacks)
        {
            if(a.GetName() == "Charge!")
            {
                foreach(Attack a2 in unitAttacks)
                {
                    if (a != a2)
                    {
                        a.AddDeactivatedAttackId(a2.GetId());
                        a.AddBlockedAttackId(a2.GetId());
                    }
                }
                foreach (Attack a2 in additionalAttacks)
                {
                    if (a != a2)
                    {
                        a.AddDeactivatedAttackId(a2.GetId());
                        a.AddBlockedAttackId(a2.GetId());
                    }
                }
            }
        }
        foreach (Attack a in additionalAttacks)
        {
            if (a.GetName() == "Charge!")
            {
                foreach (Attack a2 in unitAttacks)
                {
                    if (a != a2) a.AddDeactivatedAttackId(a2.GetId());
                }
                foreach (Attack a2 in additionalAttacks)
                {
                    if (a != a2) a.AddDeactivatedAttackId(a2.GetId());
                }
            }
        }
    }

    public int GetNotBlockedTarget()    // zwraca id jednostki atakowanej przez niezablokowany atak jeżeli jest tylko jeden taki atak 
    {
        int unblockedAttackNumber;
        Attack tempAttack;

        tempAttack = null;
        unblockedAttackNumber = 0;
        foreach(Attack a in unitAttacks)
        {
            if (!a.IsBlocked())
            {
                unblockedAttackNumber++;
                tempAttack = a;
            }
        }
        if (unblockedAttackNumber == 1 && tempAttack != null) return tempAttack.GetTargetId();
        else return 0;
    }

    public void UnblockAttacks(int blockerId)
    {
        foreach(Attack a in unitAttacks)
        {
            if (a.IsBlocked() && a.GetBlockerId() == blockerId)
            {
                a.UnBlock();
                a.ActivateNotForcedAttack();
            }
        }
    }
}
