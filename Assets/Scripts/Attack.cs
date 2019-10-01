using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Attack
{
    protected int attackId;
    protected int targetId;
    protected Unit target;
    protected bool isActiveState;
    protected int armyId;
    protected Unit owner;
    protected List<int> activatesAttacks;
    protected List<int> deactivatesAttacks;
    protected int keyFieldId;
    protected Vector3 arrowPosition;    // współrzędne pozycji strzałki przedstawiającej ten atak


    public Attack(int aId, bool state, int army, Unit o, int keyField, int tId, Vector3 p) // konstruktor
    {
        attackId = aId;
        isActiveState = state;
        armyId = army;
        owner = o;
        activatesAttacks = new List<int>();
        deactivatesAttacks = new List<int>();
        keyFieldId = keyField;
        targetId = tId;
        target = null;
        arrowPosition = p;
    }

    protected Attack(Attack pattern, Unit o) // konstruktor kopiujący
    {
        attackId = pattern.attackId;
        isActiveState = pattern.isActiveState;
        armyId = pattern.armyId;
        activatesAttacks = new List<int>();
        foreach(int i in activatesAttacks)
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
        target = null;
        arrowPosition = pattern.arrowPosition;
    }

    public void Activate()
    {
        Assert.IsFalse(isActiveState);
        isActiveState = true;
    }

    public void Deactivate()
    {
        Assert.IsTrue(isActiveState);
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

    public bool CheckAndSetTarget( Unit u)    // sprawdza czy podana jednostka ma id takie jak id celu ataku, jeżeli tak to ustawia referencję na cel ataku i zwraca true jeżeli nie, zwraca false
    {
        if (u.MyId(targetId))
        {
            target = u;
            return true;
        }
        else return false;
    }

    public Vector3 GetPosition()
    {
        return arrowPosition;
    }

    public abstract StateChange MakeAttack();
    public abstract List<StateChange> getOutcomes();
    public abstract Attack GetCopy(Unit o);

}
