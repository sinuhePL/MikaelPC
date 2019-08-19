using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private bool isInitialized = false;
    private int _squadNumber;
    private int _aliveSquadNumer;
    private int _unitId;
    private GameObject[] _squads;

    public int UnitId
    {
        get
        {
            return _unitId;
        }
        set
        {
            _unitId = value;
        }
    }

    public void InitializeUnit(int squadNumber, int unitId, GameObject unitSquadPrefab)
    {
        Vector3 tempPos;

        if (!isInitialized)
        {
            if (squadNumber > BattleManager.maxSquads)
            {
                Debug.Log("Tried to create too much squads in unit! Id: " + unitId);
                return;
            }
            isInitialized = true;
            _unitId = unitId;
            _squadNumber = squadNumber;
            _aliveSquadNumer = squadNumber;
            _squads = new GameObject[_squadNumber];
            for (int i = 0; i< _squadNumber; i++)
            {
                tempPos = transform.position;
                tempPos.x = tempPos.x + i % 3;
                if (i > 2) tempPos.z = tempPos.z - 0.5f;
                _squads[i] = Instantiate(unitSquadPrefab, tempPos, transform.rotation);
                _squads[i].GetComponentInChildren<PawnController>().UnitId = _unitId;
            }
        }
        else Debug.Log("Tried to initialized CavaleryController again! Id: " + _unitId);
    }

    public void KillSquads(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_aliveSquadNumer - 1 >= 0)
            {
                _squads[_aliveSquadNumer - 1].GetComponentInChildren<PawnController>().Disable();
                _aliveSquadNumer--;
            }
        }
    }

    public void Outline()
    {
        for (int i = 0; i < _aliveSquadNumer; i++)
        {
            _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
        }
    }

    public void DisableOutline()
    {
        for (int i = 0; i < _aliveSquadNumer; i++)
        {
            _squads[i].GetComponentInChildren<PawnController>().DisableOutline();
        }
    }
}
