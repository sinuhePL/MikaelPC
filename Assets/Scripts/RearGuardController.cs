using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RearGuardController : UnitController
{
    protected GameObject sideArrow;
    protected GameObject sideArrowEmpty;
    protected bool hasArrived;

    public override void InitializeUnit(int unitId, int armyId, int tileId, int deployPosition, string commander)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, tileId, deployPosition, commander);
        _unitType = "Arquebusiers";
        _unitCaption.text = _unitCommander;
        _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, 1.5f);
        hasArrived = false;

        rightArrow.transform.rotation = arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 135.0f);
        rightArrow.transform.position += new Vector3(-0.4f, 0.0f, 1.8f);
        rightArrow.transform.localScale = new Vector3(1.6f, 4.5f, 1.0f);
        rightArrowEmpty.transform.rotation = arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 135.0f);
        rightArrowEmpty.transform.position += new Vector3(-0.4f, 0.0f, 1.8f);
        rightArrowEmpty.transform.localScale = new Vector3(1.6f, 4.5f, 1.0f);
        sideArrow = Instantiate(arrowPrefab, transform.position + new Vector3(3.6f, 0.002f, 0.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -90.0f));
        sideArrow.transform.localScale = new Vector3(1.2f, 2.0f, 1.0f);
        sideArrow.GetComponent<ArrowController>().InitializeArrow("forward", "blue", "solid");
        sideArrow.transform.SetParent(transform);
        sideArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(3.6f, 0.002f, 0.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -90.0f));
        sideArrowEmpty.transform.localScale = new Vector3(1.2f, 2.0f, 1.0f);
        sideArrowEmpty.GetComponent<ArrowController>().InitializeArrow("forward", "blue", "empty");
        sideArrowEmpty.transform.SetParent(transform);
        sideArrow.GetComponent<ArrowController>().ArrowId = _unitId + 4;
        sideArrowEmpty.GetComponent<ArrowController>().ArrowId = _unitId + 4;
        sideArrow.SetActive(false);
        sideArrowEmpty.SetActive(false);
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
    }

    public override int GetArrowId(string direction)
    {
        switch (direction)
        {
            case "right":
                return rightArrowEmpty.GetComponent<ArrowController>().ArrowId;
            case "central":
                return forwardArrowEmpty.GetComponent<ArrowController>().ArrowId;
            case "left":
                return leftArrowEmpty.GetComponent<ArrowController>().ArrowId;
            case "side":
                return sideArrowEmpty.GetComponent<ArrowController>().ArrowId;
        }
        return -1;
    }

    public override ArrowController GetArrowController(string direction)
    {
        switch (direction)
        {
            case "right":
                return rightArrowEmpty.GetComponent<ArrowController>();
            case "central":
                return forwardArrowEmpty.GetComponent<ArrowController>();
            case "left":
                return leftArrowEmpty.GetComponent<ArrowController>();
            case "side":
                return sideArrowEmpty.GetComponent<ArrowController>();
        }
        return null;
    }

    protected void MoveUnit()
    {
        transform.DOMoveX(transform.position.x + BattleManager.Instance.boardFieldWitdth, 1.0f).SetEase(Ease.InQuad);
        _unitTileId = 11;
        hasArrived = true;
    }

    protected override void UpdateMe(string mode)
    {
        base.UpdateMe(mode);
        Unit myUnit;
        Attack myAttack;
        myUnit = BattleManager.Instance.GetUnit(UnitId);
        myAttack = myUnit.GetAttacksByArrowId(144)[0];
        if (!hasArrived && myUnit.UnitArrived())
        {
            MoveUnit();
            myAttack.Deactivate();
        }
        if(myAttack.IsActive()) ActivateAttack(sideArrow.GetComponent<ArrowController>().ArrowId);
        else DeactivateAttack(sideArrow.GetComponent<ArrowController>().ArrowId);
    }

    protected override void anyAttackClicked(int idAttack, bool isCounterAttack)
    {
        base.anyAttackClicked(idAttack, isCounterAttack);
        sideArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
    }

    protected override void anyTileClicked(int idTile)
    {
        base.anyTileClicked(idTile);
        sideArrowEmpty.GetComponent<ArrowController>().HideArrow();
        sideArrow.GetComponent<ArrowController>().HideArrow();
        sideArrow.SetActive(false);
        sideArrowEmpty.SetActive(false);
    }

    protected override void myUnitClicked(int idUnit)
    {
        if (!BattleManager.Instance.isInputBlocked && BattleManager.Instance.gameMode != "deploy")
        {
            if (idUnit == UnitId && !isOutlined)
            {
                forwardArrow.SetActive(true);
                forwardArrowEmpty.SetActive(true);
                leftArrow.SetActive(true);
                leftArrowEmpty.SetActive(true);
                rightArrow.SetActive(true);
                rightArrowEmpty.SetActive(true);
                sideArrow.SetActive(true);
                sideArrowEmpty.SetActive(true);
                for (int i = 0; i < _strength; i++)
                {
                    _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
                }
                forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                leftArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                rightArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                sideArrowEmpty.GetComponent<ArrowController>().ShowArrow();
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
                leftArrowEmpty.GetComponent<ArrowController>().HideArrow();
                rightArrowEmpty.GetComponent<ArrowController>().HideArrow();
                sideArrowEmpty.GetComponent<ArrowController>().HideArrow();
                forwardArrow.GetComponent<ArrowController>().HideArrow();
                leftArrow.GetComponent<ArrowController>().HideArrow();
                rightArrow.GetComponent<ArrowController>().HideArrow();
                sideArrow.GetComponent<ArrowController>().HideArrow();
                forwardArrow.SetActive(false);
                forwardArrowEmpty.SetActive(false);
                leftArrow.SetActive(false);
                leftArrowEmpty.SetActive(false);
                rightArrow.SetActive(false);
                rightArrowEmpty.SetActive(false);
                sideArrow.SetActive(false);
                sideArrowEmpty.SetActive(false);
            }
        }
    }

    public override void ActivateAttack(int arrowId)
    {
        base.ActivateAttack(arrowId);
        if (sideArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            sideArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            sideArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
    }

    public override void DeactivateAttack(int arrowId)
    {
        base.DeactivateAttack(arrowId);
        if (sideArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            sideArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            sideArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
    }
}

