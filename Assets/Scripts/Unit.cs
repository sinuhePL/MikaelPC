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
    private bool movedToFrontLine;
    private List<Attack> additionalAttacks;
    private List<Attack> unitAttacks;
    private Army owner;
    private int _supportLineUnitId;

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
        movedToFrontLine = pattern.movedToFrontLine;
        unitAttacks = new List<Attack>();
        foreach(Attack at in pattern.unitAttacks)
        {
            unitAttacks.Add(at.GetCopy(this));
        }
        foreach (Attack at in pattern.additionalAttacks)
        {
            additionalAttacks.Add(at.GetCopy(this));
        }
    }

    public Unit(int uId, string uType, int iStrength, int iMorale, Army a, string commander)  // konstruktor
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
        movedToFrontLine = false;
        unitCommander = commander;
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
            return _supportLineUnitId;
        }
        return 0;
    }

    public void MoveToFrontLine()
    {
        movedToFrontLine = true;
        foreach (Attack a in unitAttacks)
        {
            if (a.GetName() == "Charge!") a.Activate();
            if (a.GetName() == "Skirmish") a.Deactivate();
        }
    }

    public int ChangeMorale(int mc)    //zmienia morale jednostki o wskazaną wartość i zwraca identyfikator jednostki wsparcia który ją zastępuje gdy została zabita
    {
        morale += mc;
        owner.ChangeMorale(mc);
        if (morale <= 0)
        {
            isAlive = false;
            strength = 0;
            foreach (Attack a in unitAttacks)
            {
                if(a.IsActive()) a.Deactivate();
            }
            return _supportLineUnitId;
        }
        return 0;
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

    /*public void SetAttacksTargets(List<Unit> _unitList) // ustawia referencję celów ataków na odpowiednie jednostki bazując na id celu ataku
    {
        foreach(Attack _attack in unitAttacks)
        {
            foreach(Unit _unit in _unitList)
            {
                if (_attack.CheckAndSetTarget(_unit)) break;
            }
        }
    }*/

    public Attack GetAttack(int idAttack)
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetId() == idAttack) return a;
        }
        return null;
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

    /*public bool CheckIfForwardAttackOn(int unitId)
    {
        foreach(Attack a in unitAttacks)
        {
            if (a.IsAttackForward() && a.GetTargetId() == unitId && a.IsActive()) return true;
        }
        return false;
    }*/

    public void ActivateNotForwardAttacks() // called when opposing unit destroyed/fled
    {
        foreach (Attack a in unitAttacks)
        {
            if (a.GetName() != "Charge!")
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

    public List<int> GetAttacksActivating(int myAttackId)
    {
        List<int> result = new List<int>();
        
        foreach(Attack a in unitAttacks)
        {
            if (a.CheckIfActivates(myAttackId)) result.Add(a.GetId());
        }
        return result;
    }

    public void DeactivateCounterAttacks()
    {
        foreach(Attack a in unitAttacks)
        {
            if (a.GetName() == "Counter Attack") a.Deactivate();
        }
    }

    public bool UnitMoved()
    {
        return movedToFrontLine;
    }

    public void PromoteAttackOnUnit(int uId)
    {
        Attack tempAttack;
        tempAttack = null;
        foreach (Attack a in additionalAttacks)
        {
            if (a.GetTargetId() == uId) tempAttack = a;
        }
        if (tempAttack != null)
        {
            tempAttack.Activate();
            unitAttacks.Add(tempAttack);
            additionalAttacks.Remove(tempAttack);
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
}
