using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Attack
{
    protected int attackId;
    protected int targetId;
    //protected Unit target;
    protected bool isActiveState;
    protected int armyId;
    protected Unit owner;
    protected List<int> activatesAttacks;
    protected List<int> deactivatesAttacks;
    protected int keyFieldId;
    protected Vector3 arrowPosition;    // coordinates of arrow representing this attack - sets place for dice throw
    protected string attackName;
    protected int attackDiceNumber;
    protected int defenceDiceNumber;
    protected bool isKeyFieldTaken;
    protected int specialOutcomeType;

    public bool keyFieldTaken
    {
        get { return isKeyFieldTaken; }
        set { isKeyFieldTaken = value; }
    }

    protected Attack(Attack pattern, Unit o) // copying constructor
    {
        attackId = pattern.attackId;
        isActiveState = pattern.isActiveState;
        armyId = pattern.armyId;
        activatesAttacks = new List<int>();
        foreach (int i in activatesAttacks)
        {
            activatesAttacks.Add(i);
        }
        deactivatesAttacks = new List<int>();
        foreach (int i in deactivatesAttacks)
        {
            deactivatesAttacks.Add(i);
        }
        keyFieldId = pattern.keyFieldId;
        owner = o;
        targetId = pattern.targetId;
        //target = null;
        arrowPosition = pattern.arrowPosition;
        attackDiceNumber = pattern.attackDiceNumber;
        defenceDiceNumber = pattern.defenceDiceNumber;
        isKeyFieldTaken = pattern.isKeyFieldTaken;
        specialOutcomeType = pattern.specialOutcomeType;
    }

    public string GetName()
    {
        return attackName;
    }

    public int GetAttackDiceNumber()
    {
        return attackDiceNumber;
    }

    public int GetDefenceDiceNumber()
    {
        return defenceDiceNumber;
    }

    public Attack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p) // constructor
    {
        attackId = aId;
        isActiveState = state;
        armyId = army;
        owner = o;
        activatesAttacks = new List<int>();
        deactivatesAttacks = new List<int>();
        keyFieldId = keyField;
        isKeyFieldTaken = isKFTaken;
        targetId = tId;
        //target = null;
        arrowPosition = p;
        attackDiceNumber = 0;
        defenceDiceNumber = 0;
    }

    public void ChangeAttack(int amount)
    {
        attackDiceNumber += amount;
    }

    public void ChangeDefence(int amount)
    {
        if(defenceDiceNumber + amount >= 2) defenceDiceNumber += amount;
    }

    public void Activate()
    {
        //Assert.IsFalse(isActiveState);
        isActiveState = true;
    }

    public void Deactivate()
    {
        isActiveState = false;
    }

    public bool IsActive()
    {
        return isActiveState;
    }

    public int GetId()
    {
        return attackId;
    }

    public void AddActivatedAttackId(int i)
    {
        activatesAttacks.Add(i);
    }

    public void AddDeactivatedAttackId(int i)
    {
        deactivatesAttacks.Add(i);
    }

    /*public bool CheckAndSetTarget( Unit u)    // sprawdza czy podana jednostka ma id takie jak id celu ataku, jeżeli tak to ustawia referencję na cel ataku i zwraca true jeżeli nie, zwraca false
    {
        if (u.MyId(targetId))
        {
            target = u;
            return true;
        }
        else return false;
    }*/

    public Vector3 GetPosition()
    {
        return arrowPosition;
    }

    public int GetArmyId()
    {
        return armyId;
    }

    /*public Unit GetTarget()
    {
        return target;
    }*/

    //tymczasowo
    public int GetTargetId()
    {
        return targetId;
    }

    public Unit GetOwner()
    {
        return owner;
    }

    public int GetKeyFieldId()
    {
        return keyFieldId;
    }

    public bool CheckIfActivates(int myAttackId)
    {
        return activatesAttacks.Contains(myAttackId);
    }

    public List<int> GetActivatedAttacks()
    {
        return activatesAttacks;
    }

    public List<int> GetDeactivatedAttacks()
    {
        return deactivatesAttacks;
    }

    public virtual List<StateChange> GetOutcomes()
    {
        List<StateChange> stc;
        StateChange sc;

        stc = new List<StateChange>();
        if (attackDiceNumber == 2 && defenceDiceNumber == 2)
        {
            //                                                am  dm  as ds kf kfo
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0625f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.027778f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.159722f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.027778f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.012346f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.070988f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.152778f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.067901f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.390432f);
            stc.Add(sc);
            // adding result for special attack
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006944f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003086f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.017747f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 2)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.055556f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.319444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.064815f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028807f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.165638f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.042824f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.019033f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.109439f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.017361f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007716f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.044367f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 2 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.064815f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.060185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.055556f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028807f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.026749f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.305556f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.158436f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.147119f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.013889f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007202f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006687f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.25f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.12963f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.12037f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.12963f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.067215f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.062414f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.085648f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.04441f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.041238f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.034722f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.018004f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016718f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 2)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1041667f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.046296f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.266204f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.052469f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.02332f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.134088f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015625f);
            stc.Add(sc);
            /* sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006944f);
             stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.039931f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.041667f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.018519f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.106481f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01794f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007973f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.045846f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010417f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00463f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.02662f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003086f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001372f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007888f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00463f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002058f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011831f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.2083333f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.108025f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.100309f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.104938f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.054412f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.050526f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03588f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.018604f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.017275f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016204f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015046f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.083333f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.04321f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.040123f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.020833f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010802f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010031f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006173f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003201f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002972f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.009259f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004801f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004458f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1909722f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.095165f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0299f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.096193f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.047935f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015061f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03289f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016389f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.005149f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.026042f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.069444f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00514403f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.013117f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.034979f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002591f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004485f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01196f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000886f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028646f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.014275f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004485f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003906f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010417f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000772f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.076389f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.038066f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01196f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010417f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.027778f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002058f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.019097f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.009516f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00299f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002604f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006944f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000514f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.005658f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00282f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000886f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000772f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002058f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000152f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008488f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00423f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001329f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001157f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003086f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000229f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 2)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0347222f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015432f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.088735f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015432f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006859f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.039438f);
            stc.Add(sc);
            // adding result for special attack
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004694f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002086f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011996f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.046875f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.020833f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.119792f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.092593f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.041152f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.236626f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028935f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01286f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.073945f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011317f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00503f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028921f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015432f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006859f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.039438f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0694444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.036008f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.033436f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.030864f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016004f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.014861f);
            stc.Add(sc);
            // adding result for special attack
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.009388f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004868f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00452f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.09375f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.048611f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.045139f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.185185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.096022f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.089163f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.05787f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.030007f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.027864f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.022634f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011736f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010898f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.030864f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016004f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.014861f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0636574f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.031722f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.009967f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028292f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.014098f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00443f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008606f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004288f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001347f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008681f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.023148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00171468f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003858f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010288f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00076f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001173f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003129f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00023f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.085937f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.042824f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.013455f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011719f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03125f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002315f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.169753f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.084591f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.026578f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.023148f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.061728f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004572f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.053048f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.026435f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008305f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007234f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01929f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001429f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.020748f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010339f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003248f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002829f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007545f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00056f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028292f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.014098f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00443f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003858f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010288f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00076f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0353652f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.017147f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002608f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015718f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007621f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001159f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004781f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002318f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00035f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.026042f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.05144f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00628715f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011574f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.022862f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002794f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00352f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006954f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00085f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.047743f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.023148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00352f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.035156f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.069444f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008488f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.094307f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.045725f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006954f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.069444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.137174f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016766f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.029471f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.014289f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002173f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.021701f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.042867f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.005239f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011526f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.005589f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00085f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008488f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016766f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002049f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015718f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007621f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001159f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011574f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.022862f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002794f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 2 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1145833f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.057099f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01794f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.050926f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.025377f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007973f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.012731f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006344f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001993f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.280093f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.139575f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.043853f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015625f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.041667f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00308642f);
            stc.Add(sc);*/
            /*sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006944f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.018519f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001372f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001736f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00463f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00034f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.038194f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.101852f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007545f);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 2 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0636574f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.030864f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004694f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028292f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.013717f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002086f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007073f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003429f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.000522f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.155607f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.075446f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011474f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.046875f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.092593f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01131687f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.020833f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.041152f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00503f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.005208f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010288f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001257f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.114583f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.226337f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.027663f);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.2291667f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.114198f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03588f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.118827f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.059214f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.018604f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.031829f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015861f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004983f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.078511f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.039123f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.012292f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.083333f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00617284f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.016204f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.04321f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003201f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00434f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011574f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00086f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.010706f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.028549f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002115f);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1273148f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.061728f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.009388f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.066015f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.032007f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004868f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.017683f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008573f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001304f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.043617f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.021148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003216f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.09375f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.185185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.02263374f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.048611f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.096022f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011736f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.013021f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.02572f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003144f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.032118f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.063443f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007754f);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1060957f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.05144f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007823f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.053441f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.025911f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00394f);
            stc.Add(sc);*/
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.018272f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.008859f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001347f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.078125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.154321f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01886145f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.039352f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.077732f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.009501f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.013455f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.026578f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003248f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015914f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007716f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001173f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.011719f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.023148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002829f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.042438f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.020576f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003129f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.03125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.061728f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007545f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01061f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.005144f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00078f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.007813f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.015432f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001886f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003144f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.001524f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00023f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002315f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004572f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00056f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.004715f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.002286f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00035f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.003472f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.006859f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.00084f);
            SpecialOutcome(ref sc);
            stc.Add(sc);*/
        }
        return stc;
    }

    public virtual void MakeAttack()
    {
        int throw1 = 0, throw2 = 0;
        Dice.Clear();
        if (armyId == 1)
        {
            if (attackDiceNumber > 0) throw1 = Dice.Roll(attackDiceNumber.ToString() + "d6", "d6-attackblue", arrowPosition + new Vector3(-2.0f, 2.0f, -1.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
            if (defenceDiceNumber > 0) throw2 = Dice.Roll(defenceDiceNumber.ToString() + "d6", "d6-defenceyellow", arrowPosition + new Vector3(-2.0f, 2.0f, -2.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
        }
        else
        {
            if (attackDiceNumber > 0) throw1 = Dice.Roll(attackDiceNumber.ToString() + "d6", "d6-attackyellow", arrowPosition + new Vector3(-2.0f, 2.0f, -2.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
            if (defenceDiceNumber > 0) throw2 = Dice.Roll(defenceDiceNumber.ToString() + "d6", "d6-defenceblue", arrowPosition + new Vector3(-2.0f, 2.0f, -1.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
        }
        BattleManager.Instance.StartCoroutine(WaitForDice(throw1, throw2, attackId));
    }

    // Waits for dice to stop rolling
    protected IEnumerator WaitForDice(int throw1Id, int throw2Id, int attackId)
    {
        string result1 = "", result2 = "";
        bool isSpecialOutcome;

        while (Dice.rolling)
        {
            yield return null;
        }
        if (throw1Id > 0) result1 = Dice.AsString("d6", throw1Id);
        if (throw2Id > 0) result2 = Dice.AsString("d6", throw2Id);
        Debug.Log(result1);
        Debug.Log(result2);
        StateChange result = new StateChange();
        string[] throw1Hits, throw2Hits;
        int attackStrengthHit = 0, attackMoraleHit = 0, defenceStrengthHit = 0, defenceMoraleHit = 0;
        if (!(result1.Contains("?") || result2.Contains("?") || result1.Length < 13 && result1.Length > 0 || result2.Length < 12 && result2.Length > 0))    // sprawdzenie czy rzut był udany/bezbłędny
        {
            if (throw1Id > 0) throw1Hits = Dice.ResultForThrow("d6", throw1Id);
            else throw1Hits = new string[0];
            if (throw2Id > 0) throw2Hits = Dice.ResultForThrow("d6", throw2Id);
            else throw2Hits = new string[0];
            if (throw1Hits != null && throw2Hits != null)
            {
                isSpecialOutcome = false;
                for (int i = 0; i < throw1Hits.Length; i++)
                {
                    if (throw1Hits[i] == "S") attackStrengthHit++;
                    if (throw1Hits[i] == "M") attackMoraleHit++;
                    if (throw1Hits[i] == "*") isSpecialOutcome = true;
                }
                for (int i = 0; i < throw2Hits.Length; i++)
                {
                    if (throw2Hits[i] == "S") defenceStrengthHit++;
                    if (throw2Hits[i] == "M") defenceMoraleHit++;
                }
                result.attackerId = owner.GetUnitId();
                result.defenderId = targetId;
                result.attackerMoraleChanged = -defenceMoraleHit;
                result.attackerStrengthChange = -defenceStrengthHit;
                result.defenderMoraleChanged = -attackMoraleHit;
                result.defenderStrengthChange = -attackStrengthHit;
                if (isSpecialOutcome) SpecialOutcome(ref result);
                result.activatedAttacks = new List<int>(GetActivatedAttacks().ToArray());
                result.deactivatedAttacks = new List<int>(GetDeactivatedAttacks().ToArray());
                Debug.Log("Attack inflicted " + attackStrengthHit + " strength casualty and " + attackMoraleHit + " morale loss for defender.");
                Debug.Log("Defence inflicted " + defenceStrengthHit + " strength casualty and " + defenceMoraleHit + " morale loss for attacker.");
                BattleManager.Instance.hasTurnOwnerAttacked = true;
                yield return new WaitForSeconds(1.5f);
                EventManager.RaiseEventOnDiceResult(result);
                Dice.Clear();
            }
        }
        else
        {
            Debug.Log("Błąd przy rzucie");
            MakeAttack();
        }
    }

    public virtual void SpecialAction()
    {

    }

    //public abstract StateChange ApplyAttack(int attackerStrengthHits, int attackerMoraleHits, int defenderStrengthHits, int defenderMoraleHits, float probability, int winner);
    public abstract Attack GetCopy(Unit o);
    public abstract void SpecialOutcome(ref StateChange sc);
    public abstract string GetSpecialOutcomeDescription();
}
