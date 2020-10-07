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
    protected bool isDisabled;
    protected bool _isPlaced;
    protected bool _isBlocked;
    protected bool isMoved;
    protected int _strength;
    protected string _unitType;
    protected string _unitCommander;
    protected int _morale;
    protected int _armyId;
    protected int _unitId;
    protected int _unitTileId;
    protected int blockingUnitId;
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
    protected virtual void anyAttackClicked(int idAttack, bool isCounterAttack)
    {
        forwardArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
        leftArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
        rightArrow.GetComponent<ArrowController>().ShowArrow(idAttack, isCounterAttack);
    }

    // deselects unit after any tile is clicked
    protected virtual void anyTileClicked(int idTile)
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

    protected virtual void myUnitClicked(int idUnit)
    {
        if(!BattleManager.Instance.isInputBlocked && BattleManager.Instance.gameMode != "deploy")
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

    protected IEnumerator DisableUnit() // called when unit is killed
    {
        for (int i = 0; i < _squads.Length; i++)
        {
            _squads[i].GetComponentInChildren<PawnController>().Disable();
        }
        _unitCaption.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.75f).SetEase(Ease.InBack);
        flag.transform.DOScale(new Vector3(0.0f, 0.0f, 0.0f), 0.75f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.75f);
        flag.SetActive(false);
        for (int i = 0; i < _squads.Length; i++)
        {
            _squads[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    protected virtual void UpdateMe(string mode)
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
            if(_strength > myUnit.strength) KillSquads(_strength - myUnit.strength);
            _strength = myUnit.strength;
        }
        if (_morale != myUnit.morale)    // check if unit lost morale
        {
            flag.GetComponent<FlagController>().ChangeBannerHeight(initialMorale, myUnit.morale);
            _morale = myUnit.morale;
        }
        //check if unit moved to front line
        if(myUnit.UnitMoved() && !isMoved)
        {
            isMoved = true;
            SetArrowsBlockValue(false);
            if (_armyId == 1)
            {
                transform.DOMoveZ(transform.position.z + BattleManager.Instance.boardFieldHeight, 1.0f).SetEase(Ease.InQuad);
                _unitTileId += -1;
            }
            if(_armyId == 2)
            {
                transform.DOMoveZ(transform.position.z - BattleManager.Instance.boardFieldHeight, 1.0f).SetEase(Ease.InQuad);
                _unitTileId += 1;
            }
        }
        //check if unit attacks are still active
        tempAttacks = null;
        tempAttacks = myUnit.GetAttacksByArrowId(forwardArrow.GetComponent<ArrowController>().ArrowId);
        if(tempAttacks != null)
        {
            isActive = false;
            foreach(Attack a in tempAttacks)
            {
                if (a.IsActive()) isActive = true;
            }
            if(isActive) ActivateAttack(forwardArrow.GetComponent<ArrowController>().ArrowId);
            else DeactivateAttack(forwardArrow.GetComponent<ArrowController>().ArrowId);
        }
        tempAttacks = null;
        tempAttacks = myUnit.GetAttacksByArrowId(rightArrow.GetComponent<ArrowController>().ArrowId);
        if (tempAttacks != null)
        {
            isActive = false;
            foreach (Attack a in tempAttacks)
            {
                if (a.IsActive()) isActive = true;
            }
            if (isActive) ActivateAttack(rightArrow.GetComponent<ArrowController>().ArrowId);
            else DeactivateAttack(rightArrow.GetComponent<ArrowController>().ArrowId);
        }
        tempAttacks = null;
        tempAttacks = myUnit.GetAttacksByArrowId(leftArrow.GetComponent<ArrowController>().ArrowId);
        if (tempAttacks != null)
        {
            isActive = false;
            foreach (Attack a in tempAttacks)
            {
                if (a.IsActive()) isActive = true;
            }
            if (isActive) ActivateAttack(leftArrow.GetComponent<ArrowController>().ArrowId);
            else DeactivateAttack(leftArrow.GetComponent<ArrowController>().ArrowId);
        }
    }

    protected void ChangePosition(int tileId)
    {
        float xpos = (tileId - 1) / BattleManager.Instance.boardHeight + 1;
        xpos = 4.0f + xpos * BattleManager.Instance.boardFieldWitdth - BattleManager.Instance.boardFieldWitdth * 0.25f;
        float zpos = (tileId-1) % BattleManager.Instance.boardHeight;
        zpos = -8.0f - zpos * BattleManager.Instance.boardFieldHeight; 
        transform.position = new Vector3(xpos, 0.05f, zpos);
        _unitTileId = tileId;
    }

    protected void PlaceOnTile(int uId, int tId)
    {
        if (uId == _unitId && !_isBlocked)
        {
            ShowAll();
            ChangePosition(tId);
            _isPlaced = true;
        }
        else
        {
            if (_armyId == 1)
            {
                if (tId - 1 == _unitTileId)
                {
                    _isBlocked = true;
                    blockingUnitId = uId;
                }
                else
                {
                    if (blockingUnitId == uId) _isBlocked = false;
                }
            }
            if(_armyId == 2)
            {
                if (tId + 1 == _unitTileId)
                {
                    _isBlocked = true;
                    blockingUnitId = uId;
                }
                else
                {
                    if (blockingUnitId == uId) _isBlocked = false;
                }
            }
        }
    }

    protected void UnitDeployUIClicked(int aId, int p, int uId, string uType, string commander)
    {
        if (isPlaced)
        {
            if (uId == UnitId && !isOutlined)
            {
                for (int i = 0; i < _strength; i++)
                {
                    _squads[i].GetComponentInChildren<PawnController>().EnableOutline();
                }
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
            }
        }
    }

    protected void UpdateOnStart()
    {
        if(_isPlaced) UpdateMe("GameStart");
    }

    protected virtual void OnEnable()
    {
        EventManager.onAttackClicked += anyAttackClicked;
        EventManager.onUnitClicked += myUnitClicked;
        EventManager.onTileClicked += anyTileClicked;
        EventManager.onResultMenuClosed += UpdateMe;
        EventManager.onUnitDeployed += PlaceOnTile;
        EventManager.onDeploymentStart += StartDeployment;
        EventManager.onGameStart += UpdateOnStart;
        EventManager.onUIDeployPressed += UnitDeployUIClicked;
    }

    protected virtual void OnDestroy()
    {
        EventManager.onAttackClicked -= anyAttackClicked;
        EventManager.onUnitClicked -= myUnitClicked;
        EventManager.onTileClicked -= anyTileClicked;
        EventManager.onResultMenuClosed -= UpdateMe;
        EventManager.onUnitDeployed -= PlaceOnTile;
        EventManager.onDeploymentStart -= StartDeployment;
        EventManager.onGameStart -= UpdateOnStart;
        EventManager.onUIDeployPressed -= UnitDeployUIClicked;
    }

    public int UnitId
    {
        get {return _unitId;}
    }

    public bool isPlaced
    {
        get { return _isPlaced; }
        set { _isPlaced = value; }
    }

    public string UnitType
    {
        get {return _unitType; }
    }

    public string UnitCommander
    {
        get { return _unitCommander; }
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

    protected void PlaceWidget(int position)    // called from child
    {
        deployWidget = Instantiate(deployUnitPrefab, deployUnitPrefab.transform.position, deployUnitPrefab.transform.rotation);
        deployWidget.transform.SetParent(uiCanvas.transform);
        deployWidget.GetComponent<RectTransform>().anchoredPosition = new Vector3(485.0f, -65.0f - position * 115.0f, 0.0f);
        deployWidget.GetComponent<DeployUIController>().InitializeDeploy(_unitType, _strength, _morale, position, _armyId, _unitId, _unitCommander);
    }

    protected void StartDeployment(int aId)
    {
        if (aId < 3 && GameManagerController.Instance.isPlayer2Human) HideAll();
        else
        {
            ShowAll();
            if (!isPlaced)
            {
                BattleManager.Instance.RemoveUnitController(gameObject);
                Destroy(gameObject);
            }
        }
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

    public bool isBlocked
    {
        get { return _isBlocked; }
        set { _isBlocked = value; }
    }

    public virtual void InitializeUnit(int unitId, int armyId, int tileId, int deployPosition, string commander)   //   armyId == 1 then blue else red
    {
        Color myColor;

        if (!isInitialized)
        {
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
            _unitId = unitId*10;
            _strength = initialStrength;
            _morale = initialMorale;
            _armyId = armyId;
            _squads = new GameObject[initialStrength];
            _unitCaption = GetComponentInChildren<TextMeshPro>();
            _unitCommander = commander;
            blockingUnitId = 0;
            isMoved = false;

            // set position based on id tile which it sits on
            ChangePosition(tileId);

            if (_armyId == 1)
            {
                forwardArrow = Instantiate(arrowPrefab, transform.position + new Vector3(1.14f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                forwardArrow.GetComponent<ArrowController>().InitializeArrow("forward", "blue", "solid");
                forwardArrow.transform.SetParent(transform);
                forwardArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(1.14f, 0.002f, 4.0f), arrowPrefab.transform.rotation);
                forwardArrowEmpty.GetComponent<ArrowController>().InitializeArrow("forward", "blue", "empty");
                forwardArrowEmpty.transform.SetParent(transform);
                leftArrow = Instantiate(arrowPrefab, transform.position + new Vector3(-1.25f, -0.002f, 4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 10.0f));
                leftArrow.GetComponent<ArrowController>().InitializeArrow("left", "blue", "solid");
                leftArrow.transform.SetParent(transform);
                leftArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(-1.25f, -0.002f, 4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 10.0f));
                leftArrowEmpty.GetComponent<ArrowController>().InitializeArrow("left", "blue", "empty");
                leftArrowEmpty.transform.SetParent(transform);
                rightArrow = Instantiate(arrowPrefab, transform.position + new Vector3(3.25f, -0.002f, 4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -10.0f));
                rightArrow.GetComponent<ArrowController>().InitializeArrow("right", "blue", "solid");
                rightArrow.transform.SetParent(transform);
                rightArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(3.25f, -0.002f, 4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, -10.0f));
                rightArrowEmpty.GetComponent<ArrowController>().InitializeArrow("right", "blue", "empty");
                rightArrowEmpty.transform.SetParent(transform);
                ColorUtility.TryParseHtmlString(BattleManager.Army1Color, out myColor);
                _unitCaption.color = myColor;
                flag = Instantiate(flagPrefab, transform.position + new Vector3(1.0f, 0.2f, -0.35f), flagPrefab.transform.rotation);
                flag.transform.SetParent(transform);

            }
            else
            {
                forwardArrow = Instantiate(arrowPrefab, transform.position + new Vector3(0.89f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                forwardArrow.GetComponent<ArrowController>().InitializeArrow("forward", "yellow", "solid");
                forwardArrow.transform.SetParent(transform);
                forwardArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(0.89f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 180.0f));
                forwardArrowEmpty.GetComponent<ArrowController>().InitializeArrow("forward", "yellow", "empty");
                forwardArrowEmpty.transform.SetParent(transform);
                leftArrow = Instantiate(arrowPrefab, transform.position + new Vector3(3.25f, -0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 190.0f));
                leftArrow.GetComponent<ArrowController>().InitializeArrow("left", "yellow", "solid");
                leftArrow.transform.SetParent(transform);
                leftArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(3.25f, -0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 190.0f));
                leftArrowEmpty.GetComponent<ArrowController>().InitializeArrow("left", "yellow", "empty");
                leftArrowEmpty.transform.SetParent(transform);
                rightArrow = Instantiate(arrowPrefab, transform.position + new Vector3(-1.0f, -0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 170.0f));
                rightArrow.GetComponent<ArrowController>().InitializeArrow("right", "yellow", "solid");
                rightArrow.transform.SetParent(transform);
                rightArrowEmpty = Instantiate(arrowPrefab, transform.position + new Vector3(-1.0f, 0.002f, -4.0f), arrowPrefab.transform.rotation * Quaternion.Euler(0.0f, 0.0f, 170.0f));
                rightArrowEmpty.GetComponent<ArrowController>().InitializeArrow("right", "yellow", "empty");
                rightArrowEmpty.transform.SetParent(transform);
                ColorUtility.TryParseHtmlString(BattleManager.Army2Color, out myColor);
                _unitCaption.color = myColor;
                flag = Instantiate(flagPrefab, transform.position + new Vector3(1.0f, 0.2f, 0.35f), flagPrefab.transform.rotation * Quaternion.Euler(0.0f, 180.0f, 0.0f));
                flag.transform.SetParent(transform);
            }
            forwardArrow.GetComponent<ArrowController>().ArrowId = _unitId + 1;
            forwardArrowEmpty.GetComponent<ArrowController>().ArrowId = _unitId + 1;
            leftArrow.GetComponent<ArrowController>().ArrowId = _unitId + 2;
            leftArrowEmpty.GetComponent<ArrowController>().ArrowId = _unitId + 2;
            rightArrow.GetComponent<ArrowController>().ArrowId = _unitId + 3;
            rightArrowEmpty.GetComponent<ArrowController>().ArrowId = _unitId + 3;
            forwardArrow.SetActive(false);
            forwardArrowEmpty.SetActive(false);
            leftArrow.SetActive(false);
            leftArrowEmpty.SetActive(false);
            rightArrow.SetActive(false);
            rightArrowEmpty.SetActive(false);
        }
        else Debug.Log("Tried to initialized UnitController again! Id: " + _unitId);
    }


    public virtual void ActivateAttack(int arrowId)
    {
        if (forwardArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            forwardArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
        if (leftArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            leftArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            leftArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
        if (rightArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            rightArrowEmpty.GetComponent<ArrowController>().isArrowActive = true;
            rightArrow.GetComponent<ArrowController>().isArrowActive = true;
        }
    }

    public virtual void DeactivateAttack(int arrowId)
    {
        if (forwardArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            forwardArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            forwardArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
        if (leftArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            leftArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            leftArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
        if (rightArrowEmpty.GetComponent<ArrowController>().ArrowId == arrowId)
        {
            rightArrowEmpty.GetComponent<ArrowController>().isArrowActive = false;
            rightArrow.GetComponent<ArrowController>().isArrowActive = false;
        }
    }

    public virtual int GetArrowId(string direction)    
    {
        switch(direction)
        {
            case "right":
                return rightArrowEmpty.GetComponent<ArrowController>().ArrowId;
            case "central":
                return forwardArrowEmpty.GetComponent<ArrowController>().ArrowId;
            case "left":
                return leftArrowEmpty.GetComponent<ArrowController>().ArrowId;
        }
        return -1;
    }

    public virtual ArrowController GetArrowController(string direction)
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

    public void SetArrowsBlockValue(bool value)
    {
        ArrowController ac;
        ac = forwardArrow.GetComponent<ArrowController>();
        ac.isArrowBlocked = value;
        ac = forwardArrowEmpty.GetComponent<ArrowController>();
        ac.isArrowBlocked = value;
        ac = leftArrow.GetComponent<ArrowController>();
        ac.isArrowBlocked = value;
        ac = leftArrowEmpty.GetComponent<ArrowController>();
        ac.isArrowBlocked = value;
        ac = rightArrow.GetComponent<ArrowController>();
        ac.isArrowBlocked = value;
        ac = rightArrowEmpty.GetComponent<ArrowController>();
        ac.isArrowBlocked = value;
    }

    public void ResetUnit()
    {
        PawnController pc;

        isDisabled = false;
        for (int i = 0; i < _squads.Length; i++)
        {
            _squads[i].SetActive(true);
            pc = _squads[i].GetComponentInChildren<PawnController>();
            pc.Enable();
        }
        gameObject.SetActive(true);
        _unitCaption.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        flag.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f); 
        flag.SetActive(true);
    }
}
