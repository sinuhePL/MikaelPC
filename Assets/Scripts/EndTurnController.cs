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
    }

    public void ButtonPressed()
    {
        if (!BattleManager.isInputBlocked)
        {
            transform.DOKill();
            transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
            if (mode == 1)  // if displays End Turn
            {
                isClicked = true;
                leftArrow.GetComponent<LookArrowController>().ChangeActivityState();
                rightArrow.GetComponent<LookArrowController>().ChangeActivityState();
                if (BattleManager.turnOwnerId == 1) BattleManager.turnOwnerId = 2;
                else BattleManager.turnOwnerId = 1;
                EventManager.RaiseEventOnTurnEnd();
                return;
            }
            if (mode == 2) // if displays Attack
            {
                EventManager.RaiseEventOnAttackOrdered(LastClickedAttack);
                myText.text = "";
            }
            if (mode == 3) // if displays Close Result (attack result)
            {
                //AttackResolved();
                myText.text = "";
                EventManager.RaiseEventResultMenuClosed("attack");
            }
            if(mode == 4) // if displays Close Result (rout test result)
            {
                //AttackResolved();
                myText.text = "";
                EventManager.RaiseEventResultMenuClosed("routtest");
            }
        }
    }

    private void RouteTestEnded(string resultDescription, int result, int morale)
    {
        if (resultDescription == "noResult") AttackResolved();
        else
        {
            myText.text = "Close Result";
            mode = 4;
            if (resultDescription == "frenchFlee") EventManager.RaiseEventGameOver(2);
            else if (resultDescription == "imperialFlee") EventManager.RaiseEventGameOver(1);
        }
    }

    public void AttackResolved()
    {
        myText.text = "End Turn";
        mode = 1;
        if (BattleManager.turnOwnerId == 1 && !BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && !BattleManager.isPlayer2Human) ButtonPressed();
        else StartCoroutine(WaitForClick());
    }

    public void AttackClicked(int attackId)
    {
        if (BattleManager.turnOwnerId == BattleManager.Instance.GetAttack(attackId).GetOwner().GetArmyId() && !BattleManager.hasTurnOwnerAttacked)
        {
            if (BattleManager.turnOwnerId == 1 && BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && BattleManager.isPlayer2Human)
            {
                myText.text = "Attack";
                mode = 2;
                lastClickedAttack = attackId;
            }
            else myText.text = "";
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
        mode = 3;
    }

    public void Reset(int i)
    {
        if (mode == 2)
        {
            myText.text = "End Turn";
            mode = 1;
        }
    }
}
