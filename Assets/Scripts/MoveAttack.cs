using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAttack : Attack
{
    public MoveAttack(int aId, int arrId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p) : base(aId, arrId, state, army, o, keyField, isKFTaken, tId, p)
    {
        attackName = "Move";
    }

    public override void SpecialOutcome(ref StateChange sc)
    {
        sc.specialOutcomeDescription = GetSpecialOutcomeDescription();
    }

    public override string GetSpecialOutcomeDescription()
    {
        return "French Rear Guard moved to battlefield";
    }

    public override Attack GetCopy(Unit o)
    {
        MoveAttack nca;
        nca = new MoveAttack(attackId, arrowId, isActiveState, armyId, o, keyFieldId, isKeyFieldTaken, targetId, arrowPosition);
        foreach (int i in activatesAttacks)
        {
            nca.AddActivatedAttackId(i);
        }
        foreach (int i in deactivatesAttacks)
        {
            nca.AddDeactivatedAttackId(i);
        }
        return (Attack)nca;
    }

    public override void MakeAttack()
    {
        StateChange result = new StateChange();
        result.attackerId = owner.GetUnitId();
        SpecialOutcome(ref result);
        owner.SetUnitInSupportLine();
        BattleManager.Instance.hasTurnOwnerAttacked = true;
        EventManager.RaiseEventOnDiceResult(result);
    }

    public override List<StateChange> GetOutcomes()
    {
        List<StateChange> stc;
        StateChange sc;
        stc = new List<StateChange>();
        sc = new StateChange();
        sc.changeProbability = 1.0f;
        sc.attackerId = owner.GetUnitId();
        owner.SetUnitInSupportLine();
        sc.specialOutcomeDescription = "French Rear Guard moved to battlefield";
        stc.Add(sc);
        return stc;
    }
}
