using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EndTurnController : MonoBehaviour
{
    private bool isClicked;
    private int mode; // 1 - end turn, 2 - attack, 3 - close attack result
    private Text myText;
    private int lastClickedAttack;

    public int LastClickedAttack { get => lastClickedAttack; set => lastClickedAttack = value; }

    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    private IEnumerator WaitForClick()
    {
        isClicked = false;
        while (!isClicked)
        {
            //DOTween.Clear();  powoduje problemy przy braku route testu
            transform.DOKill();
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.0f), 0.5f);
            yield return new WaitForSeconds(3.0f);
        }
    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onDiceResult += DiceThrown;
        EventManager.onTileClicked += Reset;
        EventManager.onUnitClicked += Reset;
        EventManager.onRouteTestOver += RouteTestEnded;
        EventManager.onGameStart += GameStart;
    }

    private void Start()
    {
        isClicked = false;
        myText = GetComponentInChildren<Text>();
        mode = 1;
        LastClickedAttack = 0;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onDiceResult -= DiceThrown;
        EventManager.onTileClicked -= Reset;
        EventManager.onUnitClicked -= Reset;
        EventManager.onRouteTestOver -= RouteTestEnded;
        EventManager.onGameStart -= GameStart;
    }

    public void ButtonPressed()
    {
        if (!BattleManager.isInputBlocked)
        {
            transform.DOKill();
            transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
            if (mode == 0) return;
            if (mode == 1) // if displays End Deployment
            {
                if(BattleManager.turnOwnerId == 1)
                {
                    EventManager.RaiseEventOnDeploymentStart(2);
                }
                else if(BattleManager.turnOwnerId == 2)
                {
                    EventManager.RaiseEventOnDeploymentStart(3);
                    leftArrow.SetActive(true);
                    rightArrow.SetActive(true);
                }
                return;
            }
            if (mode == 2)  // if displays End Turn
            {
                isClicked = true;
                leftArrow.GetComponent<LookArrowController>().ChangeActivityState();
                rightArrow.GetComponent<LookArrowController>().ChangeActivityState();
                if (BattleManager.turnOwnerId == 1) BattleManager.turnOwnerId = 2;
                else BattleManager.turnOwnerId = 1;
                EventManager.RaiseEventOnTurnEnd();
                return;
            }
            if (mode == 3) // if displays Attack
            {
                EventManager.RaiseEventOnAttackOrdered(LastClickedAttack);
                myText.text = "";
            }
            if (mode == 4) // if displays Close Result (attack result)
            {
                myText.text = "";
                EventManager.RaiseEventResultMenuClosed("attack");
            }
            if(mode == 5) // if displays Close Result (rout test result)
            {
                myText.text = "";
                EventManager.RaiseEventResultMenuClosed("routtest");
            }
        }
    }

    private void GameStart()
    {
        myText.text = "End Turn";
        mode = 2;
    }

    private void RouteTestEnded(string resultDescription, int result, int morale)
    {
        if (resultDescription == "noResult") AttackResolved();
        else
        {
            myText.text = "Close Result";
            mode = 5;
            if (resultDescription == "frenchFlee") EventManager.RaiseEventGameOver(2);
            else if (resultDescription == "imperialFlee") EventManager.RaiseEventGameOver(1);
        }
    }

    public void AttackResolved()
    {
        myText.text = "End Turn";
        mode = 2;
        if (BattleManager.turnOwnerId == 1 && !GameManagerController.isPlayer1Human || BattleManager.turnOwnerId == 2 && !GameManagerController.isPlayer2Human) ButtonPressed();
        else StartCoroutine(WaitForClick());
    }

    public void AttackClicked(int attackId, bool isCounterAttack)
    {
        if (isCounterAttack)
        {
            myText.text = "";
            mode = 0;
            return;
        }
        if (BattleManager.turnOwnerId == BattleManager.Instance.GetAttack(attackId).GetOwner().GetArmyId() && !BattleManager.hasTurnOwnerAttacked)
        {
            if (BattleManager.turnOwnerId == 1 && GameManagerController.isPlayer1Human || BattleManager.turnOwnerId == 2 && GameManagerController.isPlayer2Human)
            {
                myText.text = "Attack";
                mode = 3;
                lastClickedAttack = attackId;
            }
            else
            {
                myText.text = "";
                mode = 0;
            }
        }
        else
        {
            Reset(0);
        }
    }

    public void DiceThrown(StateChange st)
    {
        if (st.attackerStrengthChange != 0 || st.defenderStrengthChange != 0) myText.text = "Rout Test";
        else myText.text = "Close Result";
        mode = 4;
    }

    public void Reset(int i)
    {
        if (mode == 3)
        {
            GameStart();
        }
    }
}
