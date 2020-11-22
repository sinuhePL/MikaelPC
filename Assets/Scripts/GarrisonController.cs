using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GarrisonController : UnitController
{
    public override void InitializeUnit(int unitId, int armyId, int tileId, int deployPosition, string commander)
    {
        Vector3 tempPos;
        Color myColor;

        if (initialStrength > BattleManager.Instance.maxSquads)
        {
            Debug.Log("Tried to create too much squads in unit! Id: " + unitId);
            return;
        }
        isInitialized = true;
        isOutlined = false;
        isDisabled = false;
        _isBlocked = false;
        _isPlaced = false;
        _unitId = unitId * 10;
        _strength = initialStrength;
        _morale = initialMorale;
        _armyId = armyId;
        _squads = new GameObject[initialStrength];
        _unitCaption = GetComponentInChildren<TextMeshPro>();
        _unitCommander = commander;
        blockingUnitId = 0;
        isMoved = true;
        // set position based on id tile which it sits on
        ChangePosition(tileId);

        transform.position = transform.position + new Vector3(-1.0f, 0.0f, -0.5f);
        forwardArrow = Instantiate(arrowPrefab, transform.position + new Vector3(3.6f, 0.002f, 2.1f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -45.0f));
        forwardArrow.transform.localScale = new Vector3(1.4f, 4.0f, 1.0f);
        forwardArrow.GetComponent<ArrowController>().InitializeArrow("forward", "yellow", "solid");
        forwardArrow.transform.SetParent(transform);
        forwardArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(3.6f, 0.002f, 2.1f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -45.0f));
        forwardArrowEmpty.transform.localScale = new Vector3(1.4f, 4.0f, 1.0f);
        forwardArrowEmpty.GetComponent<ArrowController>().InitializeArrow("forward", "yellow", "empty");
        forwardArrowEmpty.transform.SetParent(transform);
        ColorUtility.TryParseHtmlString(BattleManager.Army1Color, out myColor);
        _unitCaption.color = myColor;
        flag = Instantiate(flagPrefab, transform.position + new Vector3(1.0f, 0.2f, -0.35f), flagPrefab.transform.rotation);
        flag.transform.SetParent(transform);
        forwardArrow.GetComponent<ArrowController>().ArrowId = _unitId + 1;
        forwardArrowEmpty.GetComponent<ArrowController>().ArrowId = _unitId + 1;
        forwardArrow.SetActive(false);
        forwardArrowEmpty.SetActive(false);
        _unitType = "Garrison";
        _unitCaption.text = _unitCommander;
        _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, -1.5f);
        // inicjalizacja squadów
        for (int i = 0; i < initialStrength; i++)
        {
            tempPos = transform.position;
            tempPos.x = tempPos.x + (i % 2) * 1.0f + 0.5f;
            if (_armyId == 1) tempPos.z = tempPos.z + 0.5f;
            else tempPos.z = tempPos.z - 0.5f;
            if (i > 1)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 1.0f;
                else tempPos.z = tempPos.z + 1.0f;
            }
            if (i > 3)
            {
                if (_armyId == 1) tempPos.z = tempPos.z - 1.0f;
                else tempPos.z = tempPos.z + 1.0f;
            }
            _squads[i] = Instantiate(unitSquadPrefab, tempPos, transform.rotation);
            _squads[i].GetComponentInChildren<PawnController>().UnitId = _unitId;
            _squads[i].transform.SetParent(transform);
        }
    }

    protected override void anyTileClicked(int idTile)
    {
        if (isOutlined)
        {
            for (int i = 0; i < _strength; i++)
            {
                _squads[i].GetComponentInChildren<PawnController>().DisableOutline();
            }
            isOutlined = false;
        }
        forwardArrowEmpty.GetComponent<ArrowController>().HideArrow();
        forwardArrow.GetComponent<ArrowController>().HideArrow();
        forwardArrow.SetActive(false);
        forwardArrowEmpty.SetActive(false);
    }

    public override void ActivateAttack(int arrowId)
    {
        if (forwardArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            forwardArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
    }

    public override void DeactivateAttack(int arrowId)
    {
        if (forwardArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            forwardArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
    }

    public override int GetArrowId(string direction)
    {
        if(direction == "central") return forwardArrowEmpty.GetComponent<ArrowController>().ArrowId;
        else return -1;
    }

    public override ArrowController GetArrowController(string direction)
    {
        if(direction == "central") return forwardArrowEmpty.GetComponent<ArrowController>();
        else return null;
    }

    protected override void UpdateMe(string mode)
    {
        Unit myUnit;
        List<Attack> tempAttacks;
        bool isActive;

        if (mode == "routtest") return;
        myUnit = BattleManager.Instance.GetUnit(UnitId);
        anyTileClicked(0);
        if (!myUnit.IsAlive && !isDisabled)    // check if unit killed
        {
            isDisabled = true;
            EventManager.RaiseEventOnUnitDestroyed(UnitId);
            StartCoroutine(DisableUnit());
            return;
        }
        if (_strength != myUnit.strength)  // check if unit lost strength
        {
            if (_strength > myUnit.strength) KillSquads(_strength - myUnit.strength);
            _strength = myUnit.strength;
        }
        if (_morale != myUnit.morale)    // check if unit lost morale
        {
            flag.GetComponent<FlagController>().ChangeBannerHeight(initialMorale, myUnit.morale);
            _morale = myUnit.morale;
        }
        //check if unit moved to front line
        if (myUnit.UnitMoved() && !isMoved)
        {
            isMoved = true;
            SetArrowsBlockValue(false);
            if (_armyId == 1)
            {
                transform.DOMoveZ(transform.position.z + BattleManager.Instance.boardFieldHeight, 1.0f).SetEase(Ease.InQuad);
                _unitTileId += -1;
            }
            if (_armyId == 2)
            {
                transform.DOMoveZ(transform.position.z - BattleManager.Instance.boardFieldHeight, 1.0f).SetEase(Ease.InQuad);
                _unitTileId += 1;
            }
        }
        if (!myUnit.UnitMoved() && isMoved) isMoved = false;
        //check if unit attacks are still active
        tempAttacks = null;
        tempAttacks = myUnit.GetAttacksByArrowId(forwardArrow.GetComponent<ArrowController>().ArrowId);
        if (tempAttacks.Count > 0)
        {
            isActive = false;
            foreach (Attack a in tempAttacks)
            {
                if (a.IsActive()) isActive = true;
            }
            if (isActive) ActivateAttack(forwardArrow.GetComponent<ArrowController>().ArrowId);
            else DeactivateAttack(forwardArrow.GetComponent<ArrowController>().ArrowId);
        }
    }

    protected override void myUnitClicked(int idUnit)
    {
        if (!BattleManager.Instance.isInputBlocked && BattleManager.Instance.gameMode != "deploy")
        {
            if (idUnit == UnitId && !isOutlined)
            {
                forwardArrow.SetActive(true);
                forwardArrowEmpty.SetActive(true);
                for (int i = 0; i < _strength; i++)
                {
                    _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
                }
                forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                isOutlined = true;
            }
            else
            {
                if (isOutlined)
                {
                    for (int i = 0; i < _strength; i++)
                    {
                        _squads[i].GetComponentInChildren<PawnController>().DisableOutline();
                    }
                    isOutlined = false;
                }
                forwardArrowEmpty.GetComponent<ArrowController>().HideArrow();
                forwardArrow.GetComponent<ArrowController>().HideArrow();
                forwardArrow.SetActive(false);
                forwardArrowEmpty.SetActive(false);
            }
        }
    }

    protected override void anyAttackClicked(int idAttack, bool isCounterAttack)
    {
        forwardArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
    }
}

