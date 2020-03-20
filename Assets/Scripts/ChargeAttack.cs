using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{
    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, string aType, string dType, bool f) : base(aId, state, army, o, keyField, isKFTaken, tId, p, f)
    {
        attackName = "Charge!";
        switch(aType)
        {
            case "Gendarmes":
                switch(dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        break;
                }
                break;
            case "Landsknechte":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                    case "Suisse":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                }
                break;
            case "Suisse":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                    case "Suisse":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        break;
                }
                break;
            case "Imperial Cavalery":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 3;
                        break;
                }
                break;
        }
    }

    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, int aNum, int dNum, bool f) : base(aId, state, army, o, keyField, isKFTaken, tId, p, f)
    {
        attackName = "Charge!";
        attackDiceNumber = aNum;
        defenceDiceNumber = dNum;
    }

    public override StateChange ApplyAttack(int attackerStrengthHits, int attackerMoraleHits, int defenderStrengthHits, int defenderMoraleHits, float probability, int winner)
    {

        StateChange st = new StateChange();
        st.attackerId = owner.GetUnitId();
        st.attackerMoraleChanged = defenderMoraleHits * -1;
        st.attackerStrengthChange = defenderStrengthHits * -1;
        st.changeProbability = probability;
        st.defenderId = targetId;
        st.defenderMoraleChanged = attackerMoraleHits * -1;
        st.defenderStrengthChange = attackerStrengthHits * -1;
        st.changeProbability = probability;
        return st;

    }

    public override void SpecialAttack(ref StateChange sc)
    {
        if(keyFieldId != 0 && !isKeyFieldTaken)
        {
            sc.keyFieldChangeId = keyFieldId;
            sc.keyFieldNewOccupantId = owner.GetArmyId();
        }
    }

    public override List<StateChange> GetOutcomes()
    {
        List<StateChange> stc;
        StateChange sc;

        stc = new List<StateChange>();
        if (attackDiceNumber == 2 && defenceDiceNumber == 2)
        {
            //                                                am  dm  as ds kf kfo
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.0625f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.027778f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.159722f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.027778f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.012346f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.070988f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.152778f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.067901f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.390432f);
            stc.Add(sc);
            // adding result for special attack
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.006944f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.003086f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.017747f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 2)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.055556f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.319444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.064815f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.028807f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.165638f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.042824f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.019033f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.109439f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.017361f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.007716f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.044367f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 2 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.064815f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.060185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.055556f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.028807f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.026749f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.305556f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.158436f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.147119f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.013889f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.007202f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.006687f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.25f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.12963f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.12037f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.12963f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.067215f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.062414f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.085648f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.04441f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.041238f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.034722f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.018004f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.016718f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 2)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.1041667f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.046296f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.266204f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.052469f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.02332f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.134088f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.015625f);
            stc.Add(sc);
            /* sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.006944f);
             stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.039931f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.041667f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.018519f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.106481f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.01794f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.007973f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.045846f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.010417f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.00463f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.02662f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.003086f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.001372f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.007888f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.00463f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.002058f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.011831f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.2083333f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.108025f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.100309f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.104938f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.054412f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.050526f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.03588f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.018604f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.017275f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.03125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.016204f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.015046f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.083333f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.04321f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.040123f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.020833f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.010802f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.010031f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.006173f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.003201f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.002972f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.009259f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.004801f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.004458f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.1909722f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.095165f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.0299f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.096193f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.047935f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.015061f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.03289f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.016389f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.005149f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.026042f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.069444f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.00514403f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.013117f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.034979f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.002591f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.004485f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.01196f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.000886f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.028646f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.014275f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.004485f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, null, null, 0.003906f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, null, null, 0.010417f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, null, null, 0.000772f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.076389f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.038066f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.01196f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, null, null, 0.010417f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, null, null, 0.027778f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, null, null, 0.002058f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.019097f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.009516f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.00299f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.002604f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.006944f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.000514f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.005658f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.00282f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.000886f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, null, null, 0.000772f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, null, null, 0.002058f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, null, null, 0.000152f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.008488f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.00423f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.001329f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.001157f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.003086f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.000229f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 2)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.0347222f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.015432f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.088735f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.015432f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.006859f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.039438f);
            stc.Add(sc);
            // adding result for special attack
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.004694f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.002086f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.011996f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.046875f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.020833f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.119792f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.092593f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.041152f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.236626f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.028935f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.01286f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.073945f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.011317f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.00503f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.028921f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.015432f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.006859f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.039438f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 3)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.0694444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.036008f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.033436f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.030864f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.016004f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.014861f);
            stc.Add(sc);
            // adding result for special attack
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.009388f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.004868f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.00452f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.09375f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.048611f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.045139f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.185185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.096022f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.089163f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.05787f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.030007f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.027864f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.022634f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.011736f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.010898f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.030864f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.016004f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.014861f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.0636574f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.031722f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.009967f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.028292f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.014098f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.00443f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.008606f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.004288f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.001347f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.008681f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.023148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.00171468f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.003858f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.010288f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.00076f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.001173f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.003129f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.00023f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.085937f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.042824f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.013455f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, null, null, 0.011719f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, null, null, 0.03125f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, null, null, 0.002315f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.169753f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.084591f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.026578f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, null, null, 0.023148f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, null, null, 0.061728f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, null, null, 0.004572f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.053048f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.026435f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.008305f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.007234f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.01929f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.001429f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.020748f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.010339f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.003248f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, null, null, 0.002829f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, null, null, 0.007545f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, null, null, 0.00056f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.028292f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.014098f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.00443f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.003858f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.010288f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.00076f);
            SpecialAttack(ref sc);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 5 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.0353652f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.017147f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.002608f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.015718f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.007621f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.001159f);
            stc.Add(sc);
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.004781f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.002318f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.00035f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.026042f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.05144f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.00628715f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.011574f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.022862f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.002794f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.00352f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.006954f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.00085f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.047743f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.023148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.00352f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, null, null, 0.035156f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, null, null, 0.069444f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, null, null, 0.008488f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.094307f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.045725f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.006954f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, null, null, 0.069444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, null, null, 0.137174f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, null, null, 0.016766f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.029471f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.014289f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.002173f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.021701f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.042867f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.005239f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.011526f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.005589f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.00085f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, null, null, 0.008488f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, null, null, 0.016766f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, null, null, 0.002049f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.015718f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.007621f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.001159f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.011574f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.022862f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.002794f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 2 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.1145833f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.057099f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.01794f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.050926f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.025377f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.007973f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.012731f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.006344f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.001993f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.280093f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.139575f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.043853f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.015625f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.041667f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.00308642f);
            stc.Add(sc);*/
            /*sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.006944f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.018519f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.001372f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.001736f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.00463f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.00034f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.038194f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.101852f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.007545f);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 2 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.0636574f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.030864f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.004694f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.028292f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.013717f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.002086f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.007073f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.003429f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.000522f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.155607f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.075446f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.011474f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.046875f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.092593f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.01131687f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.020833f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.041152f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.00503f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.005208f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.010288f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.001257f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.114583f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.226337f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.027663f);
            stc.Add(sc);
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 4)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.2291667f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.114198f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.03588f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.118827f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.059214f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.018604f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.031829f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.015861f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.004983f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.078511f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.039123f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.012292f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.03125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.083333f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.00617284f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.016204f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.04321f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.003201f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.00434f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.011574f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.00086f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.010706f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.028549f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.002115f);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 3 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.1273148f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.061728f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.009388f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.066015f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.032007f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.004868f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.017683f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.008573f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.001304f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.043617f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.021148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.003216f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.09375f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.185185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.02263374f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.048611f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.096022f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.011736f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.013021f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.02572f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.003144f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.032118f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.063443f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.007754f);
            stc.Add(sc);*/
        }
        else if (attackDiceNumber == 4 && defenceDiceNumber == 5)
        {
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.1060957f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.05144f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.007823f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.053441f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.025911f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.00394f);
            stc.Add(sc);*/
            // adding result for special attack
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.018272f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.008859f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.001347f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.078125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.154321f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.01886145f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.039352f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.077732f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.009501f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, 0, 0, 0, null, null, 0.013455f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, 0, 0, 0, null, null, 0.026578f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, 0, 0, 0, null, null, 0.003248f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, 0, 0, 0, 0, null, null, 0.015914f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -1, 0, 0, 0, null, null, 0.007716f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, 0, 0, 0, 0, null, null, 0.001173f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -2, 0, 0, 0, 0, null, null, 0.011719f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -2, -1, 0, 0, 0, null, null, 0.023148f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -2, -2, 0, 0, 0, null, null, 0.002829f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, -1, 0, 0, null, null, 0.042438f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, -1, 0, 0, null, null, 0.020576f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, -1, 0, 0, null, null, 0.003129f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, -1, 0, 0, null, null, 0.03125f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, -1, 0, 0, null, null, 0.061728f);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, -1, 0, 0, null, null, 0.007545f);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, 0, 0, 0, 0, null, null, 0.01061f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -1, 0, 0, 0, null, null, 0.005144f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, 0, 0, 0, 0, null, null, 0.00078f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, -1, 0, 0, 0, 0, null, null, 0.007813f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            sc = new StateChange(owner.GetUnitId(), targetId, -1, -1, -1, 0, 0, 0, null, null, 0.015432f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            /*sc = new StateChange(owner.GetUnitId(), targetId, 0, -1, -2, 0, 0, 0, null, null, 0.001886f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
            /*sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -2, 0, 0, null, null, 0.003144f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -2, 0, 0, null, null, 0.001524f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -2, 0, 0, null, null, 0.00023f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -2, 0, 0, null, null, 0.002315f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -2, 0, 0, null, null, 0.004572f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -2, 0, 0, null, null, 0.00056f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, -1, 0, 0, null, null, 0.004715f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, -1, 0, 0, null, null, 0.002286f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, -1, 0, 0, null, null, 0.00035f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -2, 0, 0, -1, 0, 0, null, null, 0.003472f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, -1, -1, 0, 0, null, null, 0.006859f);
            SpecialAttack(ref sc);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -2, -1, 0, 0, null, null, 0.00084f);
            SpecialAttack(ref sc);
            stc.Add(sc);*/
        }
        return stc;
    }

    public override Attack GetCopy(Unit o)
    {
        ChargeAttack nca;
        nca = new ChargeAttack(attackId, isActiveState, armyId, o, keyFieldId, isKeyFieldTaken, targetId, arrowPosition, attackDiceNumber, defenceDiceNumber, isAttackForward);
        return (Attack)nca;
    }
}
