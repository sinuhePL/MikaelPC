using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitController : MonoBehaviour
{
    private bool isInitialized = false;
    private bool isOutlined;
    private int _squadNumber;
    private int _aliveSquadNumer;
    private int _morale;
    private int _armyId;
    private string _unitType;
    private int _unitId;
    private int _unitTileId;
    private GameObject[] _squads;
    private GameObject forwardArrow;
    private GameObject forwardArrowEmpty;
    private GameObject leftArrow;
    private GameObject leftArrowEmpty;
    private GameObject rightArrow;
    private GameObject rightArrowEmpty;
    private GameObject unitCaption;
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
    [SerializeField] private GameObject unitCaptionPrefab;

    // called when arrow is clicked
    private void myAttackClicked(int idAttack)
    {
        forwardArrow.GetComponent<ArrowController>().ShowArrow(idAttack);
        leftArrow.GetComponent<ArrowController>().ShowArrow(idAttack);
        rightArrow.GetComponent<ArrowController>().ShowArrow(idAttack);
    }

    private void myTileClicked(int idTile)
    {
        if (isOutlined)
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
            forwardArrow.SetActive(false);
            forwardArrowEmpty.SetActive(false);
            leftArrow.SetActive(false);
            leftArrowEmpty.SetActive(false);
            rightArrow.SetActive(false);
            rightArrowEmpty.SetActive(false);
            isOutlined = false;
        }
    }

    private void myUnitClicked(int idUnit)
    {
        if(idUnit == UnitId && !isOutlined)
        {
            forwardArrow.SetActive(true);
            forwardArrowEmpty.SetActive(true);
            leftArrow.SetActive(true);
            leftArrowEmpty.SetActive(true);
            rightArrow.SetActive(true);
            rightArrowEmpty.SetActive(true);
            for (int i = 0; i < _aliveSquadNumer; i++)
            {
                _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
            }
            forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow();
            leftArrowEmpty.GetComponent<ArrowController>().ShowArrow();
            rightArrowEmpty.GetComponent<ArrowController>().ShowArrow();
            isOutlined = true;
        }
        else if(isOutlined)
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
            forwardArrow.SetActive(false);
            forwardArrowEmpty.SetActive(false);
            leftArrow.SetActive(false);
            leftArrowEmpty.SetActive(false);
            rightArrow.SetActive(false);
            rightArrowEmpty.SetActive(false);
            isOutlined = false;
        }
    }

    private void KillSquads(int count)
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

    private void UpdateMe()
    {
        Unit myUnit;
        myUnit = BattleManager.Instance.GetUnit(UnitId);
        KillSquads(_aliveSquadNumer - myUnit.strength);
        _aliveSquadNumer = myUnit.strength;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= myAttackClicked;
        EventManager.onUnitClicked -= myUnitClicked;
        EventManager.onTileClicked -= myTileClicked;
        EventManager.onUpdateBoard -= UpdateMe;
    }

    public int UnitId
    {
        get {return _unitId;}
    }

    public string UnitType
    {
        get { return _unitType; }
    }

    public int InitialStrength
    {
        get { return _squadNumber; }
    }

    public int InitialMorale
    {
        get { return _morale; }
    }

    public int ArmyId
    {
        get { return _armyId; }
    }

    public int UnitTileId
    {
        get { return _unitTileId; }
    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += myAttackClicked;
        EventManager.onUnitClicked += myUnitClicked;
        EventManager.onTileClicked += myTileClicked;
        EventManager.onUpdateBoard += UpdateMe;
    }

    public void InitializeUnit(int squadNumber, int unitId, GameObject unitSquadPrefab, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, string unitType, int tileId)   //   armyId == 1 then blue else red
    {
        Vector3 tempPos;
        Color myColor;

        if (!isInitialized)
        {
            if (squadNumber > BattleManager.maxSquads)
            {
                Debug.Log("Tried to create too much squads in unit! Id: " + unitId);
                return;
            }
            isInitialized = true;
            isOutlined = false;
            _unitType = unitType;
            _unitId = unitId;
            _squadNumber = squadNumber;
            _aliveSquadNumer = squadNumber;
            _armyId = armyId;
            _unitTileId = tileId;
            _squads = new GameObject[_squadNumber];
            
            // set position based on id tile which it sits on 
            float xpos = (tileId % BattleManager.boardWidth) * BattleManager.boardFieldWitdth + BattleManager.boardFieldWitdth - BattleManager.boardFieldWitdth*0.25f;
            float zpos = (tileId % BattleManager.boardHeight) * -1.0f * BattleManager.boardFieldHeight - BattleManager.boardFieldHeight /*- BattleManager.boardFieldHeight * 0.4f*/;
            transform.position = new Vector3(xpos, 0.05f, zpos);

            if (_armyId == 1)
            {
                forwardArrow = Instantiate(arrowForwardBluePrefab, transform.position + new Vector3(1.0f, 0.0f, 4.0f), arrowForwardBluePrefab.transform.rotation);
                forwardArrowEmpty = Instantiate(arrowForwardBlueEmptyPrefab, transform.position + new Vector3(1.0f, 0.0f, 4.0f), arrowForwardBlueEmptyPrefab.transform.rotation);
                leftArrow = Instantiate(arrowLeftBluePrefab, transform.position + new Vector3(-0.5f, 0.0f, 4.0f), arrowLeftBluePrefab.transform.rotation);
                leftArrowEmpty = Instantiate(arrowLeftBlueEmptyPrefab, transform.position + new Vector3(-0.5f, 0.0f, 4.0f), arrowLeftBlueEmptyPrefab.transform.rotation);
                rightArrow = Instantiate(arrowRightBluePrefab, transform.position + new Vector3(2.5f, 0.0f, 4.0f), arrowRightBluePrefab.transform.rotation);
                rightArrowEmpty = Instantiate(arrowRightBlueEmptyPrefab, transform.position + new Vector3(2.5f, 0.0f, 4.0f), arrowRightBlueEmptyPrefab.transform.rotation);
                unitCaption = Instantiate(unitCaptionPrefab, transform.position + new Vector3(0.7f, 0.0f, -1.3f), unitCaptionPrefab.transform.rotation);
                ColorUtility.TryParseHtmlString(BattleManager.Army1Color, out myColor);
                unitCaption.GetComponent<TextMeshPro>().color = myColor;

            }
            else
            {
                forwardArrow = Instantiate(arrowForwardRedPrefab, transform.position + new Vector3(1.0f, 0.0f, -4.0f), arrowForwardRedPrefab.transform.rotation);
                forwardArrowEmpty = Instantiate(arrowForwardRedEmptyPrefab, transform.position + new Vector3(1.0f, 0.0f, -4.0f), arrowForwardRedEmptyPrefab.transform.rotation);
                leftArrow = Instantiate(arrowLeftRedPrefab, transform.position + new Vector3(2.5f, 0.0f, -4.0f), arrowLeftRedPrefab.transform.rotation);
                leftArrowEmpty = Instantiate(arrowLeftRedEmptyPrefab, transform.position + new Vector3(2.5f, 0.0f, -4.0f), arrowLeftRedEmptyPrefab.transform.rotation);
                rightArrow = Instantiate(arrowRightRedPrefab, transform.position + new Vector3(-0.5f, 0.0f, -4.0f), arrowRightRedPrefab.transform.rotation);
                rightArrowEmpty = Instantiate(arrowRightRedEmptyPrefab, transform.position + new Vector3(-0.5f, 0.0f, -4.0f), arrowRightRedEmptyPrefab.transform.rotation);
                unitCaption = Instantiate(unitCaptionPrefab, transform.position + new Vector3(1.3f, 0.0f, 1.2f), unitCaptionPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                ColorUtility.TryParseHtmlString(BattleManager.Army2Color, out myColor);
                unitCaption.GetComponent<TextMeshPro>().color = myColor;
            }
            switch (unitType)
            {
                case "French Cavalery":
                    _morale = 5;
                    unitCaption.GetComponent<TextMeshPro>().text = "French Cavalery";
                    break;
            }
            forwardArrow.GetComponent<ArrowController>().AttackId = forwardAttackId;
            forwardArrowEmpty.GetComponent<ArrowController>().AttackId = forwardAttackId;
            leftArrow.GetComponent<ArrowController>().AttackId = leftAttackId;
            leftArrowEmpty.GetComponent<ArrowController>().AttackId = leftAttackId;
            rightArrow.GetComponent<ArrowController>().AttackId = rightAttackId;
            rightArrowEmpty.GetComponent<ArrowController>().AttackId = rightAttackId;
            forwardArrow.SetActive(false);
            forwardArrowEmpty.SetActive(false);
            leftArrow.SetActive(false);
            leftArrowEmpty.SetActive(false);
            rightArrow.SetActive(false);
            rightArrowEmpty.SetActive(false);
            // inicjalizacja squadów
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
        }
        else Debug.Log("Tried to initialized CavaleryController again! Id: " + _unitId);
    }


    public void ActivateAttack(int attackId)
    {
        if (forwardArrowEmpty.GetComponent<ArrowController>().AttackId == attackId)
        {
            forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            forwardArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
        if (leftArrowEmpty.GetComponent<ArrowController>().AttackId == attackId)
        {
            leftArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            leftArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
        if (rightArrowEmpty.GetComponent<ArrowController>().AttackId == attackId)
        {
            rightArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            rightArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
    }

    public void DeactivateAttack(int attackId)
    {
        if (forwardArrowEmpty.GetComponent<ArrowController>().AttackId == attackId)
        {
            forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            forwardArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
        if (leftArrowEmpty.GetComponent<ArrowController>().AttackId == attackId)
        {
            leftArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            leftArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
        if (rightArrowEmpty.GetComponent<ArrowController>().AttackId == attackId)
        {
            rightArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            rightArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
    }

    // return attack id based on code - 1 - right attack 2 - central attack 3 - right attack
    public int GetAttackId(int myAttack)    
    {
        switch(myAttack)
        {
            case 1:
                return leftArrowEmpty.GetComponent<ArrowController>().AttackId;
            case 2:
                return forwardArrowEmpty.GetComponent<ArrowController>().AttackId;
            case 3:
                return rightArrowEmpty.GetComponent<ArrowController>().AttackId;
        }
        return -1;
    }
}
