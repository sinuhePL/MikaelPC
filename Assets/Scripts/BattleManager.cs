using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleManager : MonoBehaviour {

    private static BattleManager _instance;
    private BoardState myBoardState;
    private List<GameObject> units;
    private List<GameObject> tiles;

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
    [SerializeField] private GameObject[] boardFields;
    [SerializeField] private GameObject unitPrefab;
    [SerializeField] private GameObject cavalerySquadArmy1Prefab;
    [SerializeField] private GameObject cavalerySquadArmy2Prefab;

    public static int boardWidth = 6;
    public static int boardHeight = 5;
    public static float boardFieldWitdth = 4.0f;
    public static float boardFieldHeight = 4.0f;
    public static int maxSquads = 6;

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
        DOTween.Init();
        InitiateManager(boardWidth, boardHeight);
        InitiateBoard();
    }

    /*private int EvaluateBoardState(BoardState bs)
    {
        // dodać ciało metody 
    }*/

    private void InitiateManager(int _boardWidth, int _boardHeight)
    {
        GameObject tempObj;
        int tileCounter = 0;

        Random.InitState(System.Environment.TickCount);
        tiles = new List<GameObject>();
        //inicjalizacja elementów graficznych planszy
        for(int i=0; i< _boardWidth+4; i++)
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
                    int r = Random.Range(0, boardFields.Length);
                    tempObj = Instantiate(boardFields[r], new Vector3(i * 4.0f, 0.0f, j * -4.0f), boardFields[r].transform.rotation);
                    tempObj.GetComponent<TileController>().InitializeTile(++tileCounter);
                    tiles.Add(tempObj);
                }
            }
        }

        //inicjalizacja jednostek
        units = new List<GameObject>();
        // create test unit
        tempObj = Instantiate(unitPrefab, new Vector3(-10.0f, 0.05f, 10.0f), Quaternion.identity);
        tempObj.GetComponent<UnitController>().InitializeUnit(5, 1, cavalerySquadArmy1Prefab, 1, 1, 2, 3, "French Cavalery", 19);
        units.Add(tempObj);

        tempObj = Instantiate(unitPrefab, new Vector3(11.0f, 0.05f, -12.0f), Quaternion.identity);
        tempObj.GetComponent<UnitController>().InitializeUnit(4, 2, cavalerySquadArmy2Prefab, 2, 4, 5, 6, "French Cavalery", 7);
        units.Add(tempObj);
        // end test unit
    }

    private void InitiateBoard()
    {
        UnitController uc, uc2;
        Unit myUnit;
        Army army1, army2;
        Attack tempAttack;
        int army1morale = 0, army2morale = 0, leftAttackTile, centralAttackTile, rightAttackTile, leftAttackTargetId, centralAttackTargetId, rightAttackTargetId;

        // counts morale of army
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            if (uc.ArmyId == 1) army1morale += uc.InitialMorale;
            else army2morale += uc.InitialMorale;
        }
        army1 = new Army(1, army1morale, 0, 6);
        army2 = new Army(2, army2morale, 0, 6);
        myBoardState = new BoardState(army1, army2);

        // creates in memory representation of each unit on screen
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
            myUnit = new Unit(uc.UnitId, uc.UnitType, uc.InitialStrength, uc.InitialMorale, uc.ArmyId == 1 ? army1 : army2);
            
            //looks for tiles ids which attack arrows point at
            if (uc.ArmyId == 1)
            {
                leftAttackTile = uc.UnitTileId - 13;
                if (leftAttackTile % BattleManager.boardWidth == 5) leftAttackTile = 0;
                centralAttackTile = uc.UnitTileId - 12;
                rightAttackTile = uc.UnitTileId - 11;
                if (rightAttackTile % BattleManager.boardWidth == 0) rightAttackTile = 0;
            }
            else
            {
                leftAttackTile = uc.UnitTileId + 13;
                if (leftAttackTile % BattleManager.boardWidth == 0) leftAttackTile = 0;
                centralAttackTile = uc.UnitTileId + 12;
                rightAttackTile = uc.UnitTileId + 11;
                if (rightAttackTile % BattleManager.boardWidth == 5) rightAttackTile = 0;
            }
            //looks for units ids which sits on tiles pointed at attack arrows
            leftAttackTargetId = 0;
            centralAttackTargetId = 0;
            rightAttackTargetId = 0;
            foreach (GameObject g2 in units)
            {
                uc2 = g2.GetComponent<UnitController>();
                if (uc2.UnitTileId == leftAttackTile) leftAttackTargetId = uc2.UnitTileId;
                if (uc2.UnitTileId == centralAttackTile) centralAttackTargetId = uc2.UnitTileId;
                if (uc2.UnitTileId == rightAttackTile) rightAttackTargetId = uc2.UnitTileId;

            }
            if (leftAttackTargetId > 0)
            {
                tempAttack = new Attack(uc.GetAttackId(1), true, uc.ArmyId, uc, 0, leftAttackTargetId, uc.transform.position + new Vector3(-1.0f, 0.0, 2.0f));
                uc.ActivateAttack(uc.GetAttackId(1));
                myUnit.AddAttack(tempAttack);
            }
            if (centralAttackTargetId > 0)
            {
                tempAttack = new Attack(uc.GetAttackId(2), true, uc.ArmyId, uc, 0, centralAttackTargetId, uc.transform.position + new Vector3(1.0f, 0.0, 2.0f));
                uc.ActivateAttack(uc.GetAttackId(2));
                myUnit.AddAttack(tempAttack);
            }
            if (leftAttackTargetId > 0)
            {
                tempAttack = new Attack(uc.GetAttackId(3), true, uc.ArmyId, uc, 0, rightAttackTargetId, uc.transform.position + new Vector3(2.0f, 0.0, 2.0f));
                uc.ActivateAttack(uc.GetAttackId(3));
                myUnit.AddAttack(tempAttack);
            }

            myBoardState.AddUnit(myUnit);
        }
    }

    /*private int MinMax(BoardState bs)
    {
        // dodać ciało funkcji
    }

    public void MakeAttack(int un, int at)
    {
        Unit tempUnit;
        StateChange tempStateChange;

        //tempUnit = myBoardState.GetUnit(un);
        tempStateChange = tempUnit.MakeAttack(at);
        myBoardState.ChangeState(tempStateChange);
        // dodać wywołanie zmian w widoku na podstawie otrzymanego StateChange
    }*/

    public Unit GetUnit(int unitId)
    {
        return myBoardState.GetUnit(unitId);
    }

    public Attack GetAttack(int aId)
    {
        return myBoardState.GetAttack(aId);
    }
}
