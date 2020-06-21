using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArquebusiersController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, int tileId, int deployPosition)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, forwardAttackId, leftAttackId, rightAttackId, tileId, deployPosition);
        _unitType = "Arquebusiers";
        if (_armyId == 1)
        {
            _unitCaption.text = "de la Pole";
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, -1.5f);
        }
        else
        {
            _unitCaption.text = "de Vasto";
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, 2.0f);
            _unitCaption.transform.rotation = _unitCaption.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }
        // inicjalizacja squadów
        for (int i = 0; i < initialStrength; i++)
        {
            tempPos = transform.position;
            tempPos.x = tempPos.x + (i % 4) * 0.75f;
            if (i > 3)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 0.5f;
                else tempPos.z = tempPos.z + 0.5f;
            }
            if (i > 7)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 0.5f;
                else tempPos.z = tempPos.z + 0.5f;
            }
            _squads[i] = Instantiate(unitSquadPrefab, tempPos, transform.rotation);
            _squads[i].GetComponentInChildren<PawnController>().UnitId = _unitId;
            _squads[i].transform.SetParent(transform);
        }
        PlaceWidget(deployPosition);
    }
}
