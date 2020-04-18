using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UnitController : MonoBehaviour
{
    protected bool isInitialized = false;
    protected bool isOutlined;
    protected bool isDisabled = false;
    protected int _strength;
    protected string _unitType;
    protected int _morale;
    protected int _armyId;
    protected int _unitId;
    protected int _unitTileId;
    protected TextMeshPro _unitCaption;
    protected GameObject[] _squads;
    protected GameObject forwardArrow;
    protected GameObject forwardArrowEmpty;
    protected GameObject leftArrow;
    protected GameObject leftArrowEmpty;
    protected GameObject rightArrow;
    protected GameObject rightArrowEmpty;
    protected GameObject flag;
    protected GameObject deployWidget;
    protected GameObject uiCanvas;
    [SerializeField] protected int initialStrength;
    [SerializeField] protected int initialMorale;
    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] protected GameObject flagPrefab;
    [SerializeField] protected GameObject unitSquadPrefab;
    [SerializeField] protected GameObject deployUnitPrefab;

    // called when arrow is clicked
    protected void anyAttackClicked(int idAttack, bool isCounterAttack)
    {
        forwardArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
        leftArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
        rightArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
    }

    // deselects unit after any tile is clicked
    protected void anyTileClicked(int idTile)
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
        forwardArrow.SetActive(false);
        forwardArrowEmpty.SetActive(false);
        leftArrow.SetActive(false);
        leftArrowEmpty.SetActive(false);
        rightArrow.SetActive(false);
        rightArrowEmpty.SetActive(false);
    }

    protected void myUnitClicked(int idUnit)
    {
        if(!BattleManager.isInputBlocked && BattleManager.gameMode != "deploy")
        {
            if (idUnit == UnitId && !isOutlined)
            {
                forwardArrow.SetActive(true);
                forwardArrowEmpty.SetActive(true);
                leftArrow.SetActive(true);
                leftArrowEmpty.SetActive(true);
                rightArrow.SetActive(true);
                rightArrowEmpty.SetActive(true);
                for (int i = 0; i < _strength; i++)
                {
                    _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
                }
                forwardArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                leftArrowEmpty.GetComponent<ArrowController>().ShowArrow();
                rightArrowEmpty.GetComponent<ArrowController>().ShowArrow();
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
                forwardArrow.SetActive(false);
                forwardArrowEmpty.SetActive(false);
                leftArrow.SetActive(false);
                leftArrowEmpty.SetActive(false);
                rightArrow.SetActive(false);
                rightArrowEmpty.SetActive(false);
            }
        }
    }

    protected void KillSquads(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (_strength - 1 >= 0)
            {
                _squads[_strength - 1].GetComponentInChildren<PawnController>().Disable();
                _strength--;
            }
        }
    }

    protected IEnumerator DisableUnit()
    {
        foreach (GameObject g in _squads)
        {
            g.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.75f).SetEase(Ease.InBack);
        }
        _unitCaption.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.75f).SetEase(Ease.InBack);
        flag.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.75f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.75f);
        flag.SetActive(false);
        foreach (GameObject g in _squads)
        {
            g.SetActive(false);
        }
        gameObject.SetActive(false);
    }

    protected void UpdateMe(string mode)
    {
        Unit myUnit;
        Attack tempAttack;

        if (mode == "routtest") return;
        myUnit = BattleManager.Instance.GetUnit(UnitId);
        anyTileClicked(0);
        if (!myUnit.IsAvialable && !isDisabled)    // check if unit killed
        {
            isDisabled = true;
            EventManager.RaiseEventOnUnitDestroyed(UnitId);
            StartCoroutine(DisableUnit());
            return;
        }
        if (_strength > myUnit.strength)  // check if unit lost strength
        {
            KillSquads(_strength - myUnit.strength);
            _strength = myUnit.strength;
        }
        if (_morale > myUnit.morale)    // check if unit lost morale
        {
            flag.GetComponent<FlagController>().ChangeBannerHeight(initialMorale, myUnit.morale);
            _morale = myUnit.morale;
        }
        //check if unit attacks are still active
        tempAttack = myUnit.GetAttack(forwardArrow.GetComponent<ArrowController>().AttackId);
        if (tempAttack != null && !tempAttack.IsActive())
        {
            DeactivateAttack(tempAttack.GetId());
        }
        else if(tempAttack != null && tempAttack.IsActive())
        {
            ActivateAttack(tempAttack.GetId());
        }
        tempAttack = myUnit.GetAttack(rightArrow.GetComponent<ArrowController>().AttackId);
        if (tempAttack != null && !tempAttack.IsActive())
        {
            DeactivateAttack(tempAttack.GetId());
        }
        else if (tempAttack != null && tempAttack.IsActive())
        {
            ActivateAttack(tempAttack.GetId());
        }
        tempAttack = myUnit.GetAttack(leftArrow.GetComponent<ArrowController>().AttackId);
        if (tempAttack != null && !tempAttack.IsActive())
        {
            DeactivateAttack(tempAttack.GetId());
        }
        else if (tempAttack != null && tempAttack.IsActive())
        {
            ActivateAttack(tempAttack.GetId());
        }
    }

    protected void ChangePosition(int tileId)
    {
        float xpos = (tileId / BattleManager.boardHeight + 2) * BattleManager.boardFieldWitdth - BattleManager.boardFieldWitdth * 0.25f;
        float zpos = (tileId % BattleManager.boardHeight) * -1.0f * BattleManager.boardFieldHeight - BattleManager.boardFieldHeight;
        transform.position = new Vector3(xpos, 0.05f, zpos);
        _unitTileId = tileId;
    }

    protected void PlaceOnTile(int uId, int tId)
    {
        if(uId == _unitId)
        {
            ShowAll();
            ChangePosition(tId);
        }
    }

    protected void OnDestroy()
    {
        EventManager.onAttackClicked -= anyAttackClicked;
        EventManager.onUnitClicked -= myUnitClicked;
        EventManager.onTileClicked -= anyTileClicked;
        EventManager.onResultMenuClosed -= UpdateMe;
        EventManager.onUnitDeployed -= PlaceOnTile;
    }

    public int UnitId
    {
        get {return _unitId;}
    }

    public string UnitType
    {
        get {return _unitType; }
    }

    public int InitialStrength
    {
        get { return initialStrength; }
    }

    public int InitialMorale
    {
        get { return initialMorale; }
    }

    public int ArmyId
    {
        get { return _armyId; }
    }

    public int UnitTileId
    {
        get { return _unitTileId; }
    }

    private void Awake()
    {
        uiCanvas = GameObject.Find("uiCanvas");

    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += anyAttackClicked;
        EventManager.onUnitClicked += myUnitClicked;
        EventManager.onTileClicked += anyTileClicked;
        EventManager.onResultMenuClosed += UpdateMe;
        EventManager.onUnitDeployed += PlaceOnTile;
    }

    protected void PlaceWidget(int position)
    {
        deployWidget = Instantiate(deployUnitPrefab, deployUnitPrefab.transform.position, deployUnitPrefab.transform.rotation);
        deployWidget.transform.SetParent(uiCanvas.transform);
        deployWidget.GetComponent<RectTransform>().anchoredPosition = new Vector3(55.0f, -65.0f - position * 115.0f, 0.0f);
        deployWidget.GetComponent<DeployUIController>().InitializeDeploy(_armyId, _unitType, _strength, _morale, position, _armyId, _unitId);
    }

    public void HideAll()
    {
        _unitCaption.gameObject.SetActive(false);
        flag.gameObject.SetActive(false);
        for(int i =0; i<_squads.Length; i++)
        {
            _squads[i].SetActive(false);
        }
    }

    protected void ShowAll()
    {
        _unitCaption.gameObject.SetActive(true);
        flag.gameObject.SetActive(true);
        for (int i = 0; i < _squads.Length; i++)
        {
            _squads[i].SetActive(true);
        }
    }

    public virtual void InitializeUnit(int unitId, int armyId, int forwardAttackId, int leftAttackId, int rightAttackId, int tileId, int deployPosition)   //   armyId == 1 then blue else red
    {
        Color myColor;

        if (!isInitialized)
        {
            if (initialStrength > BattleManager.maxSquads)
            {
                Debug.Log("Tried to create too much squads in unit! Id: " + unitId);
                return;
            }
            isInitialized = true;
            isOutlined = false;
            _unitId = unitId;
            _strength = initialStrength;
            _morale = initialMorale;
            _armyId = armyId;
            _squads = new GameObject[initialStrength];
            _unitCaption = GetComponentInChildren<TextMeshPro>();

            // set position based on id tile which it sits on
            ChangePosition(tileId);

            if (_armyId == 1)
            {
                forwardArrow = Instantiate(arrowPrefab, transform.position + new Vector3(1.0f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                forwardArrow.GetComponent<ArrowController>().InitializeArrow("forward", "blue", "solid");
                forwardArrow.transform.SetParent(transform);
                forwardArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(1.0f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                forwardArrowEmpty.GetComponent<ArrowController>().InitializeArrow("forward", "blue", "empty");
                forwardArrowEmpty.transform.SetParent(transform);
                leftArrow = Instantiate(arrowPrefab, transform.position + new Vector3(-0.5f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                leftArrow.GetComponent<ArrowController>().InitializeArrow("left", "blue", "solid");
                leftArrow.transform.SetParent(transform);
                leftArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(-0.5f, 0.02f, 4.0f), arrowPrefab.transform.rotation);
                leftArrowEmpty.GetComponent<ArrowController>().InitializeArrow("left", "blue", "empty");
                leftArrowEmpty.transform.SetParent(transform);
                rightArrow = Instantiate(arrowPrefab, transform.position + new Vector3(2.5f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                rightArrow.GetComponent<ArrowController>().InitializeArrow("right", "blue", "solid");
                rightArrow.transform.SetParent(transform);
                rightArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(2.5f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                rightArrowEmpty.GetComponent<ArrowController>().InitializeArrow("right", "blue", "empty");
                rightArrowEmpty.transform.SetParent(transform);
                ColorUtility.TryParseHtmlString(BattleManager.Army1Color, out myColor);
                _unitCaption.color = myColor;
                flag = Instantiate(flagPrefab, transform.position + new Vector3(1.0f, 0.2f, -0.35f), flagPrefab.transform.rotation);
                flag.transform.SetParent(transform);

            }
            else
            {
                forwardArrow = Instantiate(arrowPrefab, transform.position + new Vector3(1.0f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                forwardArrow.GetComponent<ArrowController>().InitializeArrow("forward", "yellow", "solid");
                forwardArrow.transform.SetParent(transform);
                forwardArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(1.0f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                forwardArrowEmpty.GetComponent<ArrowController>().InitializeArrow("forward", "yellow", "empty");
                forwardArrowEmpty.transform.SetParent(transform);
                leftArrow = Instantiate(arrowPrefab, transform.position + new Vector3(2.5f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                leftArrow.GetComponent<ArrowController>().InitializeArrow("left", "yellow", "solid");
                leftArrow.transform.SetParent(transform);
                leftArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(2.5f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                leftArrowEmpty.GetComponent<ArrowController>().InitializeArrow("left", "yellow", "empty");
                leftArrowEmpty.transform.SetParent(transform);
                rightArrow = Instantiate(arrowPrefab, transform.position + new Vector3(-0.5f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                rightArrow.GetComponent<ArrowController>().InitializeArrow("right", "yellow", "solid");
                rightArrow.transform.SetParent(transform);
                rightArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(-0.5f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                rightArrowEmpty.GetComponent<ArrowController>().InitializeArrow("right", "yellow", "empty");
                rightArrowEmpty.transform.SetParent(transform);
                ColorUtility.TryParseHtmlString(BattleManager.Army2Color, out myColor);
                _unitCaption.color = myColor;
                flag = Instantiate(flagPrefab, transform.position + new Vector3(1.0f, 0.2f, 0.35f), flagPrefab.transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
                flag.transform.SetParent(transform);
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
        }
        else Debug.Log("Tried to initialized UnitController again! Id: " + _unitId);
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

    public int GetAttackId(string direction)    
    {
        switch(direction)
        {
            case "right":
                return rightArrowEmpty.GetComponent<ArrowController>().AttackId;
            case "central":
                return forwardArrowEmpty.GetComponent<ArrowController>().AttackId;
            case "left":
                return leftArrowEmpty.GetComponent<ArrowController>().AttackId;
        }
        return -1;
    }

    public ArrowController GetArrowController(string direction)
    {
        switch (direction)
        {
            case "right":
                return rightArrowEmpty.GetComponent<ArrowController>();
            case "central":
                return forwardArrowEmpty.GetComponent<ArrowController>();
            case "left":
                return leftArrowEmpty.GetComponent<ArrowController>();
        }
        return null;
    }
}
