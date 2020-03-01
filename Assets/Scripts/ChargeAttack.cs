using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{
    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, int tId, Vector3 p, string aType, string dType, bool f) : base(aId, state, army, o, keyField, tId, p, f)
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

    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, int tId, Vector3 p, int aNum, int dNum, bool f) : base(aId, state, army, o, keyField, tId, p, f)
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

    public override List<StateChange> GetOutcomes()
    {
        List<StateChange> stc;
        StateChange sc;

        stc = new List<StateChange>();
        if(attackDiceNumber == 2 && defenceDiceNumber == 2)
        {
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
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.159722f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.070988f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.408179f);
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
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.060185f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.026749f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.153807f);
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
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.319444f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.165638f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.153807f);
            stc.Add(sc);
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
            sc = new StateChange(owner.GetUnitId(), targetId, -1, 0, 0, 0, 0, 0, null, null, 0.12037f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, -1, 0, 0, 0, null, null, 0.062414f);
            stc.Add(sc);
            sc = new StateChange(owner.GetUnitId(), targetId, 0, 0, 0, 0, 0, 0, null, null, 0.057956f);
            stc.Add(sc);
        }
        return stc;
    }

    public override Attack GetCopy(Unit o)
    {
        ChargeAttack nca;
        nca = new ChargeAttack(attackId, isActiveState, armyId, o, keyFieldId, targetId, arrowPosition, attackDiceNumber, defenceDiceNumber, isAttackForward);
        return (Attack)nca;
    }
}
