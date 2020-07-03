using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : Attack
{
    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, string aType, string dType) : base(aId, state, army, o, keyField, isKFTaken, tId, p)
    {
        attackName = "Charge!";
        switch(aType)
        {
            case "Gendarmes":
                switch(dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 1;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 2;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 3;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 4;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 2;
                        break;
                    case "Artillery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                }
                break;
            case "Landsknechte":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 5;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 7;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 7;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 1;
                        break;
                    case "Artillery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 5;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                }
                break;
            case "Suisse":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 3;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 8;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 9;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 2;
                        break;
                    case "Artillery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 2;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                }
                break;
            case "Imperial Cavalery":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 7;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 1;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 5;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 2;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 1;
                        break;
                    case "Artillery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 10;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                }
                break;
            case "Arquebusiers":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 4;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 2;
                        break;
                    case "Suisse":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 8;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 2;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 2;
                        break;
                    case "Artillery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 1;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 6;
                        break;
                }
                break;
            case "Artillery":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 0;
                        specialOutcomeType = 10;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 0;
                        specialOutcomeType = 6;
                        break;
                    case "Suisse":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 0;
                        specialOutcomeType = 10;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 0;
                        specialOutcomeType = 5;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 0;
                        specialOutcomeType = 8;
                        break;
                    case "Artillery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 0;
                        specialOutcomeType = 7;
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
                break;
            case "Stradioti":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 10;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 6;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 10;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 5;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 8;
                        break;
                    case "Artillery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 7;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                }
                break;
            case "Coustilliers":
                switch (dType)
                {
                    case "Gendarmes":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 10;
                        break;
                    case "Landsknechte":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 6;
                        break;
                    case "Suisse":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 10;
                        break;
                    case "Imperial Cavalery":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 3;
                        specialOutcomeType = 5;
                        break;
                    case "Arquebusiers":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 8;
                        break;
                    case "Artillery":
                        attackDiceNumber = 3;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 7;
                        break;
                    case "Stradioti":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                    case "Coustilliers":
                        attackDiceNumber = 2;
                        defenceDiceNumber = 2;
                        specialOutcomeType = 6;
                        break;
                }
                break;
        }
    }

    public ChargeAttack(int aId, bool state, int army, Unit o, int keyField, bool isKFTaken, int tId, Vector3 p, int aNum, int dNum) : base(aId, state, army, o, keyField, isKFTaken, tId, p)
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
        st.defenderId = targetId;
        st.defenderMoraleChanged = attackerMoraleHits * -1;
        st.defenderStrengthChange = attackerStrengthHits * -1;
        st.changeProbability = probability;
        return st;

    }

    public override void SpecialOutcome(ref StateChange sc)
    {
        if(keyFieldId != 0 && !isKeyFieldTaken)
        {
            sc.keyFieldChangeId = keyFieldId;
            sc.keyFieldNewOccupantId = owner.GetArmyId();
        }
        else
        {
            if(specialOutcomeType == 1)
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
            if(specialOutcomeType == 10)
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
            if(specialOutcomeType == 1) return "Defender loses hits";
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

    public override Attack GetCopy(Unit o)
    {
        ChargeAttack nca;
        nca = new ChargeAttack(attackId, isActiveState, armyId, o, keyFieldId, isKeyFieldTaken, targetId, arrowPosition, attackDiceNumber, defenceDiceNumber);
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
