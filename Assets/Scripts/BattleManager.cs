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
    private Camera myCamera;

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
    public static int turnOwnerId = 1;
    public static bool isPlayer1Human = true;
    public static bool isPlayer2Human = false;
    public static bool hasTurnOwnerAttacked = false;

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
        DOTween.Init();
        InitiateManager(boardWidth, boardHeight);
        InitiateBoard();
        myCamera = Camera.main;
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += DiceThrown;
        EventManager.onAttackOrdered += MakeAttack;
        EventManager.onTurnEnd += TurnEnd;
    }

    private void Start()
    {
        EventManager.RaiseEventGameStart();
    }

    private void InitiateManager(int _boardWidth, int _boardHeight)
    {
        GameObject tempObj;
        int tileCounter = 0;

        Random.InitState(System.Environment.TickCount);
        tiles = new List<GameObject>();
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
        tempObj.GetComponent<UnitController>().InitializeUnit(5, 5, 1, cavalerySquadArmy1Prefab, 1, 1, 2, 3, "French Cavalery", 19);
        units.Add(tempObj);

        tempObj = Instantiate(unitPrefab, new Vector3(11.0f, 0.05f, -12.0f), Quaternion.identity);
        tempObj.GetComponent<UnitController>().InitializeUnit(4, 6, 2, cavalerySquadArmy2Prefab, 2, 4, 5, 6, "French Cavalery", 7);
        units.Add(tempObj);
        // end test unit
    }

    private void InitiateBoard()
    {
        UnitController uc, uc2;
        Unit myUnit;
        Army army1, army2;
        Attack tempAttack;
        Vector3 menuRightPositionShift, menuCentralPositionShift, menuLeftPositionShift;
        int army1morale = 0, army2morale = 0, leftAttackTile, centralAttackTile, rightAttackTile, leftAttackTargetId, centralAttackTargetId, rightAttackTargetId;

        // counts morale of army
        foreach (GameObject g in units)
        {
            uc = g.GetComponent<UnitController>();
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
                if (uc2.UnitTileId == leftAttackTile) leftAttackTargetId = uc2.UnitId;
                if (uc2.UnitTileId == centralAttackTile) centralAttackTargetId = uc2.UnitId;
                if (uc2.UnitTileId == rightAttackTile) rightAttackTargetId = uc2.UnitId;

            }
            if(uc.ArmyId == 1)
            {
                menuLeftPositionShift = new Vector3(1.0f, 0.0f, 5.0f);
                menuCentralPositionShift = new Vector3(3.0f, 0.0f, 5.0f);
                menuRightPositionShift = new Vector3(5.0f, 0.0f, 5.0f);
            }
            else
            {
                menuLeftPositionShift = new Vector3(1.0f, 0.0f, -3.0f);
                menuCentralPositionShift = new Vector3(3.0f, 0.0f, -3.0f);
                menuRightPositionShift = new Vector3(5.0f, 0.0f, -3.0f);
            }
            if (leftAttackTargetId > 0)
            {
                tempAttack = new ChargeAttack(uc.GetAttackId(1), true, uc.ArmyId, myUnit, 0, leftAttackTargetId, uc.transform.position + menuLeftPositionShift, 3, 2);
                uc.ActivateAttack(uc.GetAttackId(1)); // testowo, docelowo tylko central attack jest aktywnyna początku
                myUnit.AddAttack(tempAttack);
            }
            if (centralAttackTargetId > 0)
            {
                tempAttack = new ChargeAttack(uc.GetAttackId(2), true, uc.ArmyId, myUnit, 0, centralAttackTargetId, uc.transform.position + menuCentralPositionShift, 3, 2);
                uc.ActivateAttack(uc.GetAttackId(2));
                myUnit.AddAttack(tempAttack);
            }
            if (rightAttackTargetId > 0)
            {
                tempAttack = new ChargeAttack(uc.GetAttackId(3), true, uc.ArmyId, myUnit, 0, rightAttackTargetId, uc.transform.position + menuRightPositionShift, 3, 2);
                uc.ActivateAttack(uc.GetAttackId(3)); // testowo, docelowo tylko central attack jest aktywnyna początku
                myUnit.AddAttack(tempAttack);
            }

            myBoardState.AddUnit(myUnit);
        }
    }

    // updated in memory state of board
    private void DiceThrown(StateChange result)
    {
        int winnerId;

        winnerId = myBoardState.ChangeState(result);
        if (winnerId != 0) EventManager.RaiseEventGameOver(winnerId);
    }

    // Waits for dice to stop rolling
    private IEnumerator WaitForDice(int throw1Id, int throw2Id, int attackId)
    {
        Attack tempAttack;
        string result1 = "", result2 = "";

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
                for (int i = 0; i < throw1Hits.Length; i++)
                {
                    if (throw1Hits[i] == "S") attackStrengthHit++;
                    if (throw1Hits[i] == "M") attackMoraleHit++;
                }
                for (int i = 0; i < throw2Hits.Length; i++)
                {
                    if (throw2Hits[i] == "S") defenceStrengthHit++;
                    if (throw2Hits[i] == "M") defenceMoraleHit++;
                }
                tempAttack = myBoardState.GetAttack(attackId);
                result.attackerId = tempAttack.GetOwner().GetUnitId();
                result.defenderId = tempAttack.GetTargetId();
                result.attackerMoraleChanged = -defenceMoraleHit;
                result.attackerStrengthChange = -defenceStrengthHit;
                result.defenderMoraleChanged = -attackMoraleHit;
                result.defenderStrengthChange = -attackStrengthHit;
                Debug.Log("Attack inflicted " + attackStrengthHit + " strength casualty and " + attackMoraleHit + " morale loss for defender.");
                Debug.Log("Defence inflicted " + defenceStrengthHit + " strength casualty and " + defenceMoraleHit + " morale loss for attacker.");
                hasTurnOwnerAttacked = true;
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
                if(myAttack.GetAttackDiceNumber() > 0) throw1 = Dice.Roll(myAttack.GetAttackDiceNumber().ToString() + "d6", "d6-blue", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -1.0f), new Vector3(0.1f, 0.2f + Random.value * 0.75f, 0.1f));
                if(myAttack.GetDefenceDiceNumber() > 0) throw2 = Dice.Roll(myAttack.GetDefenceDiceNumber().ToString() + "d6", "d6-yellow", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -2.0f), new Vector3(0.1f, 0.2f + Random.value * 0.75f, 0.1f));
            }
            else
            {
                if (myAttack.GetAttackDiceNumber() > 0) throw1 = Dice.Roll(myAttack.GetAttackDiceNumber().ToString() + "d6", "d6-yellow", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -2.0f), new Vector3(0.1f, 0.2f + Random.value * 0.75f, 0.1f));
                if (myAttack.GetDefenceDiceNumber() > 0) throw2 = Dice.Roll(myAttack.GetDefenceDiceNumber().ToString() + "d6", "d6-blue", myAttack.GetPosition() + new Vector3(-2.0f, 2.0f, -1.0f), new Vector3(0.1f, 0.2f + Random.value * 0.75f, 0.1f));
            }
            myCamera.GetComponent<PanZoom>().LookAtDice(myAttack.GetPosition() + new Vector3(0.0f, 10.0f, 0.0f));
            StartCoroutine(WaitForDice(throw1, throw2, idAttack));
        }
    }

    private void TurnEnd()
    {
        hasTurnOwnerAttacked = false;
        if (turnOwnerId == 1 && !isPlayer1Human || turnOwnerId == 2 && !isPlayer2Human)
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
        float attackValue = 0;
        Attack myAttack;
        BoardState outBoardState;
        myAttack = inBoardState.GetAttack(attackId);
        foreach (StateChange sc in myAttack.GetOutcomes())
        {
            outBoardState = new BoardState(inBoardState);
            outBoardState.ChangeState(sc);
            attackValue += sc.changeProbability * ExpectiMinMaxBoardState(outBoardState, limit, armyId);
        }
        return attackValue;
    }

    private void ComputeBestAttack()
    {
        List<int> avialableAttacks = new List<int>();
        int bestAttack = 0;
        float score, maxScore;

        maxScore = -1000.0f;
        avialableAttacks = myBoardState.GetPossibleAttacks(turnOwnerId);
        if (avialableAttacks.Count > 0)
        {
            foreach (int i in avialableAttacks)
            {
                score = -ExpectiMinMaxAttack(myBoardState, i, 30.0f, turnOwnerId);
                if (score > maxScore)
                {
                    maxScore = score;
                    bestAttack = i;
                }
            }
            if (bestAttack > 0)
            {
                EventManager.RaiseEventOnUnitClicked(myBoardState.GetAttack(bestAttack).GetOwner().GetUnitId());
                EventManager.RaiseEventOnAttackClicked(bestAttack);
                SoundManagerController.Instance.PlayThrowSound(0);
                MakeAttack(bestAttack);
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= DiceThrown;
        EventManager.onAttackOrdered -= MakeAttack;
        EventManager.onTurnEnd -= TurnEnd;
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
}
