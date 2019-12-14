using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpLandsknechteController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, int tileId)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, forwardAttackId, leftAttackId, rightAttackId, tileId);
        _unitType = "Landsknechte";
        _unitCaption.text = "von Frundsberg";
        // inicjalizacja squadów
        for (int i = 0; i < initialStrength; i++)
        {
            tempPos = transform.position;
            tempPos.x = tempPos.x + (i % 4)*0.75f;
            if (i > 3)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 0.5f;
                else tempPos.z = tempPos.z + 0.5f;
            }
            if(i > 7)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 0.5f;
                else tempPos.z = tempPos.z + 0.5f;
            }
            _squads[i] = Instantiate(unitSquadPrefab, tempPos, transform.rotation);
            _squads[i].GetComponentInChildren<PawnController>().UnitId = _unitId;
        }
    }
}
