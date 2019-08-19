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
    public int winnerId;
    public List<int> activatedAttacks;
    public List<int> deactivatedAttacks;
    public int attackerRouteTestModifierChange;
    public int defenderRouteTestModifierChange;
    public float changeProbability;

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
        winnerId = 0;
        activatedAttacks = null;
        deactivatedAttacks = null;
        changeProbability = 0.0f;
        attackerRouteTestModifierChange = 0;
        defenderRouteTestModifierChange = 0;
    }

    public StateChange(int ai, int di, int amc, int dmc, int asc, int dsc, int kfci, int kfnoi, int wi, List<int> aa, List<int> da, float cp, int artmc, int drtmc)
    {
        attackerId = ai;
        defenderId = di;
        attackerMoraleChanged = amc;
        defenderMoraleChanged = dmc;
        attackerStrengthChange = asc;
        defenderStrengthChange = dsc;
        keyFieldChangeId = kfci;
        keyFieldNewOccupantId = kfnoi;
        winnerId = wi;
        activatedAttacks = aa;
        deactivatedAttacks = da;
        changeProbability = cp;
        attackerRouteTestModifierChange = artmc;
        defenderRouteTestModifierChange = drtmc;
    }
}
