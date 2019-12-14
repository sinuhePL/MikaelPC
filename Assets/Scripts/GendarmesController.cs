using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GendarmesController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, int tileId)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, forwardAttackId, leftAttackId, rightAttackId, tileId);
        _unitType = "Gendarmes";
        _unitCaption.text = "Francis I";
        // inicjalizacja squadów
        for (int i = 0; i < initialStrength; i++)
        {
            tempPos = transform.position;
            tempPos.x = tempPos.x + i % 3;
            if (i > 2)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 0.5f;
                else tempPos.z = tempPos.z + 0.5f;
            }
            _squads[i] = Instantiate(unitSquadPrefab, tempPos, transform.rotation);
            _squads[i].GetComponentInChildren<PawnController>().UnitId = _unitId;
        }
    }
}
