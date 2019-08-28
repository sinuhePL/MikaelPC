using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private bool isInitialized = false;
    private int _squadNumber;
    private int _aliveSquadNumer;
    private int _armyId;
    private int _unitId;
    private GameObject[] _squads;
    private GameObject forwardArrow;
    private GameObject forwardArrowEmpty;
    private GameObject leftArrow;
    private GameObject leftArrowEmpty;
    private GameObject rightArrow;
    private GameObject rightArrowEmpty;
    [SerializeField] private GameObject arrowForwardBluePrefab;
    [SerializeField] private GameObject arrowForwardBlueEmptyPrefab;
    [SerializeField] private GameObject arrowForwardRedPrefab;
    [SerializeField] private GameObject arrowForwardRedEmptyPrefab;
    [SerializeField] private GameObject arrowLeftBluePrefab;
    [SerializeField] private GameObject arrowLeftBlueEmptyPrefab;
    [SerializeField] private GameObject arrowLeftRedPrefab;
    [SerializeField] private GameObject arrowLeftRedEmptyPrefab;
    [SerializeField] private GameObject arrowRightBluePrefab;
    [SerializeField] private GameObject arrowRightBlueEmptyPrefab;
    [SerializeField] private GameObject arrowRightRedPrefab;
    [SerializeField] private GameObject arrowRightRedEmptyPrefab;

    // called when arrow is clicked
    private void myAttackClicked(int idAttack)
    {
        forwardArrow.GetComponent<ArrowController>().ShowArrow(idAttack);
        leftArrow.GetComponent<ArrowController>().ShowArrow(idAttack);
        rightArrow.GetComponent<ArrowController>().ShowArrow(idAttack);
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked += myAttackClicked;
    }

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

    public void InitializeUnit(int squadNumber, int unitId, GameObject unitSquadPrefab, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId)   //   armyId == 1 then blue else red
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
            _armyId = armyId;
            _squads = new GameObject[_squadNumber];
            if (_armyId == 1)
            {
                forwardArrow = Instantiate(arrowForwardBluePrefab, transform.position + new Vector3(1.0f, 0.0f, 4.0f), arrowForwardBluePrefab.transform.rotation);
                forwardArrowEmpty = Instantiate(arrowForwardBlueEmptyPrefab, transform.position + new Vector3(1.0f, 0.0f, 4.0f), arrowForwardBlueEmptyPrefab.transform.rotation);
                leftArrow = Instantiate(arrowLeftBluePrefab, transform.position + new Vector3(-0.5f, 0.0f, 4.0f), arrowLeftBluePrefab.transform.rotation);
                leftArrowEmpty = Instantiate(arrowLeftBlueEmptyPrefab, transform.position + new Vector3(-0.5f, 0.0f, 4.0f), arrowLeftBlueEmptyPrefab.transform.rotation);
                rightArrow = Instantiate(arrowRightBluePrefab, transform.position + new Vector3(2.5f, 0.0f, 4.0f), arrowRightBluePrefab.transform.rotation);
                rightArrowEmpty = Instantiate(arrowRightBlueEmptyPrefab, transform.position + new Vector3(2.5f, 0.0f, 4.0f), arrowRightBlueEmptyPrefab.transform.rotation);
            }
            else
            {
                forwardArrow = Instantiate(arrowForwardRedPrefab, transform.position + new Vector3(1.0f, 0.0f, -4.0f), arrowForwardRedPrefab.transform.rotation);
                forwardArrowEmpty = Instantiate(arrowForwardRedEmptyPrefab, transform.position + new Vector3(1.0f, 0.0f, -4.0f), arrowForwardRedEmptyPrefab.transform.rotation);
                leftArrow = Instantiate(arrowLeftRedPrefab, transform.position + new Vector3(2.5f, 0.0f, -4.0f), arrowLeftRedPrefab.transform.rotation);
                leftArrowEmpty = Instantiate(arrowLeftRedEmptyPrefab, transform.position + new Vector3(2.5f, 0.0f, -4.0f), arrowLeftRedEmptyPrefab.transform.rotation);
                rightArrow = Instantiate(arrowRightRedPrefab, transform.position + new Vector3(-0.5f, 0.0f, -4.0f), arrowRightRedPrefab.transform.rotation);
                rightArrowEmpty = Instantiate(arrowRightRedEmptyPrefab, transform.position + new Vector3(-0.5f, 0.0f, -4.0f), arrowRightRedEmptyPrefab.transform.rotation);
            }
            forwardArrow.GetComponent<ArrowController>().AttackId = forwardAttackId;
            forwardArrowEmpty.GetComponent<ArrowController>().AttackId = forwardAttackId;
            leftArrow.GetComponent<ArrowController>().AttackId = leftAttackId;
            leftArrowEmpty.GetComponent<ArrowController>().AttackId = leftAttackId;
            rightArrow.GetComponent<ArrowController>().AttackId = rightAttackId;
            rightArrowEmpty.GetComponent<ArrowController>().AttackId = rightAttackId;
            for (int i = 0; i< _squadNumber; i++)
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
            EventManager.onAttackClicked += myAttackClicked;
        }
        else Debug.Log("Tried to initialized CavaleryController again! Id: " + _unitId);

        // test attack
        //forwardArrow.GetComponent<ArrowController>().isArrowActive = true;
        //forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
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

    // called when unit clicked
    public void Outline()
    {
        for (int i = 0; i < _aliveSquadNumer; i++)
        {
            _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
        }
        forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow();
        leftArrowEmpty.GetComponent<ArrowController>().ShowArrow();
        rightArrowEmpty.GetComponent<ArrowController>().ShowArrow();
    }

    // called when other unit clicked
    public void DisableOutline()
    {
        for (int i = 0; i < _aliveSquadNumer; i++)
        {
            _squads[i].GetComponentInChildren<PawnController>().DisableOutline();
        }
        forwardArrowEmpty.GetComponent<ArrowController>().HideArrow();
        leftArrowEmpty.GetComponent<ArrowController>().HideArrow();
        rightArrowEmpty.GetComponent<ArrowController>().HideArrow();
        forwardArrow.GetComponent<ArrowController>().HideArrow();
        leftArrow.GetComponent<ArrowController>().HideArrow();
        rightArrow.GetComponent<ArrowController>().HideArrow();
    }

    public void ActivateAttack(int attackId)
    {
        forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow(attackId);
        leftArrowEmpty.GetComponent<ArrowController>().ShowArrow(attackId);
        rightArrowEmpty.GetComponent<ArrowController>().ShowArrow(attackId);
        forwardArrow.GetComponent<ArrowController>().ShowArrow(attackId);
        leftArrow.GetComponent<ArrowController>().ShowArrow(attackId);
        rightArrow.GetComponent<ArrowController>().ShowArrow(attackId);
    }

    public void DeactivateAttack(int attackId)
    {
        forwardArrowEmpty.GetComponent<ArrowController>().HideArrow(attackId);
        leftArrowEmpty.GetComponent<ArrowController>().HideArrow(attackId);
        rightArrowEmpty.GetComponent<ArrowController>().HideArrow(attackId);
        forwardArrow.GetComponent<ArrowController>().HideArrow(attackId);
        leftArrow.GetComponent<ArrowController>().HideArrow(attackId);
        rightArrow.GetComponent<ArrowController>().HideArrow(attackId);
    }
}
