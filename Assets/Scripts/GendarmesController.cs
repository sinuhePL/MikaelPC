using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GendarmesController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int tileId, int deployPosition, string commander)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, tileId, deployPosition, commander);
        _unitType = "Gendarmes";
        _unitCaption.text = _unitCommander;
        if(_armyId == 1)
        {
            _unitCaption.transform.position = transform.position + new Vector3(1.2f, 0.0f, -1.3f);
        }
        else
        {
            _unitCaption.transform.position = transform.position + new Vector3(1.2f, 0.0f, 1.2f);
            //_unitCaption.transform.rotation = _unitCaption.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }
        // inicjalizacja squadów
        for (int i = 0; i < initialStrength; i++)
        {
            tempPos = transform.position;
            tempPos.x = tempPos.x + i % 3;
            if (_armyId == 1) tempPos.z = tempPos.z + 0.25f;
            else tempPos.z = tempPos.z - 0.25f;
            if (i > 2)
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
