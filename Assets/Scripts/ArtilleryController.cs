using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int tileId, int deployPosition, string commander)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, tileId, deployPosition, commander);
        _unitType = "Artillery";
        _unitCaption.text = _unitCommander;
        if (_armyId == 1)
        {
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, -1.5f);
        }
        else
        {
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, 1.5f);
            //_unitCaption.transform.rotation = _unitCaption.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
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
            if(armyId == 2) _squads[i].transform.rotation = _squads[i].transform.rotation * Quaternion.Euler(0.0f, -90.0f, 0.0f);
            else if(armyId == 1) _squads[i].transform.rotation = _squads[i].transform.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);
        }
        PlaceWidget(deployPosition);
    }
}
