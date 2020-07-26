using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterAttack : Attack
{
    public CounterAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, string aType, string dType) : base(aId, state, army, o, keyField, isKFTaken, tId, p)
    {
        attackName = "CounterAttack";
        switch (aType)
        {
            case "Gendarmes":
                attackDiceNumber = 2;
                defenceDiceNumber = 0;
                specialOutcomeType = 5;
                break;
            case "Landsknechte":
                attackDiceNumber = 2;
                defenceDiceNumber = 0;
                specialOutcomeType = 10;
                break;
            case "Suisse":
                attackDiceNumber = 2;
                defenceDiceNumber = 0;
                specialOutcomeType = 10;
                break;
            case "Imperial Cavalery":
                attackDiceNumber = 2;
                defenceDiceNumber = 0;
                specialOutcomeType = 8;
                break;
            case "Arquebusiers":
                attackDiceNumber = 4;
                defenceDiceNumber = 0;
                specialOutcomeType = 7;
                break;
            case "Artillery":
                attackDiceNumber = 3;
                defenceDiceNumber = 0;
                specialOutcomeType = 6;
                break;
        }
    }

    public CounterAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, int aNum, int dNum) : base(aId, state, army, o, keyField, isKFTaken, tId, p)
    {
        attackName = "CounterAttack";
        attackDiceNumber = aNum;
        defenceDiceNumber = dNum;
    }

    public override StateChange ApplyAttack(int attackerStrengthHits, int attackerMoraleHits, int defenderStrengthHits, int defenderMoraleHits, float probability, int winner)
    {

        StateChange st = new StateChange();
        st.attackerId = owner.GetUnitId();
        st.attackerMoraleChanged = defenderMoraleHits * -1;
        st.attackerStrengthChange = defenderStrengthHits * -1;
        st.defenderId = targetId;
        st.defenderMoraleChanged = attackerMoraleHits * -1;
        st.defenderStrengthChange = attackerStrengthHits * -1;
        st.changeProbability = probability;
        return st;

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

        stc = new List<StateChange>();
        if (attackDiceNumber == 2)
        {
            //                                                am  dm  as ds kf kfo
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.25f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1111111f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.611111111f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.027777777f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 3)
        {
            //                                                am  dm  as ds kf kfo
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.5f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.259259259f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.171296297f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.06944444f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 4)
        {
            //                                                am  dm  as ds kf kfo
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.41666666f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.2098765432f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0625f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.012345679f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.16666666f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.041666666f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0185185185f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.071759259f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5)
        {
            //                                                am  dm  as ds kf kfo
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.13888888f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.0617289351f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.1875f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.045267489f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.37037037f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.11574f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.06172839f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, new List<int>(activatesAttacks.ToArray()), new List<int>(deactivatesAttacks.ToArray()), 0.01877572f);
            SpecialOutcome(ref sc);
            stc.Add(sc);
        }
        return stc;
    }

    public override Attack GetCopy(Unit o)
    {
        CounterAttack nca;
        nca = new CounterAttack(attackId, isActiveState, armyId, o, keyFieldId, isKeyFieldTaken, targetId, arrowPosition, attackDiceNumber, defenceDiceNumber);
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
