using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureAttack : Attack
{
    public CaptureAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, string aType, string dType) : base(aId, state, army, o, keyField, isKFTaken, tId, p)
    {
        attackName = "Capture";
        attackDiceNumber = 0;
        defenceDiceNumber = 0;
    }

    public CaptureAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, int aNum, int dNum) : base(aId, state, army, o, keyField, isKFTaken, tId, p)
    {
        attackName = "Capture";
        attackDiceNumber = aNum;
        defenceDiceNumber = dNum;
    }

    public override void SpecialOutcome(ref StateChange sc)
    {
        if (keyFieldId != 0 && !isKeyFieldTaken)
        {
            sc.keyFieldChangeId = keyFieldId;
            sc.keyFieldNewOccupantId = owner.GetArmyId();
        }
        sc.specialOutcomeDescription = GetSpecialOutcomeDescription();
    }

    public override string GetSpecialOutcomeDescription()
    {
        if (keyFieldId != 0 && !isKeyFieldTaken)
        {
            return BattleManager.Instance.GetKeyFieldName(keyFieldId) + " captured! \n (+1 attack die)";
        }
        else return "";
    }

    public override List<StateChange> GetOutcomes()
    {
        List<StateChange> stc;
        StateChange sc;
        stc = new List<StateChange>();
        sc = new StateChange();
        sc.changeProbability = 1.0f;
        sc.attackerId = owner.GetUnitId();
        sc.defenderId = targetId;
        SpecialOutcome(ref sc);
        stc.Add(sc);
        return stc;
    }

    public override void MakeAttack()
    {
        StateChange result = new StateChange();
        result.attackerId = owner.GetUnitId();
        result.defenderId = targetId;
        SpecialOutcome(ref result);
        BattleManager.Instance.hasTurnOwnerAttacked = true;
        EventManager.RaiseEventOnDiceResult(result);
    }

    public override Attack GetCopy(Unit o)
    {
        CaptureAttack nca;
        nca = new CaptureAttack(attackId, isActiveState, armyId, o, keyFieldId, isKeyFieldTaken, targetId, arrowPosition, attackDiceNumber, defenceDiceNumber);
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
}
