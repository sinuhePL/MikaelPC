using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwissController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, int tileId, int deployPosition)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, forwardAttackId, leftAttackId, rightAttackId, tileId, deployPosition);
        _unitType = "Suisse";
        _unitCaption.text = "de La Marck";
        if (_armyId == 1)
        {
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, -2.0f);
        }
        else
        {
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, 2.0f);
            _unitCaption.transform.rotation = _unitCaption.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
        }
        // inicjalizacja squadów
        tempPos = transform.position;
        tempPos.x = tempPos.x - 0.7f;
        if (_armyId == 1) tempPos.z = tempPos.z + 0.5f;
        else tempPos.z = tempPos.z - 0.5f;
        for (int i = 0; i < initialStrength; i++)
        {
            tempPos.x = tempPos.x + 0.8f;
            if (_armyId == 1) tempPos.z = tempPos.z - 0.5f;
            else tempPos.z = tempPos.z + 0.5f;


            _squads[i] = Instantiate(unitSquadPrefab, tempPos, transform.rotation);
            _squads[i].GetComponentInChildren<PawnController>().UnitId = _unitId;
            _squads[i].transform.SetParent(transform);
        }
        PlaceWidget(deployPosition);
    }
}
