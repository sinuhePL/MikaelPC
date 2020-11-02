using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombardAttack : Attack
{
    protected bool isTargetAimed;
    public BombardAttack(int aId, int arrId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, string aType, string dType, bool isAimed) : base(aId, arrId, state, army, o, keyField, isKFTaken, tId, p)
    {
        isTargetAimed = isAimed;
        if(isTargetAimed) attackName = "Bombard";
        else attackName = "Aim";
        switch (dType)
        {
            case "Gendarmes":
                attackDiceNumber = 3;
                defenceDiceNumber = 0;
                specialOutcomeType = 5;
                break;
            case "Landsknechte":
                attackDiceNumber = 4;
                defenceDiceNumber = 0;
                specialOutcomeType = 10;
                break;
            case "Suisse":
                attackDiceNumber = 4;
                defenceDiceNumber = 0;
                specialOutcomeType = 10;
                break;
            case "Imperial Cavalery":
                attackDiceNumber = 3;
                defenceDiceNumber = 0;
                specialOutcomeType = 8;
                break;
            case "Arquebusiers":
                attackDiceNumber = 3;
                defenceDiceNumber = 0;
                specialOutcomeType = 7;
                break;
            case "Artillery":
                attackDiceNumber = 3;
                defenceDiceNumber = 0;
                specialOutcomeType = 6;
                break;
            case "Stradioti":
                attackDiceNumber = 2;
                defenceDiceNumber = 0;
                specialOutcomeType = 6;
                break;
            case "Coustilliers":
                attackDiceNumber = 2;
                defenceDiceNumber = 0;
                specialOutcomeType = 6;
                break;
        }
    }

    public BombardAttack(int aId, int arrId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, int aNum, int dNum, bool isAimed) : base(aId, arrId, state, army, o, keyField, isKFTaken, tId, p)
    {
        isTargetAimed = isAimed;
        if (isTargetAimed) attackName = "Bombard";
        else attackName = "Aim";
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
        else
        {
            if (specialOutcomeType == 1)
            {
                sc.attackerMoraleChanged = 0;
                sc.attackerStrengthChange = 0;
            }
            if (specialOutcomeType == 2)
            {
                sc.attackerMoraleChanged = 0;
                sc.attackerStrengthChange = 0;
                sc.defenderMoraleChanged--;
            }
            if (specialOutcomeType == 3)
            {
                sc.attackerMoraleChanged = 1;
                sc.attackerStrengthChange = 0;
            }
            if (specialOutcomeType == 4)
            {
                sc.attackerMoraleChanged = 0;
                sc.attackerStrengthChange = 0;
                sc.defenderStrengthChange--;
            }
            if (specialOutcomeType == 5)
            {
                sc.attackerMoraleChanged++;
                sc.defenderMoraleChanged--;
            }
            if (specialOutcomeType == 6)
            {
                sc.defenderMoraleChanged = sc.defenderMoraleChanged - 2;
            }
            if (specialOutcomeType == 7)
            {
                sc.attackerMoraleChanged++;
            }
            if (specialOutcomeType == 8)
            {
                sc.defenderStrengthChange--;
            }
            if (specialOutcomeType == 9)
            {
                sc.attackerMoraleChanged = 1;
                sc.attackerStrengthChange = 0;
                sc.defenderMoraleChanged--;

            }
            if (specialOutcomeType == 10)
            {
                sc.defenderMoraleChanged--;
            }
        }
        sc.specialOutcomeDescription = GetSpecialOutcomeDescription();
    }

    public override string GetSpecialOutcomeDescription()
    {
        if (keyFieldId != 0 && !isKeyFieldTaken)
        {
            return BattleManager.Instance.GetKeyFieldName(keyFieldId) + " captured! \n (+1 attack die)";
        }
        else
        {
            if (specialOutcomeType == 1) return "Defender loses hits";
            if (specialOutcomeType == 2) return "Defender loses hits and 1 morale";
            if (specialOutcomeType == 3) return "Defender loses hits, attacker gains 1 morale";
            if (specialOutcomeType == 4) return "Defender loses hits and 1 strength";
            if (specialOutcomeType == 5) return "Attacker gains 1 morale, defender loses 1 morale";
            if (specialOutcomeType == 6) return "Defender loses 2 morale";
            if (specialOutcomeType == 7) return "Attacker gains 1 morale";
            if (specialOutcomeType == 8) return "Defender loses 1 strength";
            if (specialOutcomeType == 9) return "Attacker gains 1 morale, defender loses 1 morale and hits";
            if (specialOutcomeType == 10) return "Defender loses 1 morale";
        }
        return "";
    }

    public override List<StateChange> GetOutcomes()
    {
        List<StateChange> stc;
        StateChange sc;
        if (isTargetAimed)
        {
            stc = new List<StateChange>();
            if (attackDiceNumber == 2)
            {
                //                                                am  dm  as ds kf kfo
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.25f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.1111111f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.611111111f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.027777777f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
            }
            else if (attackDiceNumber == 3)
            {
                //                                                am  dm  as ds kf kfo
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.5f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.259259259f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.171296297f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.06944444f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
            }
            else if (attackDiceNumber == 4)
            {
                //                                                am  dm  as ds kf kfo
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.41666666f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.2098765432f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.0625f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.012345679f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.16666666f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.041666666f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.0185185185f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.071759259f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
            }
            else if (attackDiceNumber == 5)
            {
                //                                                am  dm  as ds kf kfo
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.13888888f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.0617289351f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.1875f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.045267489f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.37037037f);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.11574f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.06172839f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
                sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), new List<int>(), 0.01877572f);
                SpecialOutcome(ref sc);
                stc.Add(sc);
            }
        }
        else
        {
            stc = new List<StateChange>();
            sc = new StateChange();
            sc.changeProbability = 1.0f;
            sc.attackerId = owner.GetUnitId();
            sc.defenderId = targetId;
            sc.specialOutcomeDescription = "Target aimed";
            owner.OtherAttacksSpecialAction(attackId);
            isTargetAimed = true;
            attackName = "Bombard";
            stc.Add(sc);
        }
        return stc;
    }

    public override void MakeAttack()
    {
        if(!isTargetAimed)
        {
            owner.OtherAttacksSpecialAction(attackId);
            isTargetAimed = true;
            StateChange result = new StateChange();
            result.attackerId = owner.GetUnitId();
            result.defenderId = targetId;
            result.specialOutcomeDescription = "Target aimed";
            attackName = "Bombard";
            BattleManager.Instance.hasTurnOwnerAttacked = true;
            BattleManager.Instance.isInputBlocked = false;
            EventManager.RaiseEventOnDiceResult(result);
        }
        else base.MakeAttack();
    }

    public override void SpecialAction()
    {
        isTargetAimed = false;
        attackName = "Aim";
    }

    public override Attack GetCopy(Unit o)
    {
        BombardAttack nca;
        nca = new BombardAttack(attackId, arrowId, isActiveState, armyId, o, keyFieldId, isKeyFieldTaken, targetId, arrowPosition, attackDiceNumber, defenceDiceNumber, isTargetAimed);
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
