using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private static BattleManager _instance;
    private BoardState myBoardState;
    private List<GameObject> units;
    private int clickedUnitId;

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
    [SerializeField] private Transform[] boardFields;
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
        myBoardState = InitiateBoard(boardWidth, boardHeight);
    }

    /*private int EvaluateBoardState(BoardState bs)
    {
        // dodać ciało metody 
    }*/

    private BoardState InitiateBoard(int _boardWidth, int _boardHeight)
    {
        GameObject tempObj;

        Random.InitState(System.Environment.TickCount);
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
                    Instantiate(boardFields[r], new Vector3(i * 4.0f, 0.0f, j * -4.0f), boardFields[r].transform.rotation);
                }
            }
        }

        //inicjalizacja jednostek
        clickedUnitId = -1;
        units = new List<GameObject>();
        // create test unit
        tempObj = Instantiate(unitPrefab, new Vector3(15.0f, 0.05f, -20.0f), Quaternion.identity);
        tempObj.GetComponent<UnitController>().InitializeUnit(5, 1, cavalerySquadArmy1Prefab, 1, 1, 2, 3);
        units.Add(tempObj);

        tempObj = Instantiate(unitPrefab, new Vector3(11.0f, 0.05f, -12.0f), Quaternion.identity);
        tempObj.GetComponent<UnitController>().InitializeUnit(4, 2, cavalerySquadArmy2Prefab, 2, 4, 5, 6);
        units.Add(tempObj);
        // end test unit
        EventManager.onUnitClicked += myUnitClicked;

        return new BoardState(20, 6);
    }

    private void myUnitClicked(int unitId)
    {
        if(clickedUnitId != unitId && clickedUnitId >= 0)
        {
            for (int j = 0; j < units.Count; j++)
            {
                if (units[j].GetComponent<UnitController>().UnitId == clickedUnitId)
                {
                    units[j].GetComponent<UnitController>().DisableOutline();
                    break;
                }
            }
        }
        if (unitId >= 0)
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].GetComponent<UnitController>().UnitId == unitId)
                {
                    //units[i].GetComponent<UnitController>().KillSquads(1);
                    units[i].GetComponent<UnitController>().Outline();
                    break;
                }
            }
        }
        clickedUnitId = unitId;
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

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= myUnitClicked;
    }
}
