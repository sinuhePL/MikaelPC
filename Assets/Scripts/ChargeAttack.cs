using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{
    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, int tId, Vector3 p) : base(aId, state, army, o, keyField, tId, p)
    {

    }

    public override StateChange MakeAttack()
    {

        StateChange st = new StateChange();
        return st;

    }

    public override List<StateChange> getOutcomes()
    {
        List<StateChange> stc;
        stc = new List<StateChange>();
        return stc;
    }

    public override Attack GetCopy(Unit o)
    {
        ChargeAttack nca;
        nca = new ChargeAttack(attackId, isActiveState, armyId, o, keyFieldId, targetId, arrowPosition);
        return (Attack)nca;
    }
}
