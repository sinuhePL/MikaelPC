using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StradiotiController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, int tileId, int deployPosition)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, forwardAttackId, leftAttackId, rightAttackId, tileId, deployPosition);
        if (_armyId == 1)
        {
            _unitType = "Coustilliers";
            _unitCaption.text = "Tiercelin";
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, -1.5f);
        }
        else
        {
            _unitType = "Stradioti";
            _unitCaption.text = "";
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, 2.0f);
            _unitCaption.transform.rotation = _unitCaption.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
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
            _squads[i].transform.rotation = _squads[i].transform.rotation * Quaternion.Euler(0.0f, Random.Range(-3.0f, 3.0f), 0.0f);
        }
        PlaceWidget(deployPosition);
    }
}
