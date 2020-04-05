using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChange
{
    public int attackerId;
    public int defenderId;
    public int attackerMoraleChanged;
    public int defenderMoraleChanged;
    public int attackerStrengthChange;
    public int defenderStrengthChange;
    public int keyFieldChangeId;
    public int keyFieldNewOccupantId;
    public List<int> activatedAttacks;
    public List<int> deactivatedAttacks;
    public float changeProbability;
    public string specialOutcomeDescription;

    public StateChange()
    {
        attackerId = 0;
        defenderId = 0;
        attackerMoraleChanged = 0;
        defenderMoraleChanged = 0;
        attackerStrengthChange = 0;
        defenderStrengthChange = 0;
        keyFieldChangeId = 0;
        keyFieldNewOccupantId = 0;
        activatedAttacks = new List<int>();
        deactivatedAttacks = new List<int>();
        changeProbability = 0.0f;
        specialOutcomeDescription = "";
    }

    public StateChange(int ai, int di, int amc, int dmc, int asc, int dsc, int kfci, int kfnoi, List<int> aa, List<int> da, float cp)
    {
        attackerId = ai;
        defenderId = di;
        attackerMoraleChanged = amc;
        defenderMoraleChanged = dmc;
        attackerStrengthChange = asc;
        defenderStrengthChange = dsc;
        keyFieldChangeId = kfci;
        keyFieldNewOccupantId = kfnoi;
        activatedAttacks = aa;
        deactivatedAttacks = da;
        changeProbability = cp;
        specialOutcomeDescription = "";
    }
}
