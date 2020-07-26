using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleManager : MonoBehaviour {

    private struct UnitPlacementHelp
    {
        public int unitId;
        public int tileId;
        public int points;

        public UnitPlacementHelp(int uId, int tId, int p)
        {
            unitId = uId;
            tileId = tId;
            points = p;
        }
    }

    private static BattleManager _instance;
    private BoardState myBoardState;
    private List<GameObject> units;
    private List<GameObject> tiles;
    private Camera myCamera;
    private int armyRouteTest;
    private float[] routProbabilityTable;

    public static BattleManager Instance { get { return _instance; } }
    [SerializeField] private Transform marginBottom;
    [SerializeField] private Transform marginCornerLeftBottom;
    [SerializeField] private Transform marginCornerLeftTop;
    [SerializeField] private Transform marginCornerRightBottom;
    [SerializeField] private Transform marginCornerRightTop;
    [SerializeField] private Transform marginLeft;
    [SerializeField] private Transform marginRight;
    [SerializeField] private Transform marginTop;
    [SerializeField] private Transform marginAround;
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject gendarmesPrefab;
    [SerializeField] private GameObject frenchLandsknechtePrefab;
    [SerializeField] private GameObject frenchArquebusiersPrefab;
    [SerializeField] private GameObject frenchArtilleryPrefab;
    [SerializeField] private GameObject frenchCoustilliersPrefab;
    [SerializeField] private GameObject suissePrefab;
    [SerializeField] private GameObject imperialLandsknechtePrefab;
    [SerializeField] private GameObject imperialLandsknechtePrefab2;
    [SerializeField] private GameObject imperialCavaleryPrefab;
    [SerializeField] private GameObject imperialArquebusiersPrefab;
    [SerializeField] private GameObject imperialArtilleryPrefab;
    [SerializeField] private GameObject imperialStradiotiPrefab;


    public int boardWidth = 7;
    public int boardHeight = 5;
    public float boardFieldWitdth = 4.0f;
    public float boardFieldHeight = 4.0f;
    public int maxSquads = 12;
    public int turnOwnerId = 1;
    public bool hasTurnOwnerAttacked = false;
    public bool isInputBlocked = false;
    public string gameMode = "deploy";

    public const string Army1Color = "#4158f3";
    public const string Army2Color = "#ecc333";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        InitiateManager(boardWidth, boardHeight);
        //EventManager.RaiseEventOnDeploymentStart(1);
        //InitiateBoard();
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += DiceThrown;
        EventManager.onAttackOrdered += MakeAttack;
        EventManager.onTurnStart += TurnStart;
        EventManager.onResultMenuClosed += MakeRouteTest;
        EventManager.onDeploymentStart += DeployArmy;
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= DiceThrown;
        EventManager.onAttackOrdered -= MakeAttack;
        EventManager.onTurnStart -= TurnStart;
        EventManager.onResultMenuClosed -= MakeRouteTest;
        EventManager.onDeploymentStart -= DeployArmy;
    }

    private void Start()
    {
        routProbabilityTable = new float[] {1.0f, 1.0f, 1.0f, 0.999f, 0.996f, 0.99f, 0.98f, 0.965f, 0.944f, 0.916f, 0.88f, 0.835f, 0.78f, 0.717f, 0.648f, 0.575f, 0.5f, 0.425f, 0.352f, 0.283f, 0.22f, 0.165f, 0.12f, 0.084f, 0.056f, 0.035f, 0.02f, 0.01f, 0.004f, 0.001f, 0.0f};
        //EventManager.RaiseEventGameStart();
        EventManager.RaiseEventOnDeploymentStart(1);
    }

    private void InitiateManager(int _boardWidth, int _boardHeight)
    {
        GameObject tempObj;
        int tileCounter = 0, keyFieldCounter = 0, tempKeyFieldId, possibleArmyDeployment;

        units = new List<GameObject>();
        tiles = new List<GameObject>();
        Random.InitState(System.Environment.TickCount);
        myCamera = Camera.main;
        armyRouteTest = 0;
        //inicjalizacja elementów graficznych planszy
        for (int i=0; i< _boardWidth+4; i++)
        {
            for(int j=0;j<_boardHeight+4;j++)
            {
                if(j==0 || i ==0 || j==_boardHeight + 3 || i == _boardWidth + 3) Instantiate(marginAround, new Vector3(i * 4.0f, 0.0f, j * -4.0f), marginAround.transform.rotation);
                else if (j == 1 && i == 1) Instantiate(marginCornerLeftTop, new Vector3(4.0f, 0.0f, -4.0f), marginCornerLeftTop.transform.rotation);
                else if (j == 1 && i < _boardWidth + 2) Instantiate(marginTop, new Vector3(i * 4.0f, 0.0f, -4.0f), marginTop.transform.rotation);
                else if (j == 1 && i == _boardWidth + 2) Instantiate(marginCornerRightTop, new Vector3(i * 4.0f, 0.0f, -4.0f), marginCornerRightTop.transform.rotation);
                else if (i == 1 && j < _boardHeight + 2) Instantiate(marginLeft, new Vector3(4.0f, 0.0f, j * -4.0f), marginLeft.transform.rotation);
                else if (i == 1 && j == _boardHeight + 2) Instantiate(marginCornerLeftBottom, new Vector3(4.0f, 0.0f, j * -4.0f), marginCornerLeftBottom.transform.rotation);
                else if (i == _boardWidth + 2 && j < _boardHeight + 2) Instantiate(marginRight, new Vector3(i * 4.0f, 0.0f, j * -4.0f), marginRight.transform.rotation);
                else if (i < _boardWidth + 2 && j == _boardHeight + 2) Instantiate(marginBottom, new Vector3(i * 4.0f, 0.0f, j * -4.0f), marginBottom.transform.rotation);
                else if (i == _boardWidth + 2 && j == _boardHeight + 2) Instantiate(marginCornerRightBottom, new Vector3(i * 4.0f, 0.0f, j * -4.0f), marginCornerRightBottom.transform.rotation);
                else
                {
                    int r = Random.Range(0, 5);
                    tempObj = Instantiate(tilePrefab, new Vector3(i * 4.0f, 0.0f, j * -4.0f), tilePrefab.transform.rotation);
                    // add condition on creating key field
                    if (j == 4 && (r < 3) && i > 2 && i < _boardWidth + 1) tempKeyFieldId = ++keyFieldCounter;
                    else tempKeyFieldId = 0;
                    possibleArmyDeployment = 0;
                    if (j == 3 && i > 2 && i < _boardWidth+1) possibleArmyDeployment = 2;
                    if(j == 5 && i > 2 && i < _boardWidth+1) possibleArmyDeployment = 1;
                    switch (r)
                    {
                        case 0:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "forest", possibleArmyDeployment);
                            break;
                        case 1:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "hill", possibleArmyDeployment);
                            break;
                        case 2:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "town", possibleArmyDeployment);
                            break;
                        default:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "field", possibleArmyDeployment);
                            break;
                    }
                    tiles.Add(tempObj);
                }
            }
        }
    }

    private void DeployEasy(int armyId)
    {
        List<int> possibleTiles = new List<int>();
        foreach (GameObject t in tiles)
        {
            TileController tc;
            tc = t.GetComponent<TileController>();
            if (tc.DeploymentPossible(armyId)) possibleTiles.Add(tc.tileId); //checks if deployment possible for army aId on this tile and adds it to list of possible tiles
        }
        foreach (GameObject g in units)
        {
            UnitController uc;
            int tempId = Random.Range(0, possibleTiles.Count);
            uc = g.GetComponent<UnitController>();
            if (uc.ArmyId == armyId)
            {
                EventManager.RaiseEventOnUnitDeployed(uc.UnitId, possibleTiles[tempId]);
                possibleTiles.RemoveAt(tempId);
            }
        }
        EventManager.RaiseEventOnDeploymentStart(armyId+1);
    }

    private void DeployMedium(int armyId)
    {
        int tileUnitResult, tempTile, tempUnit;
        List<TileController> possibleTiles = new List<TileController>();
        List<UnitPlacementHelp> placementList = new List<UnitPlacementHelp>();  // list of possible pairs unit - tile
        List<UnitPlacementHelp> placementToRemove;
        UnitPlacementHelp tempPlacementHelp;
        foreach (GameObject t in tiles) // looks for tiles with possible placement
        {
            TileController tc;
            tc = t.GetComponent<TileController>();
            if (tc.DeploymentPossible(armyId)) possibleTiles.Add(tc); //checks if deployment possible for army aId on this tile and adds it to list of possible tiles
        }
        foreach (GameObject g in units) // creates list of every possible pair tile - unit
        {
            UnitController uc;
            uc = g.GetComponent<UnitController>();
            if (uc.ArmyId == armyId)
            {
                foreach (TileController tc2 in possibleTiles)
                {
                    tileUnitResult = tc2.GetUnitValue(uc.UnitType);    // gets value of pair tile - unit
                    tempPlacementHelp = new UnitPlacementHelp(uc.UnitId, tc2.tileId, tileUnitResult);
                    placementList.Add(tempPlacementHelp);
                }
            }
        }
        placementList.Sort((p1,p2)=>p1.points.CompareTo(p2.points));    // sorts list of possible pairs unit - tile by value
        while (placementList.Count > 0) // do until all units are placed
        {
            EventManager.RaiseEventOnUnitDeployed(placementList[0].unitId, placementList[0].tileId); // place an unit
            tempTile = placementList[0].tileId;
            tempUnit = placementList[0].unitId;
            placementToRemove = new List<UnitPlacementHelp>();
            foreach (UnitPlacementHelp uph in placementList)    // looks for elements to remove and stores them in separate list
            {
                if (uph.tileId == tempTile || uph.unitId == tempUnit) placementToRemove.Add(uph);
            }
            foreach(UnitPlacementHelp uph2 in placementToRemove)    // removes from list of possible pair tile - unit
            {
                placementList.Remove(uph2);
            }
        }
        EventManager.RaiseEventOnDeploymentStart(armyId + 1);
    }

    private void DeployHard(int armyId)
    {
        int tileUnitValue, tempTile, tempUnit;
        TileController oppositeTile;
        List<TileController> possibleTiles = new List<TileController>();
        List<UnitPlacementHelp> placementList = new List<UnitPlacementHelp>();  // list of possible pairs unit - tile
        List<UnitPlacementHelp> placementToRemove;
        UnitPlacementHelp tempPlacementHelp;
        foreach (GameObject t in tiles) // looks for tiles with possible placement
        {
            TileController tc;
            tc = t.GetComponent<TileController>();
            if (tc.DeploymentPossible(armyId)) possibleTiles.Add(tc); //checks if deployment possible for army aId on this tile and adds it to list of possible tiles
        }
        foreach (GameObject g in units) // creates list of every possible pair tile - unit
        {
            UnitController uc;
            uc = g.GetComponent<UnitController>();
            if (uc.ArmyId == armyId)
            {
                foreach (TileController tc2 in possibleTiles)
                {
                    tileUnitValue = tc2.GetUnitValue(uc.UnitType);    // gets value of pair tile - unit
                    if (uc.ArmyId == 1)
                    {
                        if ((tc2.tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) oppositeTile = GetTile(tc2.tileId - 3);
                        else oppositeTile = GetTile(tc2.tileId - 2);
                    }
                    else
                    {
                        if((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) oppositeTile = GetTile(tc2.tileId + 3);
                        else oppositeTile = GetTile(tc2.tileId + 2);
                    }
                    tileUnitValue += oppositeTile.GetOpposingUnitValue(uc.UnitType);
                    tempPlacementHelp = new UnitPlacementHelp(uc.UnitId, tc2.tileId, tileUnitValue);
                    placementList.Add(tempPlacementHelp);
                }
            }
        }
        placementList.Sort((p1, p2) => p1.points.CompareTo(p2.points));    // sorts list of possible pairs unit - tile by value
        while (placementList.Count > 0) // do until all units are placed
        {
            EventManager.RaiseEventOnUnitDeployed(placementList[0].unitId, placementList[0].tileId); // place an unit
            tempTile = placementList[0].tileId;
            tempUnit = placementList[0].unitId;
            placementToRemove = new List<UnitPlacementHelp>();
            foreach (UnitPlacementHelp uph in placementList)    // looks for elements to remove and stores them in separate list
            {
                if (uph.tileId == tempTile || uph.unitId == tempUnit) placementToRemove.Add(uph);
            }
            foreach (UnitPlacementHelp uph2 in placementToRemove)    // removes from list of possible pair tile - unit
            {
                placementList.Remove(uph2);
            }
        }
        EventManager.RaiseEventOnDeploymentStart(armyId + 1);
    }

    private void DeployArmy(int armyId)
    {
        GameObject tempObj;

        if(armyId < 3) BattleManager.Instance.turnOwnerId = armyId;
        if (armyId == 1)
        {
            tempObj = Instantiate(gendarmesPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(1, 1, 1, 0, "Francis I");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(frenchLandsknechtePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(2, 1, 1, 1, "de Lorraine");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(suissePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(3, 1, 1, 2, "de La Marck");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(frenchArquebusiersPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(4, 1, 1, 3, "de la Pole");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(frenchArtilleryPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(5, 1, 1, 4, "de Genouillac");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(frenchCoustilliersPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(6, 1, 1, 5, "Tiercelin");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);
            if (!GameManagerController.Instance.isPlayer1Human) // places units on board
            {
                if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.easy) DeployEasy(armyId);
                else if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.medium) DeployMedium(armyId);
                else if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard) DeployHard(armyId);
            }
        }
        else if(armyId == 2)
        {
            tempObj = Instantiate(imperialLandsknechtePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(7, 2, 1, 0, "von Frundsberg");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialCavaleryPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(8, 2, 1, 1, "de Lannoy");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialArquebusiersPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(9, 2, 1, 2, "de Vasto");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialArtilleryPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(10, 2, 1, 3, "");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialStradiotiPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(11, 2, 1, 4, "");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialLandsknechtePrefab2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            tempObj.GetComponent<UnitController>().InitializeUnit(12, 2, 1, 5, "Pescara");
            tempObj.GetComponent<UnitController>().HideAll();
            units.Add(tempObj);
            if (!GameManagerController.Instance.isPlayer2Human)
            {
                if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.easy) DeployEasy(armyId);
                else if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.medium) DeployMedium(armyId);
                else if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard) DeployHard(armyId);
            }
        }
        else if(armyId == 3)
        {
            InitiateBoard();
        }
    }

    private void InitiateBoard()
    {
        UnitController uc, uc2;
        TileController tc;
        Unit myUnit, otherUnit;
        KeyField myKeyField;
        ArrowController ac;
        Army army1, army2;
        Attack tempAttack;
        Vector3 arrowRightPositionShift, arrowCentralPositionShift, arrowLeftPositionShift;
        int army1morale = 0, army2morale = 0, leftAttackTile, centralAttackTile, rightAttackTile, keyFieldTile, myKeyFieldId, tempAttackId, leftAttackTileSupport, centralAttackTileSupport, rightAttackTileSupport, supportTile;
        List<int> attackList;

        // counts morale of army
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced) continue;
            if (uc.ArmyId == 1) army1morale += uc.InitialMorale;
            else army2morale += uc.InitialMorale;
        }
        army1 = new Army(1, army1morale, 0, 6, "France");
        army2 = new Army(2, army2morale, 0, 6, "HRE");
        myBoardState = new BoardState(army1, army2);

        // creates in memory representation of each unit on screen
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced) continue;
            myUnit = new Unit(uc.UnitId, uc.UnitType, uc.InitialStrength, uc.InitialMorale, uc.ArmyId == 1 ? army1 : army2, uc.UnitCommander);
            supportTile = 0;
            myKeyFieldId = 0;
            // sets arrow position depending on direction of attack (left, central right)
            if (uc.ArmyId == 1)
            {
                arrowLeftPositionShift = new Vector3(1.0f, 0.0f, 5.0f);
                arrowCentralPositionShift = new Vector3(3.0f, 0.0f, 5.0f);
                arrowRightPositionShift = new Vector3(5.0f, 0.0f, 5.0f);
            }
            else
            {
                arrowLeftPositionShift = new Vector3(1.0f, 0.0f, -3.0f);
                arrowCentralPositionShift = new Vector3(3.0f, 0.0f, -3.0f);
                arrowRightPositionShift = new Vector3(5.0f, 0.0f, -3.0f);
            }

            if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) // if unit in second line
            {
                //looks for tiles ids which attack arrows point at
                if (uc.ArmyId == 1)
                {
                    uc.SetArrowsBlockValue(true);
                    leftAttackTile = uc.UnitTileId - 8;
                    centralAttackTile = uc.UnitTileId - 3;
                    rightAttackTile = uc.UnitTileId + 2;
                    leftAttackTileSupport = uc.UnitTileId - 9;
                    centralAttackTileSupport = uc.UnitTileId - 4;
                    rightAttackTileSupport = uc.UnitTileId + 1;
                    if (leftAttackTile <= 5) leftAttackTile = 0;
                    if (leftAttackTileSupport <= 5) leftAttackTileSupport = 0;
                    if (rightAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTile = 0;
                    if (rightAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTileSupport = 0;
                    keyFieldTile = uc.UnitTileId - 2;
                }
                else
                {
                    uc.SetArrowsBlockValue(true);
                    leftAttackTile = uc.UnitTileId + 8;
                    centralAttackTile = uc.UnitTileId + 3;
                    rightAttackTile = uc.UnitTileId - 2;
                    leftAttackTileSupport = uc.UnitTileId + 9;
                    centralAttackTileSupport = uc.UnitTileId + 4;
                    rightAttackTileSupport = uc.UnitTileId - 1;
                    if (rightAttackTile <= 5) rightAttackTile = 0;
                    if (rightAttackTileSupport <= 5) rightAttackTileSupport = 0;
                    if (leftAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTile = 0;
                    if (leftAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTileSupport = 0;
                    keyFieldTile = uc.UnitTileId + 2;
                }
            }
            else   // if unit in first line
            {
                //looks for tiles ids which attack arrows point at
                if (uc.ArmyId == 1)
                {
                    supportTile = uc.UnitTileId + 1;
                    leftAttackTile = uc.UnitTileId - 7;
                    centralAttackTile = uc.UnitTileId - 2;
                    rightAttackTile = uc.UnitTileId + 3;
                    leftAttackTileSupport = uc.UnitTileId - 8;
                    centralAttackTileSupport = uc.UnitTileId - 3;
                    rightAttackTileSupport = uc.UnitTileId + 2;
                    if (leftAttackTile <= 5) leftAttackTile = 0;
                    if (leftAttackTileSupport <= 5) leftAttackTileSupport = 0;
                    if (rightAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTile = 0;
                    if (rightAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTileSupport = 0;
                    keyFieldTile = uc.UnitTileId - 1;
                }
                else
                {
                    supportTile = uc.UnitTileId - 1;
                    leftAttackTile = uc.UnitTileId + 7;
                    centralAttackTile = uc.UnitTileId + 2;
                    rightAttackTile = uc.UnitTileId - 3;
                    leftAttackTileSupport = uc.UnitTileId + 8;
                    centralAttackTileSupport = uc.UnitTileId + 3;
                    rightAttackTileSupport = uc.UnitTileId - 2;
                    if (rightAttackTile <= 5) rightAttackTile = 0;
                    if (rightAttackTileSupport <= 5) rightAttackTileSupport = 0;
                    if (leftAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTile = 0;
                    if (leftAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTileSupport = 0;
                    keyFieldTile = uc.UnitTileId + 1;
                }
            }

            // looks for tiles between unit and its oponent to set key field Id for central attack
            foreach (GameObject t in tiles)
            {
                tc = t.GetComponent<TileController>();
                if (tc.GetKeyFieldId() != 0)
                {
                    myKeyField = new KeyField(tc.GetKeyFieldId(), 0, tc.GetKeyFieldName());
                    myBoardState.AddKeyField(myKeyField);
                }
                if (tc.tileId == keyFieldTile) myKeyFieldId = tc.GetKeyFieldId();
            }

            //looks for units ids which sits on tiles pointed at attack arrows and support units
            foreach (GameObject g2 in units)
            {
                uc2 = g2.GetComponent<UnitController>();
                if (!uc2.isPlaced) continue;
                if (uc2.UnitTileId == supportTile) myUnit.supportLineUnitId = uc2.UnitId; 
                if (uc2.UnitTileId == leftAttackTile)
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttack = null;
                    if(myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(uc.GetAttackId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Landsknechte" || myUnit.GetUnitType() == "Suisse")
                    {
                        tempAttack = new ChargeAttack(uc.GetAttackId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                        tempAttack.ChangeAttack(1);
                        tempAttack.ChangeDefence(-1);
                    }
                    if(tempAttack != null) myUnit.AddAttack(tempAttack);
                }
                if (uc2.UnitTileId == centralAttackTile)
                {
                    if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) // if unit in second line
                    {
                        tempAttack = new ChargeAttack(uc.GetAttackId("central"), false, uc.ArmyId, myUnit, myKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                        if (uc.ArmyId == 1) tc = GetTile(uc.UnitTileId - 1);
                        else tc = GetTile(uc.UnitTileId + 1);
                        tempAttack.ChangeAttack(tc.ChangeAttackStrength(uc.UnitType));
                        myUnit.AddAttack(tempAttack);
                        if (uc.UnitType == "Coustilliers" || uc.UnitType == "Stradioti") // sets far attack for light cavalry
                        {
                            tempAttack = new SkirmishAttack(uc.GetAttackId("far"), true, uc.ArmyId, myUnit, myKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                            myUnit.AddAttack(tempAttack);
                        }
                    }
                    else   // if unit in first line
                    {
                        tempAttack = new ChargeAttack(uc.GetAttackId("central"), true, uc.ArmyId, myUnit, myKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                        tc = GetTile(uc.UnitTileId);
                        tempAttack.ChangeAttack(tc.ChangeAttackStrength(uc.UnitType));
                        myUnit.AddAttack(tempAttack);
                    }
                }
                if (uc2.UnitTileId == rightAttackTile)
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttackId = uc.GetAttackId("right");
                    tempAttack = null;
                    if (myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Landsknechte" || myUnit.GetUnitType() == "Suisse")
                    {
                        tempAttack = new ChargeAttack(tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                        tempAttack.ChangeAttack(1);
                        tempAttack.ChangeDefence(-1);
                    }
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                }
                if (uc2.UnitTileId == leftAttackTileSupport)
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttack = null;
                    if (myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(uc.GetAttackId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Landsknechte" || myUnit.GetUnitType() == "Suisse")
                    {
                        tempAttack = new ChargeAttack(uc.GetAttackId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                        tempAttack.ChangeAttack(1);
                        tempAttack.ChangeDefence(-1);
                    }
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                }
                if (uc2.UnitTileId == centralAttackTileSupport)
                {
                    tc = null;
                    tempAttack = new ChargeAttack(uc.GetAttackId("central"), false, uc.ArmyId, myUnit, myKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                    if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) tc = GetTile(uc.UnitTileId-1);
                    else if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tc = GetTile(uc.UnitTileId + 1);
                    else tc = GetTile(uc.UnitTileId);
                    if (tc != null)
                    {
                        tempAttack.ChangeAttack(tc.ChangeAttackStrength(uc.UnitType));
                        myUnit.AddAdditionalAttack(tempAttack);
                    }
                    if(uc.UnitType == "Coustilliers" || uc.UnitType == "Stradioti")
                    {
                        tempAttack = new SkirmishAttack(uc.GetAttackId("far"), false, uc.ArmyId, myUnit, myKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                        myUnit.AddAdditionalAttack(tempAttack);
                    }
                }
                if (uc2.UnitTileId == rightAttackTileSupport)
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttackId = uc.GetAttackId("right");
                    tempAttack = null;
                    if (myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Landsknechte" || myUnit.GetUnitType() == "Suisse")
                    {
                        tempAttack = new ChargeAttack(tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                        tempAttack.ChangeAttack(1);
                        tempAttack.ChangeDefence(-1);
                    }
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                }

            }
            myBoardState.AddUnit(myUnit);
        }
        //sets attack dependencies
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced) continue;
            myUnit = myBoardState.GetUnit(uc.UnitId);
            //looks for tiles ids which attack arrows point at
            if (uc.ArmyId == 1)
            {
                if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) // if unit in second line
                {
                    leftAttackTile = uc.UnitTileId - 8;
                    centralAttackTile = uc.UnitTileId - 3;
                    rightAttackTile = uc.UnitTileId + 2;
                    leftAttackTileSupport = uc.UnitTileId - 9;
                    centralAttackTileSupport = uc.UnitTileId - 4;
                    rightAttackTileSupport = uc.UnitTileId + 1;
                }
                else
                {
                    leftAttackTile = uc.UnitTileId - 7;
                    centralAttackTile = uc.UnitTileId - 2;
                    rightAttackTile = uc.UnitTileId + 3;
                    leftAttackTileSupport = uc.UnitTileId - 8;
                    centralAttackTileSupport = uc.UnitTileId - 3;
                    rightAttackTileSupport = uc.UnitTileId + 2;
                }
                if (leftAttackTile <= 5) leftAttackTile = 0;
                if (leftAttackTileSupport <= 5) leftAttackTileSupport = 0;
                if (rightAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTile = 0;
                if (rightAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTileSupport = 0;
            }
            else
            {
                if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0)  //  if unit in second line
                {
                    leftAttackTile = uc.UnitTileId + 8;
                    centralAttackTile = uc.UnitTileId + 3;
                    rightAttackTile = uc.UnitTileId - 2;
                    leftAttackTileSupport = uc.UnitTileId + 9;
                    centralAttackTileSupport = uc.UnitTileId + 4;
                    rightAttackTileSupport = uc.UnitTileId - 1;
                }
                else
                {
                    leftAttackTile = uc.UnitTileId + 7;
                    centralAttackTile = uc.UnitTileId + 2;
                    rightAttackTile = uc.UnitTileId - 3;
                    leftAttackTileSupport = uc.UnitTileId + 8;
                    centralAttackTileSupport = uc.UnitTileId + 3;
                    rightAttackTileSupport = uc.UnitTileId - 2;
                }
                if (rightAttackTile <= 5) rightAttackTile = 0;
                if (rightAttackTileSupport <= 5) rightAttackTileSupport = 0;
                if (leftAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTile = 0;
                if (leftAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTileSupport = 0;
            }
            //looks for units ids which sits on tiles pointed at attack arrows
            foreach (GameObject g2 in units)
            {
                uc2 = g2.GetComponent<UnitController>();
                if (!uc2.isPlaced) continue;
                if (uc2.UnitTileId == leftAttackTile)   // adds to central attack, attack  on the left unit, that is activated by this central attack
                {
                    if (uc2.UnitType == "Suisse" || uc2.UnitType == "Landsknechte" || uc2.UnitType == "Arquebusiers")
                    {
                        tempAttack = myUnit.GetAttack(uc.GetAttackId("central"));
                        if (tempAttack != null) tempAttack.AddActivatedAttackId(uc2.GetAttackId("left"));
                    }
                }
                if (uc2.UnitTileId == rightAttackTile) // adds to central attack, attack  on the right unit, that is activated by this central attack
                {
                    if (uc2.UnitType == "Suisse" || uc2.UnitType == "Landsknechte" || uc2.UnitType == "Arquebusiers")
                    {
                        tempAttack = myUnit.GetAttack(uc.GetAttackId("central"));
                        if (tempAttack != null) tempAttack.AddActivatedAttackId(uc2.GetAttackId("right"));
                    }
                }
                if(uc2.UnitTileId == centralAttackTile) // changes attack strength because of tile g2 unit sits on
                {
                    otherUnit = myBoardState.GetUnit(uc2.UnitId);
                    tempAttack = otherUnit.GetAttack(uc2.GetAttackId("central"));
                    if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) tc = GetTile(uc.UnitTileId - 1);
                    else if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tc = GetTile(uc.UnitTileId + 1);
                    else tc = GetTile(uc.UnitTileId);
                    tempAttack.ChangeDefence(tc.ChangeDefenceStrength(uc.UnitType, uc2.UnitType));
                }
                if (uc2.UnitTileId == leftAttackTileSupport)   // adds to central attack, attack  on the left unit, that is activated by this central attack
                {
                    if (uc2.UnitType == "Suisse" || uc2.UnitType == "Landsknechte" || uc2.UnitType == "Arquebusiers")
                    {
                        tempAttack = myUnit.GetAttack(uc.GetAttackId("central"));
                        if (tempAttack != null) tempAttack.AddActivatedAttackId(uc2.GetAttackId("left"));
                    }
                }
                if (uc2.UnitTileId == rightAttackTileSupport) // adds to central attack, attack  on the right unit, that is activated by this central attack
                {
                    if (uc2.UnitType == "Suisse" || uc2.UnitType == "Landsknechte" || uc2.UnitType == "Arquebusiers")
                    {
                        tempAttack = myUnit.GetAttack(uc.GetAttackId("central"));
                        if (tempAttack != null) tempAttack.AddActivatedAttackId(uc2.GetAttackId("right"));
                    }
                }
                if (uc2.UnitTileId == centralAttackTileSupport) // changes attack strength because of tile g2 unit sits on
                {
                    otherUnit = myBoardState.GetUnit(uc2.UnitId);
                    tempAttack = null;
                    tempAttack = otherUnit.GetAttack(uc2.GetAttackId("central"));
                    if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) tc = GetTile(uc.UnitTileId - 1);
                    else if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tc = GetTile(uc.UnitTileId + 1);
                    else tc = GetTile(uc.UnitTileId);
                    if(tempAttack != null) tempAttack.ChangeDefence(tc.ChangeDefenceStrength(uc.UnitType, uc2.UnitType));
                }
            }
        }
        // sets activating attacs for attack arrows
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced) continue;
            ac = uc.GetArrowController("left");
            attackList = myBoardState.GetAttacksActivating(ac.AttackId);
            foreach (int i in attackList)
            {
                ac.AddActivatingAttack(i);
            }
            ac = uc.GetArrowController("right");
            attackList = myBoardState.GetAttacksActivating(ac.AttackId);
            foreach (int i in attackList)
            {
                ac.AddActivatingAttack(i);
            }
        }
        gameMode = "fight";
        EventManager.RaiseEventGameStart();
    }

    // updated in memory state of board
    private void DiceThrown(StateChange result)
    {
        int winnerId;

        winnerId = myBoardState.ChangeState(result);
        if (winnerId != 0) EventManager.RaiseEventGameOver(winnerId);
    }

    private void MakeRouteTest(string closedMode)
    {
        Vector3 testSpot = new Vector3(20.0f, 2.0f, -16.0f);

        if (armyRouteTest == 0 || armyRouteTest < 3 && closedMode == "routtest")
        {
            EventManager.RaiseEventRouteTestOver("noResult", 0, 0);
        }
        else
        {
            if(armyRouteTest == 1 || armyRouteTest == 3 && turnOwnerId == 2 && closedMode != "routtest" || armyRouteTest == 3 && turnOwnerId == 1 && closedMode == "routtest")
            {
                SoundManagerController.Instance.PlayThrowSound(0);
                myCamera.GetComponent<PanZoom>().RoutTest(testSpot + new Vector3(2.0f, 0.0f, 1.0f));
                StartCoroutine(WaitForRouteTest(1, closedMode, "d10-blue"));
            }
            else if(armyRouteTest == 2 || armyRouteTest == 3 && turnOwnerId == 1 && closedMode != "routtest" || armyRouteTest == 3 && turnOwnerId == 2 && closedMode == "routtest")
            {
                SoundManagerController.Instance.PlayThrowSound(0);
                myCamera.GetComponent<PanZoom>().RoutTest(testSpot + new Vector3(2.0f, 0.0f, 1.0f));
                StartCoroutine(WaitForRouteTest(2, closedMode, "d10-yellow"));
            }
        }
        if (armyRouteTest == 3 && closedMode == "routtest") armyRouteTest = 0;
    }

    private IEnumerator WaitForRouteTest(int testingArmyId, string mode, string diceTexture)
    {
        int throwResult, armyMorale, throwId;
        string stringResult = "";
        string resultDescription;
        Vector3 testSpot = new Vector3(20.0f, 2.0f, -16.0f);

        yield return new WaitForSeconds(1.5f);
        throwId = Dice.Roll("3d10", diceTexture, testSpot, new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
        while (Dice.rolling)
        {
            yield return null;
        }
        stringResult = Dice.AsString("d10", throwId);
        armyMorale = myBoardState.GetArmyMorale(testingArmyId);
        if (!stringResult.Contains("?"))
        {
            throwResult = Dice.Value("d10");
            Debug.Log("Wynik testu morale: " + throwResult);
            if (throwResult > armyMorale)
            {
                if (testingArmyId == 1) resultDescription = "frenchFlee";
                else resultDescription = "imperialFlee";
            }
            else
            {
                if (testingArmyId == 1) resultDescription = "frenchStays";
                else resultDescription = "imperialStays";
            }
            Debug.Log(resultDescription);
            yield return new WaitForSeconds(1.5f);
            EventManager.RaiseEventRouteTestOver(resultDescription, throwResult, armyMorale);
            Dice.Clear();
        }
        else
        {
            Debug.Log("Błąd przy route test");
            Dice.Clear();
            MakeRouteTest(mode);
        }
    }

    private void MakeAttack(int idAttack)
    {
        Attack myAttack;
        int throw1 = 0, throw2 = 0;

        myAttack = myBoardState.GetAttack(idAttack);
        if (myAttack.GetArmyId() == turnOwnerId && !hasTurnOwnerAttacked)
        {
            Dice.Clear();
            if (myAttack.GetArmyId() == 1)
            {
                if (myAttack.GetAttackDiceNumber() > 0) throw1 = Dice.Roll(myAttack.GetAttackDiceNumber().ToString() + "d6", "d6-attackblue", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -1.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
                if (myAttack.GetDefenceDiceNumber() > 0) throw2 = Dice.Roll(myAttack.GetDefenceDiceNumber().ToString() + "d6", "d6-defenceyellow", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -2.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
            }
            else
            {
                if (myAttack.GetAttackDiceNumber() > 0) throw1 = Dice.Roll(myAttack.GetAttackDiceNumber().ToString() + "d6", "d6-attackyellow", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -2.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
                if (myAttack.GetDefenceDiceNumber() > 0) throw2 = Dice.Roll(myAttack.GetDefenceDiceNumber().ToString() + "d6", "d6-defenceblue", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -1.0f), new Vector3(2.0f, 5.5f + Random.value * 0.5f, 0.0f));
            }
            StartCoroutine(WaitForDice(throw1, throw2, idAttack));
        }
    }

    // Waits for dice to stop rolling
    private IEnumerator WaitForDice(int throw1Id, int throw2Id, int attackId)
    {
        Attack tempAttack;
        string result1 = "", result2 = "";
        bool isSpecialOutcome;
        int attackerArmyId, defenderArmyId;

        while (Dice.rolling)
        {
            yield return null;
        }
        if(throw1Id > 0) result1 = Dice.AsString("d6", throw1Id);
        if(throw2Id > 0) result2 = Dice.AsString("d6", throw2Id);
        Debug.Log(result1);
        Debug.Log(result2);
        StateChange result = new StateChange();
        string[] throw1Hits, throw2Hits;
        int attackStrengthHit=0, attackMoraleHit=0, defenceStrengthHit=0, defenceMoraleHit=0;
        if (!(result1.Contains("?") || result2.Contains("?") || result1.Length < 13 && result1.Length > 0 || result2.Length < 12 && result2.Length > 0))    // sprawdzenie czy rzut był udany/bezbłędny
        {
            if (throw1Id > 0) throw1Hits = Dice.ResultForThrow("d6", throw1Id);
            else throw1Hits = new string[0];
            if (throw2Id > 0) throw2Hits = Dice.ResultForThrow("d6", throw2Id);
            else throw2Hits = new string[0];
            if (throw1Hits != null && throw2Hits != null)
            {
                isSpecialOutcome = false;
                tempAttack = myBoardState.GetAttack(attackId);
                for (int i = 0; i < throw1Hits.Length; i++)
                {
                    if (throw1Hits[i] == "S") attackStrengthHit++;
                    if (throw1Hits[i] == "M") attackMoraleHit++;
                    if (throw1Hits[i] == "*") isSpecialOutcome = true;
                }
                for (int i = 0; i < throw2Hits.Length; i++)
                {
                    if (throw2Hits[i] == "S") defenceStrengthHit++;
                    if (throw2Hits[i] == "M") defenceMoraleHit++;
                }
                result.attackerId = tempAttack.GetOwner().GetUnitId();
                result.defenderId = tempAttack.GetTargetId();
                result.attackerMoraleChanged = -defenceMoraleHit;
                result.attackerStrengthChange = -defenceStrengthHit;
                result.defenderMoraleChanged = -attackMoraleHit;
                result.defenderStrengthChange = -attackStrengthHit;
                if(isSpecialOutcome) tempAttack.SpecialOutcome(ref result);
                result.activatedAttacks = new List<int>(tempAttack.GetActivatedAttacks().ToArray());
                result.deactivatedAttacks = new List<int>(tempAttack.GetDeactivatedAttacks().ToArray());
                Debug.Log("Attack inflicted " + attackStrengthHit + " strength casualty and " + attackMoraleHit + " morale loss for defender.");
                Debug.Log("Defence inflicted " + defenceStrengthHit + " strength casualty and " + defenceMoraleHit + " morale loss for attacker.");
                hasTurnOwnerAttacked = true;
                //check if route test needed
                armyRouteTest = 0;
                attackerArmyId = turnOwnerId;
                if (turnOwnerId == 1)
                {
                    defenderArmyId = 2;
                }
                else defenderArmyId = 1;
                if (result.attackerStrengthChange < 0 
                    && result.defenderStrengthChange < 0 
                    && myBoardState.GetArmy(attackerArmyId).GetMorale() <= 30
                    && myBoardState.GetArmy(defenderArmyId).GetMorale() <= 30) armyRouteTest = 3;
                else if (result.attackerStrengthChange < 0 && myBoardState.GetArmy(attackerArmyId).GetMorale() <= 30) armyRouteTest = attackerArmyId;
                else if (result.defenderStrengthChange < 0 && myBoardState.GetArmy(defenderArmyId).GetMorale() <= 30) armyRouteTest = defenderArmyId;
                yield return new WaitForSeconds(1.5f);
                EventManager.RaiseEventOnDiceResult(result);
                Dice.Clear();
            }
        }
        else
        {
            Debug.Log("Błąd przy rzucie");
            MakeAttack(attackId);
        }
    }

    private void TurnStart()
    {
        hasTurnOwnerAttacked = false;
        if (turnOwnerId == 1) turnOwnerId = 2;
        else turnOwnerId = 1;
        if (turnOwnerId == 1 && !GameManagerController.Instance.isPlayer1Human || turnOwnerId == 2 && !GameManagerController.Instance.isPlayer2Human)
        {
            ComputeBestAttack();
        }
    }

    private float ExpectiMinMaxBoardState(BoardState inBoardState, float limit, int armyId)
    {
        float score = 0;
        float maxScore = -1000.0f;
        int otherArmyId, possibleOutcomesCount;

        if (armyId == 1) otherArmyId = 2;
        else otherArmyId = 1;
        List<int> avialableAttacks = new List<int>();
        if (limit < 1.0) return inBoardState.EvaluateBoard(otherArmyId);
        avialableAttacks = inBoardState.GetPossibleAttacks(otherArmyId);
        if (avialableAttacks.Count == 0) return -500.0f;
        else
        {
            possibleOutcomesCount = 0;
            foreach (int i in avialableAttacks)
            {
                possibleOutcomesCount += inBoardState.GetPossibleOutcomes(otherArmyId).Count;
            }
            foreach (int i in avialableAttacks)
            {
                score = -ExpectiMinMaxAttack(inBoardState, i, limit / possibleOutcomesCount, otherArmyId);
                if (score > maxScore)
                {
                    maxScore = score;
                }
            }
            return maxScore;
        }
    }

    private float ExpectiMinMaxAttack(BoardState inBoardState, int attackId, float limit, int armyId)
    {
        float attackValue = 0, attackerRoutProbability, defenderRoutProbability;
        Attack myAttack;
        BoardState outBoardState;
        int otherArmyId;
        if (armyId == 1) otherArmyId = 2;
        else otherArmyId = 1;
        attackerRoutProbability = 0;
        defenderRoutProbability = 0;
        myAttack = inBoardState.GetAttack(attackId);
        foreach (StateChange sc in myAttack.GetOutcomes())
        {
            outBoardState = new BoardState(inBoardState);
            outBoardState.ChangeState(sc);
            if(sc.defenderStrengthChange != 0 && outBoardState.GetArmyMorale(otherArmyId) < 30)  //defender looses rout test
            {
                defenderRoutProbability = routProbabilityTable[outBoardState.GetArmyMorale(otherArmyId)];
                 attackValue += -500.0f * defenderRoutProbability * sc.changeProbability;
            }
            if(sc.attackerStrengthChange != 0 && outBoardState.GetArmyMorale(armyId) < 30) // attacker looses from test
            {
                attackerRoutProbability = routProbabilityTable[outBoardState.GetArmyMorale(armyId)];
                attackValue += 500.0f * attackerRoutProbability * (1 - defenderRoutProbability) * sc.changeProbability;
            }
            attackValue += sc.changeProbability * (1 - defenderRoutProbability) * (1 - attackerRoutProbability) * ExpectiMinMaxBoardState(outBoardState, limit, armyId);
        }
        return attackValue;
    }

    private void ComputeBestAttack()
    {
        List<int> avialableAttacks = new List<int>();
        int bestAttack = 0;
        float score, maxScore, minimaxLimit;

        maxScore = -1000.0f;
        switch(GameManagerController.Instance.difficultyLevel)
        {
            case GameManagerController.diffLevelEnum.easy:
                minimaxLimit = 10.0f;
                break;
            case GameManagerController.diffLevelEnum.medium:
                minimaxLimit = 20.0f;
                break;
            case GameManagerController.diffLevelEnum.hard:
                minimaxLimit = 30.0f;
                break;
            default:
                minimaxLimit = 20.0f;
                break;
        }
        avialableAttacks = myBoardState.GetPossibleAttacks(turnOwnerId);
        if (avialableAttacks.Count > 0)
        {
            foreach (int i in avialableAttacks)
            {
                score = -ExpectiMinMaxAttack(myBoardState, i, minimaxLimit, turnOwnerId);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestAttack = i;
                }
            }
            if (bestAttack > 0)
            {
                EventManager.RaiseEventOnUnitClicked(myBoardState.GetAttack(bestAttack).GetOwner().GetUnitId());
                EventManager.RaiseEventOnAttackClicked(bestAttack, false);
                EventManager.RaiseEventOnAttackOrdered(bestAttack);
                SoundManagerController.Instance.PlayThrowSound(0);
            }
        }
    }

    public TileController GetTile(int tId)
    {
        TileController tempTileController;
        foreach(GameObject g in tiles)
        {
            tempTileController = g.GetComponent<TileController>();
            if (tempTileController.tileId == tId) return tempTileController;
        }
        return null;
    }

    public Unit GetUnit(int unitId)
    {
        return myBoardState.GetUnit(unitId);
    }

    public Attack GetAttack(int aId)
    {
        return myBoardState.GetAttack(aId);
    }

    public int GetArmyMorale(int armyId)
    {
        return myBoardState.GetArmyMorale(armyId);
    }

    public string GetArmyName(string army, Attack a)
    {
        int i = 0;

        if (army == "attacker")
        {
            i = a.GetOwner().GetArmyId();
        }
        else if(army == "defender")
        {
            i = myBoardState.GetUnit(a.GetTargetId()).GetArmyId();
        }
        if (i > 0) return myBoardState.GetArmyName(i);
        else return null;
    }

    public string GetArmyName(int unitId)
    {
        return myBoardState.GetArmyName(myBoardState.GetUnit(unitId).GetArmyId());
    }

    public string GetKeyFieldName(int keyFieldId)
    {
        return myBoardState.GetKeyFieldName(keyFieldId);
    }

    public void RemoveUnitController(GameObject u)
    {
        units.Remove(u);
    }

    public bool isUnitControllerBlocked(int uId)
    {
        UnitController uc;
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (uc.UnitId == uId) return uc.isBlocked;
        }
        return false;
    }
}
