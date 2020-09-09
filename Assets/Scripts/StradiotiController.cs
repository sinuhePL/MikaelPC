using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StradiotiController : UnitController
{
    protected GameObject farArrow;
    protected GameObject farArrowEmpty;

    public override void InitializeUnit(int unitId, int armyId, int tileId, int deployPosition, string commander)
    {
        Vector3 tempPos;

        base.InitializeUnit(unitId, armyId, tileId, deployPosition, commander);
        _unitCaption.text = _unitCommander;
        if (_armyId == 1)
        {
            _unitType = "Coustilliers";
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, -1.5f);
            farArrow = Instantiate(arrowPrefab, transform.position + new Vector3(2.5f, 0.002f, 6.0f), arrowPrefab.transform.rotation);
            farArrow.GetComponent<ArrowController>().InitializeArrow("far", "blue", "solid");
            farArrow.transform.localScale = new Vector3(farArrow.transform.localScale.x, 10.5f, farArrow.transform.localScale.z);
            farArrow.transform.SetParent(transform);
            farArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(2.5f, 0.002f, 6.0f), arrowPrefab.transform.rotation);
            farArrowEmpty.GetComponent<ArrowController>().InitializeArrow("far", "blue", "empty");
            farArrowEmpty.transform.localScale = new Vector3(farArrow.transform.localScale.x, 10.5f, farArrow.transform.localScale.z);
            farArrowEmpty.transform.SetParent(transform);
        }
        else
        {
            _unitType = "Stradioti";
            _unitCaption.transform.position = transform.position + new Vector3(1.0f, 0.0f, 2.0f);
            _unitCaption.transform.rotation = _unitCaption.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f);
            farArrow = Instantiate(arrowPrefab, transform.position + new Vector3(-0.5f, 0.002f, -6.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
            farArrow.GetComponent<ArrowController>().InitializeArrow("far", "yellow", "solid");
            farArrow.transform.localScale = new Vector3(farArrow.transform.localScale.x, 10.5f, farArrow.transform.localScale.z);
            farArrow.transform.SetParent(transform);
            farArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(-0.5f, 0.002f, -6.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
            farArrowEmpty.GetComponent<ArrowController>().InitializeArrow("far", "yellow", "empty");
            farArrowEmpty.transform.localScale = new Vector3(farArrow.transform.localScale.x, 10.5f, farArrow.transform.localScale.z);
            farArrowEmpty.transform.SetParent(transform);
        }
        farArrow.GetComponent<ArrowController>().ArrowId = _unitId + 4;
        farArrowEmpty.GetComponent<ArrowController>().ArrowId = _unitId + 4;
        farArrow.SetActive(false);
        farArrowEmpty.SetActive(false);
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
            case "far":
                return farArrowEmpty.GetComponent<ArrowController>().ArrowId;
        }
        return -1;
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
                farArrow.SetActive(true);
                farArrowEmpty.SetActive(true);
                for (int i = 0; i < _strength; i++)
                {
                    _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
                }
                forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                leftArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                rightArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                farArrowEmpty.GetComponent<ArrowController>().ShowArrow();
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
                forwardArrow.GetComponent<ArrowController>().HideArrow();
                leftArrow.GetComponent<ArrowController>().HideArrow();
                rightArrow.GetComponent<ArrowController>().HideArrow();
                farArrowEmpty.GetComponent<ArrowController>().HideArrow();
                farArrow.GetComponent<ArrowController>().HideArrow();
                forwardArrow.SetActive(false);
                forwardArrowEmpty.SetActive(false);
                leftArrow.SetActive(false);
                leftArrowEmpty.SetActive(false);
                rightArrow.SetActive(false);
                rightArrowEmpty.SetActive(false);
                farArrow.SetActive(false);
                farArrowEmpty.SetActive(false);
            }
        }
    }

    protected override void anyTileClicked(int idTile)
    {
        base.anyTileClicked(idTile);
        
        farArrowEmpty.GetComponent<ArrowController>().HideArrow();
        farArrow.GetComponent<ArrowController>().HideArrow();
        farArrow.SetActive(false);
        farArrowEmpty.SetActive(false);
    }

    protected override void anyAttackClicked(int idAttack, bool isCounterAttack)
    {
        base.anyAttackClicked(idAttack, isCounterAttack);
        farArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
    }

    protected override void UpdateMe(string mode)
    {
        Unit myUnit;
        List<Attack> tempAttacks;
        bool isActive;

        if (mode == "routtest") return;
        base.UpdateMe(mode);
        myUnit = BattleManager.Instance.GetUnit(UnitId);
        tempAttacks = null;
        tempAttacks = myUnit.GetAttacksByArrowId(farArrow.GetComponent<ArrowController>().ArrowId);
        if (tempAttacks != null)
        {
            isActive = false;
            foreach (Attack a in tempAttacks)
            {
                if (a.IsActive()) isActive = true;
            }
            if (isActive) ActivateAttack(farArrow.GetComponent<ArrowController>().ArrowId);
            else DeactivateAttack(farArrow.GetComponent<ArrowController>().ArrowId);
        }
    }

    public override void ActivateAttack(int arrowId)
    {
        base.ActivateAttack(arrowId);
        if (farArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            farArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            farArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
    }

    public override void DeactivateAttack(int arrowId)
    {
        base.DeactivateAttack(arrowId);
        if (farArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            farArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            farArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
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
            case "far":
                return farArrowEmpty.GetComponent<ArrowController>();
        }
        return null;
    }
}
