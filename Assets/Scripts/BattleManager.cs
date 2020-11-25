using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private BoardState afterDeploymentBoardState;
    private List<GameObject> units;
    private List<GameObject> tiles;
    private Camera myCamera;
    private float[] routProbabilityTable;
    private int armyRouteTest;

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
    [SerializeField] private GameObject frenchArquebusiersPrefab2;
    [SerializeField] private GameObject frenchArtilleryPrefab;
    [SerializeField] private GameObject frenchCoustilliersPrefab;
    [SerializeField] private GameObject suissePrefab;
    [SerializeField] private GameObject imperialLandsknechtePrefab;
    [SerializeField] private GameObject imperialLandsknechtePrefab2;
    [SerializeField] private GameObject garrisonPrefab;
    [SerializeField] private GameObject imperialCavaleryPrefab;
    [SerializeField] private GameObject imperialArquebusiersPrefab;
    [SerializeField] private GameObject imperialArtilleryPrefab;
    [SerializeField] private GameObject imperialStradiotiPrefab;


    public int boardWidth;
    public int boardHeight;
    public float boardFieldWitdth = 4.0f;
    public float boardFieldHeight = 4.0f;
    public int maxSquads = 12;
    public int turnOwnerId = 1;
    public bool hasTurnOwnerAttacked = false;
    public bool isInputBlocked = false;
    public string gameMode = "deploy";
    public bool ignoreRoutTest;
    
    public const string Army1Color = "#ecc333";
    public const string Army2Color = "#4158f3";

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
        ignoreRoutTest = false;
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
                    if (j == 4 && (r < 3) && i > 3 && i < _boardWidth + 1) tempKeyFieldId = ++keyFieldCounter;
                    else tempKeyFieldId = 0;
                    possibleArmyDeployment = 0;
                    if (j == 3 && i > 3 && i < _boardWidth+1) possibleArmyDeployment = 2;
                    if(j == 5 && i > 3 && i < _boardWidth+1) possibleArmyDeployment = 1;
                    switch (r)
                    {
                        case 0:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "Forest", possibleArmyDeployment);
                            break;
                        case 1:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "Hill", possibleArmyDeployment);
                            break;
                        case 2:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "Town", possibleArmyDeployment);
                            break;
                        default:
                            tempObj.GetComponent<TileController>().InitializeTile(++tileCounter, tempKeyFieldId, "Field", possibleArmyDeployment);
                            break;
                    }
                    tiles.Add(tempObj);
                }
            }
        }
    }

    private void ComputeDeployment(int armyId)
    {
        int tileUnitValue, tempTileId, tempUnitId;
        List<TileController> possibleTiles;
        List<UnitController> possibleUnits;
        List<UnitPlacementHelp> placementList;  // list of possible pairs unit - tile
        List<UnitPlacementHelp> placementToRemove;
        TileController tc, oppositeTile;
        UnitController uc;
        UnitPlacementHelp tempPlacementHelp;

        possibleTiles = new List<TileController>();
        possibleUnits = new List<UnitController>();
        placementList = new List<UnitPlacementHelp>();
        foreach (GameObject t in tiles) // looks for tiles with possible placement
        {
            tc = t.GetComponent<TileController>();
            if (tc.DeploymentPossible(armyId)) possibleTiles.Add(tc); //checks if deployment possible for army aId on this tile and adds it to list of possible tiles
        }
        foreach (GameObject g in units) // creates list of every possible pair tile - unit
        {
            uc = g.GetComponent<UnitController>();
            possibleUnits.Add(uc);
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
                        if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) oppositeTile = GetTile(tc2.tileId + 3);
                        else oppositeTile = GetTile(tc2.tileId + 2);
                    }
                    tileUnitValue += oppositeTile.GetOpposingUnitValue(uc.UnitType);
                    tempPlacementHelp = new UnitPlacementHelp(uc.UnitId, tc2.tileId, tileUnitValue);
                    placementList.Add(tempPlacementHelp);
                }
            }
        }
        placementList.Sort((p1,p2)=>p1.points.CompareTo(p2.points));    // sorts list of possible pairs unit - tile by value
        while (possibleUnits.Count > 0 && possibleTiles.Count > 0) // do until all units are placed
        {
            EventManager.RaiseEventOnUnitDeployed(placementList[0].unitId, placementList[0].tileId); // place an unit
            possibleTiles.Remove(GetTile(placementList[0].tileId));
            possibleUnits.Remove(GetUnitController(placementList[0].unitId));
            tempTileId = placementList[0].tileId;
            tempUnitId = placementList[0].unitId;
            placementToRemove = new List<UnitPlacementHelp>();
            foreach (UnitPlacementHelp uph in placementList)    // looks for elements to remove and stores them in separate list
            {
                if (uph.tileId == tempTileId || uph.unitId == tempUnitId) placementToRemove.Add(uph);
            }
            foreach(UnitPlacementHelp uph2 in placementToRemove)    // removes from list of possible pair tile - unit
            {
                placementList.Remove(uph2);
            }
        }
        if(possibleUnits.Count > 0)
        {
            possibleTiles.Clear();
            placementList.Clear();
            foreach (GameObject t in tiles)
            {
                tc = t.GetComponent<TileController>();
                if (tc.DeploymentPossible(armyId) && !tc.IsTileOccupied()) possibleTiles.Add(tc); //checks if deployment possible for army aId on this tile and adds it to list of possible tiles
            }
            foreach (GameObject g in units) // creates list of every possible pair tile - unit
            {
                uc = g.GetComponent<UnitController>();
                if (uc.ArmyId == armyId && !uc.isPlaced)
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
                            if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) oppositeTile = GetTile(tc2.tileId + 3);
                            else oppositeTile = GetTile(tc2.tileId + 2);
                        }
                        tileUnitValue += oppositeTile.GetOpposingUnitValue(uc.UnitType);
                        tempPlacementHelp = new UnitPlacementHelp(uc.UnitId, tc2.tileId, tileUnitValue);
                        placementList.Add(tempPlacementHelp);
                    }
                }
            }
            placementList.Sort((p1, p2) => p1.points.CompareTo(p2.points));    // sorts list of possible pairs unit - tile by value
            while (placementList.Count > 0 && possibleTiles.Count > 0) // do until all units are placed
            {
                EventManager.RaiseEventOnUnitDeployed(placementList[0].unitId, placementList[0].tileId); // place an unit
                possibleTiles.Remove(GetTile(placementList[0].tileId));
                tempTileId = placementList[0].tileId;
                tempUnitId = placementList[0].unitId;
                placementToRemove = new List<UnitPlacementHelp>();
                foreach (UnitPlacementHelp uph in placementList)    // looks for elements to remove and stores them in separate list
                {
                    if (uph.tileId == tempTileId || uph.unitId == tempUnitId) placementToRemove.Add(uph);
                }
                foreach (UnitPlacementHelp uph2 in placementToRemove)    // removes from list of possible pair tile - unit
                {
                    placementList.Remove(uph2);
                }
            }
        }
        EventManager.RaiseEventOnDeploymentStart(armyId + 1);
    }

    private void DeployArmy(int armyId)
    {
        GameObject tempObj;
        UnitController uc;

        if(armyId < 3) BattleManager.Instance.turnOwnerId = armyId;
        if (armyId == 1)
        {
            tempObj = Instantiate(imperialCavaleryPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            uc = tempObj.GetComponent<UnitController>();
            uc.InitializeUnit(8, 1, 1, 0, "de Lannoy");
            uc.HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialArquebusiersPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            uc = tempObj.GetComponent<UnitController>();
            uc.InitializeUnit(9, 1, 1, 1, "de Vasto");
            uc.HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(imperialLandsknechtePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            uc = tempObj.GetComponent<UnitController>();
            uc.InitializeUnit(7, 1, 1, 2, "von Frundsberg");
            uc.HideAll();
            units.Add(tempObj);

            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                tempObj = Instantiate(imperialArtilleryPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(10, 1, 1, 4, "");
                uc.HideAll();
                units.Add(tempObj);

                tempObj = Instantiate(imperialStradiotiPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(11, 1, 1, 5, "");
                uc.HideAll();
                units.Add(tempObj);

                tempObj = Instantiate(imperialLandsknechtePrefab2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(12, 1, 1, 3, "Pescara");
                uc.HideAll();
                units.Add(tempObj);
            }
            if (!GameManagerController.Instance.isPlayer1Human) // places units on board
            {
                ComputeDeployment(armyId);
            }
        }
        else if(armyId == 2)
        {
            tempObj = Instantiate(gendarmesPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            uc = tempObj.GetComponent<UnitController>();
            uc.InitializeUnit(1, 2, 1, 0, "Francis I");
            uc.HideAll();
            units.Add(tempObj);

            tempObj = Instantiate(frenchArquebusiersPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            uc = tempObj.GetComponent<UnitController>();
            uc.InitializeUnit(4, 2, 1, 1, "de la Pole");
            uc.HideAll();
            units.Add(tempObj);

            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                tempObj = Instantiate(frenchLandsknechtePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(2, 2, 1, 3, "de Lorraine");
                uc.HideAll();
                units.Add(tempObj);

                tempObj = Instantiate(suissePrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(3, 2, 1, 2, "de La Marck");
                uc.HideAll();
                units.Add(tempObj);

                tempObj = Instantiate(frenchArtilleryPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(5, 2, 1, 4, "de Genouillac");
                uc.HideAll();
                units.Add(tempObj);

                tempObj = Instantiate(frenchCoustilliersPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(6, 2, 1, 5, "Tiercelin");
                uc.HideAll();
                units.Add(tempObj);
            }

            if (!GameManagerController.Instance.isPlayer2Human)
            {
                ComputeDeployment(armyId);
            }
        }
        else if(armyId == 3)
        {
            if (SceneManager.GetActiveScene().name != "Tutorial")
            {
                tempObj = Instantiate(garrisonPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(13, 1, 8, -1, "de Leyva");
                uc.isPlaced = true;
                units.Add(tempObj);
                tempObj = Instantiate(frenchArquebusiersPrefab2, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                uc = tempObj.GetComponent<UnitController>();
                uc.InitializeUnit(14, 2, 6, -1, "d'Alencon");
                uc.isPlaced = true;
                units.Add(tempObj);
            }
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
        Attack tempAttack, tempAdditionalAttack;
        List<Attack> resultAttacks;
        Vector3 arrowRightPositionShift, arrowCentralPositionShift, arrowLeftPositionShift;
        int army1morale = 0, army2morale = 0, leftAttackTile, centralAttackTile, rightAttackTile, centralKeyFieldTile, leftKeyFieldTile, rightKeyFieldTile, centralKeyFieldId, leftKeyFieldId, rightKeyFieldId, tempAttackId, leftAttackTileSupport, centralAttackTileSupport, rightAttackTileSupport, supportTile, attackIdCounter, tempTileId;
        List<int> attackList;
        bool isTileOccupied;

        // counts morale of army
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced) continue;
            if (uc.ArmyId == 1) army1morale += uc.InitialMorale;
            else army2morale += uc.InitialMorale;
        }
        army1 = new Army(1, army1morale, 0, 6, "HRE");
        army2 = new Army(2, army2morale, 0, 6, "France");
        myBoardState = new BoardState(army1, army2);

        // creates in memory representation of each unit on screen
        attackIdCounter = 1;
        isTileOccupied = false;
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced) continue;
            myUnit = new Unit(uc.UnitId, uc.UnitType, uc.InitialStrength, uc.InitialMorale, uc.ArmyId == 1 ? army1 : army2, uc.UnitCommander, uc.UnitTileId);
            supportTile = 0;
            centralKeyFieldId = 0;
            leftKeyFieldId = 0;
            rightKeyFieldId = 0;
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
                myUnit.SetUnitInSupportLine();
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
                    if (leftAttackTile <= 6) leftAttackTile = 0;
                    if (leftAttackTileSupport <= 6) leftAttackTileSupport = 0;
                    if (rightAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTile = 0;
                    if (rightAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTileSupport = 0;
                    centralKeyFieldTile = uc.UnitTileId - 2;
                    leftKeyFieldTile = uc.UnitTileId - 2 - BattleManager.Instance.boardHeight;
                    rightKeyFieldTile = uc.UnitTileId - 2 + BattleManager.Instance.boardHeight;
                }
                else
                {
                    if (uc.UnitTileId == 6)
                    {
                        myUnit.SetUnitOutsideOfBattlefield();
                        tempTileId = 11;
                    }
                    else tempTileId = uc.UnitTileId;
                    if (uc.UnitTileId == 11) supportTile = 6;
                    uc.SetArrowsBlockValue(true);
                    leftAttackTile = tempTileId + 8;
                    centralAttackTile = tempTileId + 3;
                    rightAttackTile = tempTileId - 2;
                    leftAttackTileSupport = tempTileId + 9;
                    centralAttackTileSupport = tempTileId + 4;
                    rightAttackTileSupport = tempTileId - 1;
                    if (rightAttackTile <= 6) rightAttackTile = 0;
                    if (rightAttackTileSupport <= 6) rightAttackTileSupport = 0;
                    if (leftAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTile = 0;
                    if (leftAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTileSupport = 0;
                    centralKeyFieldTile = tempTileId + 2;
                    leftKeyFieldTile = tempTileId + 2 + BattleManager.Instance.boardHeight;
                    rightKeyFieldTile = tempTileId + 2 - BattleManager.Instance.boardHeight;
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
                    if (leftAttackTile <= 6) leftAttackTile = 0;
                    if (leftAttackTileSupport <= 6) leftAttackTileSupport = 0;
                    if (rightAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTile = 0;
                    if (rightAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTileSupport = 0;
                    centralKeyFieldTile = uc.UnitTileId - 1;
                    leftKeyFieldTile = uc.UnitTileId - 1 - BattleManager.Instance.boardHeight;
                    rightKeyFieldTile = uc.UnitTileId - 1 + BattleManager.Instance.boardHeight;
                }
                else
                {
                    if(uc.UnitTileId == 12)
                    {
                        foreach (GameObject g2 in units)
                        {
                            uc2 = g2.GetComponent<UnitController>();
                            if (uc2.UnitTileId == 11) isTileOccupied = true;
                        }
                        if (!isTileOccupied) supportTile = 6;
                        else supportTile = 11;
                    }
                    else supportTile = uc.UnitTileId - 1;
                    leftAttackTile = uc.UnitTileId + 7;
                    centralAttackTile = uc.UnitTileId + 2;
                    rightAttackTile = uc.UnitTileId - 3;
                    leftAttackTileSupport = uc.UnitTileId + 8;
                    centralAttackTileSupport = uc.UnitTileId + 3;
                    rightAttackTileSupport = uc.UnitTileId - 2;
                    if (rightAttackTile <= 6) rightAttackTile = 0;
                    if (rightAttackTileSupport <= 6) rightAttackTileSupport = 0;
                    if (leftAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTile = 0;
                    if (leftAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTileSupport = 0;
                    centralKeyFieldTile = uc.UnitTileId + 1;
                    leftKeyFieldTile = uc.UnitTileId + 1 + BattleManager.Instance.boardHeight;
                    rightKeyFieldTile = uc.UnitTileId + 1 - BattleManager.Instance.boardHeight;
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
                if (tc.tileId == centralKeyFieldTile) centralKeyFieldId = tc.GetKeyFieldId();
                if(tc.tileId == leftKeyFieldTile) leftKeyFieldId = tc.GetKeyFieldId();
                if (tc.tileId == rightKeyFieldTile) rightKeyFieldId = tc.GetKeyFieldId();
            }
            // initialize french rear guard
            if (uc.UnitTileId == 6)
            {
                tempAttack = new MoveAttack(attackIdCounter++, uc.GetArrowId("side"), !isTileOccupied, uc.ArmyId, myUnit, 0, false, 0, uc.transform.position + arrowLeftPositionShift);
                if (tempAttack != null)
                {
                    tempAttack.AddDeactivatedAttackId(tempAttack.GetId());
                    myUnit.AddAttack(tempAttack);
                }
            }

            //looks for units ids which sits on tiles pointed at attack arrows and support units
            foreach (GameObject g2 in units)
            {
                uc2 = g2.GetComponent<UnitController>();
                if (!uc2.isPlaced) continue;
                //initialize garrison attack
                if(uc.UnitTileId == 8 && uc2.UnitTileId == 12)  // if garrison and other unit on garrisons target tile
                {
                    tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("central"), true, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, false, true);
                    myUnit.AddAttack(tempAttack);
                }
                if ((uc.UnitTileId == 12 || uc.UnitTileId == 11 || uc.UnitTileId == 6) && uc2.UnitTileId == 8)
                {
                    tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("right"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType, true, false);
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                }
                if (uc2.UnitTileId == supportTile) myUnit.supportLineUnitId = uc2.UnitId; 
                if (uc2.UnitTileId == leftAttackTile)   // set attack on left tile in first enemy line
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttack = null;
                    if(myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Artillery")
                    {
                        tempAttack = new BombardAttack(attackIdCounter++, uc.GetArrowId("left"), true, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType, false);
                        if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tempAttack.Deactivate();   // deactivate if unit in second line
                    }
                    if((myUnit.GetUnitType() == "Coustilliers" || myUnit.GetUnitType() == "Stradioti") && leftKeyFieldId !=0 ) tempAttack = new CaptureAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, leftKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                    tempAttack = null;
                    if (myUnit.GetUnitType() != "Artillery") tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType, true, false);
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                }
                if (uc2.UnitTileId == centralAttackTile) // set attack on central tile in first enemy line
                {
                    if (uc.UnitTileId == 6 && uc2.UnitTileId == 8) continue; // avoid rear guard attack on garrison
                    if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) // if attacking unit in second line
                    {
                        if (myUnit.GetUnitType() == "Artillery") tempAttack = new BombardAttack(attackIdCounter++, uc.GetArrowId("central"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, true);
                        else tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("central"), false, uc.ArmyId, myUnit, centralKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, false, true);
                        if (uc.ArmyId == 1) tc = GetTile(uc.UnitTileId - 1);
                        else tc = GetTile(uc.UnitTileId + 1);
                        tempAttack.ChangeAttack(tc.ChangeAttackStrength(uc.UnitType));
                        myUnit.AddAttack(tempAttack);
                        if (uc.UnitType == "Coustilliers" || uc.UnitType == "Stradioti") // sets far attack for light cavalry
                        {
                            tempAttack = new SkirmishAttack(attackIdCounter++, uc.GetArrowId("far"), true, uc.ArmyId, myUnit, centralKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                            myUnit.AddAttack(tempAttack);
                        }
                    }
                    else   // if attacking unit in first line
                    {
                        if (myUnit.GetUnitType() == "Artillery") tempAttack = new BombardAttack(attackIdCounter++, uc.GetArrowId("central"), true, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, true);
                        else tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("central"), true, uc.ArmyId, myUnit, centralKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, false, true);
                        tc = GetTile(uc.UnitTileId);
                        tempAttack.ChangeAttack(tc.ChangeAttackStrength(uc.UnitType));
                        if ((uc.UnitTileId != 8 || uc2.UnitTileId != 6) && (uc.UnitTileId != 6 || uc2.UnitTileId != 8)) myUnit.AddAttack(tempAttack);
                    }
                }
                if (uc2.UnitTileId == rightAttackTile) // set attack on right tile in first enemy line
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttackId = uc.GetArrowId("right");
                    tempAttack = null;
                    if (myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(attackIdCounter++, tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Artillery")
                    {
                        tempAttack = new BombardAttack(attackIdCounter++, tempAttackId, true, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType, false);
                        if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tempAttack.Deactivate();    // deactivate if unit in second line
                    }
                    if ((myUnit.GetUnitType() == "Coustilliers" || myUnit.GetUnitType() == "Stradioti") && rightKeyFieldId != 0) tempAttack = new CaptureAttack(attackIdCounter++, uc.GetArrowId("right"), false, uc.ArmyId, myUnit, rightKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                    tempAttack = null;
                    if (myUnit.GetUnitType() != "Artillery") tempAttack = new ChargeAttack(attackIdCounter++, tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType, true, false);
                    if (tempAttack != null) myUnit.AddAttack(tempAttack);
                }
                //initialize garrison additional attack
                if (uc.UnitTileId == 8 && (uc2.UnitTileId == 11 || uc2.UnitTileId == 6))  // if garrison and other unit on garrisons target tile
                {
                    tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("central"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, false, true);
                    myUnit.AddAdditionalAttack(tempAttack);
                }
                if (uc2.UnitTileId == leftAttackTileSupport || ((uc.UnitTileId == 19 || uc.UnitTileId == 20) && uc2.UnitTileId == 6)) // set attack on left tile in second enemy line
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttack = null;
                    if (myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Artillery") tempAttack = new BombardAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType,false);
                    if ((myUnit.GetUnitType() == "Coustilliers" || myUnit.GetUnitType() == "Stradioti") && leftKeyFieldId != 0) tempAttack = new CaptureAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, leftKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType);
                    if (tempAttack != null) myUnit.AddAdditionalAttack(tempAttack);
                    tempAttack = null;
                    if (myUnit.GetUnitType() != "Artillery") tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("left"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowLeftPositionShift, uc.UnitType, uc2.UnitType, true, false);
                    if (tempAttack != null) myUnit.AddAdditionalAttack(tempAttack);
                }
                if (uc2.UnitTileId == centralAttackTileSupport || ((uc.UnitTileId == 14 || uc.UnitTileId == 15) && uc2.UnitTileId == 6)) // set attack on central tile in second enemy line
                {
                    tc = null;
                    if (myUnit.GetUnitType() == "Artillery") tempAttack = new BombardAttack(attackIdCounter++, uc.GetArrowId("central"), false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, true);
                    else tempAttack = new ChargeAttack(attackIdCounter++, uc.GetArrowId("central"), false, uc.ArmyId, myUnit, centralKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType, false, true);
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
                        tempAttack = new SkirmishAttack(attackIdCounter++, uc.GetArrowId("far"), false, uc.ArmyId, myUnit, centralKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowCentralPositionShift, uc.UnitType, uc2.UnitType);
                        myUnit.AddAdditionalAttack(tempAttack);
                    }
                }
                if (uc2.UnitTileId == rightAttackTileSupport) // set attack on right tile in second enemy line
                {
                    // na podstawie typu jednostki wybrać rodzaj ataku
                    tempAttackId = uc.GetArrowId("right");
                    tempAttack = null;
                    if (myUnit.GetUnitType() == "Arquebusiers") tempAttack = new CounterAttack(attackIdCounter++, tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                    if (myUnit.GetUnitType() == "Artilllery") tempAttack = new BombardAttack(attackIdCounter++, tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType, false);
                    if ((myUnit.GetUnitType() == "Coustilliers" || myUnit.GetUnitType() == "Stradioti") && rightKeyFieldId != 0) tempAttack = new CaptureAttack(attackIdCounter++, uc.GetArrowId("right"), false, uc.ArmyId, myUnit, rightKeyFieldId, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType);
                    if (tempAttack != null) myUnit.AddAdditionalAttack(tempAttack);
                    tempAttack = null;
                    if (myUnit.GetUnitType() != "Artillery") tempAttack = new ChargeAttack(attackIdCounter++, tempAttackId, false, uc.ArmyId, myUnit, 0, false, uc2.UnitId, uc.transform.position + arrowRightPositionShift, uc.UnitType, uc2.UnitType, true, false);
                    if (tempAttack != null) myUnit.AddAdditionalAttack(tempAttack);
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
            if (uc.UnitType == "Landsknechte" || uc.UnitType == "Suisse")   //set own deactivated attacks for Landsknechts charge
            {
                myUnit = myBoardState.GetUnit(uc.UnitId);
                myUnit.SetOwnAttacksDeactivation();
            }
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
                if (leftAttackTile <= 6) leftAttackTile = 0;
                if (leftAttackTileSupport <= 6) leftAttackTileSupport = 0;
                if (rightAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTile = 0;
                if (rightAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) rightAttackTileSupport = 0;
            }
            else
            {
                if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0)  //  if unit in second line
                {
                    if (uc.UnitTileId == 6) tempTileId = 11;
                    else tempTileId = uc.UnitTileId;
                    leftAttackTile = tempTileId + 8;
                    centralAttackTile = tempTileId + 3;
                    rightAttackTile = tempTileId - 2;
                    leftAttackTileSupport = tempTileId + 9;
                    centralAttackTileSupport = tempTileId + 4;
                    rightAttackTileSupport = tempTileId - 1;
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
                if (rightAttackTile <= 6) rightAttackTile = 0;
                if (rightAttackTileSupport <= 6) rightAttackTileSupport = 0;
                if (leftAttackTile > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTile = 0;
                if (leftAttackTileSupport > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - BattleManager.Instance.boardHeight) leftAttackTileSupport = 0;
            }
            //looks for units ids which sits on tiles pointed at attack arrows
            foreach (GameObject g2 in units)
            {
                uc2 = g2.GetComponent<UnitController>();
                if (!uc2.isPlaced) continue;
                //initialize garrison dependencies
                if (uc.UnitTileId == 8 && (uc2.UnitTileId == 12 || uc2.UnitTileId == 11 || uc2.UnitTileId == 6) && uc2.UnitType != "Artillery")
                {
                    if(uc2.UnitTileId == 12) resultAttacks = myUnit.GetAttacksByArrowId(uc.GetArrowId("central"));
                    else resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("central"));
                    if (resultAttacks.Count > 0) 
                    {
                        tempAttack = null;
                        foreach (Attack a in resultAttacks)
                        {
                            if (a.GetTargetId() == uc2.UnitId) tempAttack = a;
                        }
                        otherUnit = myBoardState.GetUnit(uc2.UnitId);
                        resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                        foreach (Attack a in resultAttacks)
                        {
                            if (tempAttack != null)
                            {
                                if (a.GetName() == "Charge!") tempAttack.AddActivatedAttackId(a.GetId());
                                else
                                {
                                    tempAttack.AddDeactivatedAttackId(a.GetId());
                                    tempAttack.AddBlockedAttackId(a.GetId());
                                }
                            }
                        }
                        resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("central"));
                        foreach (Attack a in resultAttacks)
                        {
                            if (tempAttack != null)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                        }
                        resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("left"));
                        foreach (Attack a in resultAttacks)
                        {
                            if (tempAttack != null)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                        }
                    }
                }
                if (uc2.UnitTileId == leftAttackTile || uc2.UnitTileId == leftAttackTileSupport)   // adds to central attack, attack  on the left unit, that is activated by this central attack
                {
                    otherUnit = myBoardState.GetUnit(uc2.UnitId);
                    if (uc2.UnitType == "Arquebusiers" || uc2.UnitType == "Coustilliers" || uc2.UnitType == "Stradioti" || uc2.UnitType == "Landsknechte" || uc2.UnitType == "Suisse")
                    {
                        resultAttacks = myUnit.GetAttacksByArrowId(uc.GetArrowId("central"));
                        if (resultAttacks.Count > 0) tempAttack = resultAttacks[0];
                        else tempAttack = null;
                        resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("central"));
                        if (resultAttacks.Count > 0) tempAdditionalAttack = resultAttacks[0];
                        else tempAdditionalAttack = null;

                        if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("left"));
                        else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("left"));
                        foreach (Attack a in resultAttacks)
                        {
                            if (a.GetTargetId() == uc.UnitId && (a.GetName() != "Charge!" || (a.GetName() == "Charge!" && (uc2.UnitType == "Landsknechte" || uc2.UnitType == "Suisse"))))
                            {
                                if (tempAttack != null) tempAttack.AddActivatedAttackId(a.GetId());
                                if (tempAdditionalAttack != null) tempAdditionalAttack.AddActivatedAttackId(a.GetId());
                            }
                        }
                    }
                    if ((uc.UnitType == "Landsknechte" || uc.UnitType == "Suisse") && uc2.UnitType != "Artillery") //set deactivated attacks by Landsknechts charge
                    {
                        if(uc2.UnitTileId == leftAttackTile) resultAttacks = myUnit.GetAttacksByArrowId(uc.GetArrowId("left"));
                        else resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("left"));
                        if (resultAttacks.Count > 0)
                        {
                            tempAttack = null;
                            foreach(Attack a in resultAttacks)
                            {
                                if (a.GetName() == "Charge!") tempAttack = a;
                            }
                            if (tempAttack != null)
                            {
                                if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("left"));
                                else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("left"));
                                foreach (Attack a in resultAttacks)
                                {
                                    if (a.GetTargetId() == uc.UnitId && a.GetName() == "Charge!") tempAttack.AddActivatedAttackId(a.GetId());
                                    else
                                    {
                                        tempAttack.AddDeactivatedAttackId(a.GetId());
                                        tempAttack.AddBlockedAttackId(a.GetId());
                                    }
                                }
                            }
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("central"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("central"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("right"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                        }
                    }
                }
                if (uc2.UnitTileId == rightAttackTile || uc2.UnitTileId == rightAttackTileSupport) // adds to central attack, attack  on the right unit, that is activated by this central attack
                {
                    otherUnit = myBoardState.GetUnit(uc2.UnitId);
                    if (uc2.UnitType == "Arquebusiers" || uc2.UnitType == "Coustilliers" || uc2.UnitType == "Stradioti" || uc2.UnitType == "Landsknechte" || uc2.UnitType == "Suisse")
                    {
                        resultAttacks = myUnit.GetAttacksByArrowId(uc.GetArrowId("central"));
                        if (resultAttacks.Count > 0) tempAttack = resultAttacks[0];
                        else tempAttack = null;
                        resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("central"));
                        if (resultAttacks.Count > 0) tempAdditionalAttack = resultAttacks[0];
                        else tempAdditionalAttack = null;

                        if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                        else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("right"));
                        foreach (Attack a in resultAttacks)
                        {
                            if (a.GetTargetId() == uc.UnitId && (a.GetName() != "Charge!" || (a.GetName() == "Charge!" && (uc2.UnitType == "Landsknechte" || uc2.UnitType == "Suisse"))))
                            {
                                if (tempAttack != null) tempAttack.AddActivatedAttackId(a.GetId());
                                if (tempAdditionalAttack != null) tempAdditionalAttack.AddActivatedAttackId(a.GetId());
                            }
                        }
                        if (uc.UnitTileId == 14 || uc.UnitTileId == 15)
                        {
                            resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("central"));
                            if (resultAttacks.Count > 1) tempAdditionalAttack = resultAttacks[1];
                            else tempAdditionalAttack = null;
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("right"));
                            foreach (Attack a in resultAttacks)
                            {
                                if (a.GetTargetId() == uc.UnitId && (a.GetName() != "Charge!" || (a.GetName() == "Charge!" && (uc2.UnitType == "Landsknechte" || uc2.UnitType == "Suisse"))))
                                {
                                    if (tempAdditionalAttack != null) tempAdditionalAttack.AddActivatedAttackId(a.GetId());
                                }
                            }
                        }
                    }
                    if ((uc.UnitType == "Landsknechte" || uc.UnitType == "Suisse") && uc2.UnitType != "Artillery") //set deactivated attacks by Landsknechts charge
                    {
                        if (uc2.UnitTileId == rightAttackTile) resultAttacks = myUnit.GetAttacksByArrowId(uc.GetArrowId("right"));
                        else resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("right"));
                        if (resultAttacks.Count > 0)
                        {
                            tempAttack = null;
                            foreach (Attack a in resultAttacks)
                            {
                                if (a.GetName() == "Charge!") tempAttack = a;
                            }
                            if (tempAttack != null)
                            {
                                if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                                else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("right"));
                                foreach (Attack a in resultAttacks)
                                {
                                    if (a.GetTargetId() == uc.UnitId && a.GetName() == "Charge!") tempAttack.AddActivatedAttackId(a.GetId());
                                    else
                                    {
                                        tempAttack.AddDeactivatedAttackId(a.GetId());
                                        tempAttack.AddBlockedAttackId(a.GetId());
                                    }
                                }
                            }
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("central"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("central"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("left"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("left"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                        }
                    }
                }
                if((uc2.UnitTileId == centralAttackTile || uc2.UnitTileId == centralAttackTileSupport) && !(uc.UnitTileId == 6 && uc2.UnitTileId == 8) && !(uc.UnitTileId == 8 && uc2.UnitTileId == 6)) // changes attack strength because of tile g2 unit sits on
                {
                    otherUnit = myBoardState.GetUnit(uc2.UnitId);
                    if(myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("central"));
                    else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("central"));
                    if (resultAttacks.Count > 0) tempAttack = resultAttacks[0];
                    else tempAttack = null;
                    if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) tc = GetTile(uc.UnitTileId - 1);
                    else if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tc = GetTile(uc.UnitTileId + 1);
                    else tc = GetTile(uc.UnitTileId);
                    tempAttack.ChangeDefence(tc.ChangeDefenceStrength(uc.UnitType, uc2.UnitType));
                    if (resultAttacks.Count > 1)
                    {
                        tempAttack = resultAttacks[1];
                        if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) tc = GetTile(uc.UnitTileId - 1);
                        else if ((uc.UnitTileId - 1) % BattleManager.Instance.boardHeight == 0) tc = GetTile(uc.UnitTileId + 1);
                        else tc = GetTile(uc.UnitTileId);
                        tempAttack.ChangeDefence(tc.ChangeDefenceStrength(uc.UnitType, uc2.UnitType));
                    }
                    if((uc.UnitType == "Landsknechte" || uc.UnitType == "Suisse") && uc2.UnitType != "Artillery") //set deactivated attacks by Landsknechts charge
                    {
                        if (uc2.UnitTileId == centralAttackTile) resultAttacks = myUnit.GetAttacksByArrowId(uc.GetArrowId("central"));
                        else resultAttacks = myUnit.GetAdditionalAttacksByArrowId(uc.GetArrowId("central"));
                        if (resultAttacks.Count > 0)
                        {
                            tempAttack = resultAttacks[0];

                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("right"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("left"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("left"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                        }
                        if (resultAttacks.Count > 1)
                        {
                            tempAttack = resultAttacks[1];

                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("right"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("right"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                            if (myUnit.UnitMoved()) resultAttacks = otherUnit.GetAttacksByArrowId(uc2.GetArrowId("left"));
                            else resultAttacks = otherUnit.GetAdditionalAttacksByArrowId(uc2.GetArrowId("left"));
                            foreach (Attack a in resultAttacks)
                            {
                                tempAttack.AddDeactivatedAttackId(a.GetId());
                                tempAttack.AddBlockedAttackId(a.GetId());
                            }
                        }
                    }
                }
            }
        }
        // sets activating attacs for attack arrows
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (!uc.isPlaced || uc.UnitTileId == 8) continue;
            ac = uc.GetArrowController("left");
            resultAttacks = myBoardState.GetAttacksByArrowId(ac.ArrowId);
            attackList = new List<int>();
            foreach (Attack a in resultAttacks)
            {
                if (a.GetName() == "Counter Attack" || a.GetName() == "Capture") attackList.AddRange(myBoardState.GetAttacksActivating(a.GetId())); 
                else if (a.GetOwner().GetUnitType() == "Landsknechte" || a.GetOwner().GetUnitType() == "Suisse") attackList.AddRange(myBoardState.GetAttacksActivating(a.GetId()));
            }
            foreach (int i in attackList)
            {
                ac.AddActivatingAttack(i);
            }
            ac = uc.GetArrowController("right");
            resultAttacks = myBoardState.GetAttacksByArrowId(ac.ArrowId);
            attackList = new List<int>();
            foreach (Attack a in resultAttacks)
            {
                if (a.GetName() == "Counter Attack" || a.GetName() == "Capture") attackList.AddRange(myBoardState.GetAttacksActivating(a.GetId()));
                else if (a.GetOwner().GetUnitType() == "Landsknechte" || a.GetOwner().GetUnitType() == "Suisse") attackList.AddRange(myBoardState.GetAttacksActivating(a.GetId()));
            }
            foreach (int i in attackList)
            {
                ac.AddActivatingAttack(i);
            }
        }
        afterDeploymentBoardState = new BoardState(myBoardState);
        gameMode = "fight";
        EventManager.RaiseEventGameStart();
    }

    // updated in memory state of board
    private void DiceThrown(StateChange result)
    {
        int winnerId;
        int attackerArmyId, defenderArmyId;
        // check if route test needed
        isInputBlocked = false;
        armyRouteTest = 0;
        attackerArmyId = turnOwnerId;
        if (turnOwnerId == 1)
        {
            defenderArmyId = 2;
        }
        else defenderArmyId = 1;
        if (result.defenderId != 0)
        {
            if ((result.attackerStrengthChange < 0 || myBoardState.GetUnit(result.attackerId).morale + result.attackerMoraleChanged <= 0)
                && (result.defenderStrengthChange < 0 || myBoardState.GetUnit(result.defenderId).morale + result.defenderMoraleChanged <= 0)
                && myBoardState.GetArmy(attackerArmyId).GetMorale() <= 30
                && myBoardState.GetArmy(defenderArmyId).GetMorale() <= 30) armyRouteTest = 3;
            else if ((result.attackerStrengthChange < 0 || myBoardState.GetUnit(result.attackerId).morale + result.attackerMoraleChanged <= 0) && myBoardState.GetArmy(attackerArmyId).GetMorale() <= 30) armyRouteTest = attackerArmyId;
            else if ((result.defenderStrengthChange < 0 || myBoardState.GetUnit(result.defenderId).morale + result.defenderMoraleChanged <= 0) && myBoardState.GetArmy(defenderArmyId).GetMorale() <= 30) armyRouteTest = defenderArmyId;
        }
        // check if game ended
        winnerId = myBoardState.ChangeState(result);
        if (winnerId != 0) EventManager.RaiseEventGameOver(winnerId);
    }

    private void MakeRouteTest(string closedMode)
    {
        Vector3 testSpot = new Vector3(20.0f, 2.0f, -16.0f);
        if (armyRouteTest == 0 || armyRouteTest < 3 && closedMode == "routtest" || ignoreRoutTest)
        {
            EventManager.RaiseEventRouteTestOver("noResult", 0, 0);
        }
        else
        {
            BattleManager.Instance.isInputBlocked = true;
            if (armyRouteTest == 1 || armyRouteTest == 3 && turnOwnerId == 2 && closedMode != "routtest" || armyRouteTest == 3 && turnOwnerId == 1 && closedMode == "routtest")
            {
                //SoundManagerController.Instance.PlayThrowSound(0);
                myCamera.GetComponent<PanZoom>().RoutTest(testSpot + new Vector3(2.0f, 0.0f, 1.0f));
                StartCoroutine(WaitForRouteTest(1, closedMode, "d10-yellow"));
            }
            else if(armyRouteTest == 2 || armyRouteTest == 3 && turnOwnerId == 1 && closedMode != "routtest" || armyRouteTest == 3 && turnOwnerId == 2 && closedMode == "routtest")
            {
                //SoundManagerController.Instance.PlayThrowSound(0);
                myCamera.GetComponent<PanZoom>().RoutTest(testSpot + new Vector3(2.0f, 0.0f, 1.0f));
                StartCoroutine(WaitForRouteTest(2, closedMode, "d10-blue"));
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
        throwId = Dice.Roll("3d10", diceTexture, testSpot, new Vector3(2.0f, 6.5f + Random.value * 0.5f, 0.5f));
        while (Dice.rolling)
        {
            yield return null;
        }
        isInputBlocked = false;
        stringResult = Dice.AsString("d10", throwId);
        armyMorale = myBoardState.GetArmyMorale(testingArmyId);
        if (!stringResult.Contains("?"))
        {
            throwResult = Dice.Value("d10");
            //throwResult = 1;
            //Debug.Log("Wynik testu morale: " + throwResult);
            if (throwResult > armyMorale)
            {
                if (testingArmyId == 1) resultDescription = "imperialFlee";
                else resultDescription = "frenchFlee";
            }
            else
            {
                if (testingArmyId == 1) resultDescription = "imperialStays";
                else resultDescription = "frenchStays";
            }
            //Debug.Log(resultDescription);
            yield return new WaitForSeconds(1.5f);
            EventManager.RaiseEventRouteTestOver(resultDescription, throwResult, armyMorale);
            Dice.Clear();
        }
        else
        {
            //Debug.Log("Błąd przy route test");
            Dice.Clear();
            MakeRouteTest(mode);
        }
    }

    private void MakeAttack(int idAttack)
    {
        Attack myAttack;

        myAttack = myBoardState.GetAttackById(idAttack);
        if (myAttack.GetArmyId() == turnOwnerId && !hasTurnOwnerAttacked)
        {
            myAttack.MakeAttack();
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
        myAttack = inBoardState.GetAttackById(attackId);
        foreach (StateChange sc in myAttack.GetOutcomes())
        {
            outBoardState = new BoardState(inBoardState);
            outBoardState.ChangeState(sc);
            if (sc.defenderId != 0)
            {
                if ((sc.defenderStrengthChange != 0 || outBoardState.GetUnit(sc.defenderId).morale <= 0) && outBoardState.GetArmyMorale(otherArmyId) < 30)  //defender looses rout test
                {
                    defenderRoutProbability = routProbabilityTable[outBoardState.GetArmyMorale(otherArmyId)];
                    attackValue += -500.0f * defenderRoutProbability * sc.changeProbability;
                }
            }
            if((sc.attackerStrengthChange != 0 || outBoardState.GetUnit(sc.attackerId).morale <= 0) && outBoardState.GetArmyMorale(armyId) < 30) // attacker looses rout test
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
        int bestAttack = 0, maxDice, diceCount;
        float score, maxScore, minimaxLimit;

        maxScore = -1000.0f;
        maxDice = 0;
        switch(GameManagerController.Instance.difficultyLevel)
        {
            case GameManagerController.diffLevelEnum.easy:
                minimaxLimit = 1.0f;
                break;
            case GameManagerController.diffLevelEnum.medium:
                minimaxLimit = 5.0f;
                break;
            case GameManagerController.diffLevelEnum.hard:
                minimaxLimit = 10.0f;
                break;
            default:
                minimaxLimit = 5.0f;
                break;
        }
        avialableAttacks = myBoardState.GetPossibleAttacks(turnOwnerId);
        if (avialableAttacks.Count > 0)
        {
            foreach (int i in avialableAttacks)
            {
                if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.easy)
                {
                    diceCount = myBoardState.GetAttackById(i).GetAttackDiceNumber();
                    if (diceCount > maxDice)
                    {
                        maxDice = diceCount;
                        bestAttack = i;
                    }
                }
                else
                {
                    score = -ExpectiMinMaxAttack(new BoardState(myBoardState), i, minimaxLimit, turnOwnerId);
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestAttack = i;
                    }
                }
            }
            if (bestAttack > 0)
            {
                EventManager.RaiseEventOnUnitClicked(myBoardState.GetAttackById(bestAttack).GetOwner().GetUnitId());
                EventManager.RaiseEventOnAttackClicked(GetAttackById(bestAttack).GetArrowId(), false);
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

    public Attack GetAttackById(int aId)
    {
        return myBoardState.GetAttackById(aId);
    }

    public List<Attack> GetAttacksByArrowId(int arrowId)
    {
        return myBoardState.GetAttacksByArrowId(arrowId);
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
        Unit tu;
        int tai;
        tu = myBoardState.GetUnit(unitId);
        tai = tu.GetArmyId();
        return myBoardState.GetArmyName(tai);
    }

    public string GetKeyFieldName(int keyFieldId)
    {
        return myBoardState.GetKeyFieldName(keyFieldId);
    }

    public void RemoveUnitController(GameObject u)
    {
        units.Remove(u);
    }

    public UnitController GetUnitController(int uId)
    {
        UnitController uc;

        foreach(GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (uc.UnitId == uId) return uc;
        }
        return null;
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

    public int GetOpposingUnitId(int uId)
    {
        UnitController uc;
        int opposingTileId, result;

        uc = null;
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (uc.UnitId == uId) break;
        }
        if (uc == null || uc.UnitTileId == 8) return 0;
        else
        {
            if (uc.ArmyId == 1) opposingTileId = uc.UnitTileId - 2;
            else opposingTileId = uc.UnitTileId + 2;
        }
        result = 0;
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (uc.UnitTileId == opposingTileId) result = uc.UnitId;
        }
        return result;
    }

    public void StartAgain()
    {
        UnitController uc;
        TileController tc;

        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            uc.isPlaced = false;
            uc.ResetUnit();
        }
        foreach(GameObject g in tiles)
        {
            tc = g.GetComponent<TileController>();
            tc.ResetTile();
            tc.ResetDeployedUnit();
        }
        gameMode = "deploy";
        turnOwnerId = 1;
        hasTurnOwnerAttacked = false;
        isInputBlocked = false;
        EventManager.RaiseEventOnDeploymentStart(1);
    }

    public void StartAfterDeployment()
    {
        UnitController uc;
        TileController tc;

        myBoardState = afterDeploymentBoardState;
        turnOwnerId = 1;
        hasTurnOwnerAttacked = false;
        isInputBlocked = false;
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            uc.ResetUnit();
        }
        foreach (GameObject g in tiles)
        {
            tc = g.GetComponent<TileController>();
            tc.ResetTile();
        }
        EventManager.RaiseEventGameStart();
    }
}
