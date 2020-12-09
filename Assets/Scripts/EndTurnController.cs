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
    private bool isShifted;
    private Vector3 startingPosition;
    private int lastClickedUnit;

    public int LastClickedAttack { get; set; }

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
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onRouteTestOver += RouteTestEnded;
        EventManager.onGameStart += GameStart;
        EventManager.onDeploymentStart += ChangeToMode1;
    }

    private void Start()
    {
        isClicked = false;
        myText = GetComponentInChildren<Text>();
        mode = 1;
        LastClickedAttack = 0;
        startingPosition = transform.position;
        isShifted = false;
        lastClickedUnit = 0;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onDiceResult -= DiceThrown;
        EventManager.onTileClicked -= Reset;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onRouteTestOver -= RouteTestEnded;
        EventManager.onGameStart -= GameStart;
        EventManager.onDeploymentStart -= ChangeToMode1;
    }

    public void ButtonPressed()
    {
        if (!BattleManager.Instance.isInputBlocked)
        {
            transform.DOKill();
            transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
            if (mode == 0) return;
            if (mode == 1) // if displays End Deployment
            {
                SoundManagerController.Instance.PlayClick();
                if (BattleManager.Instance.turnOwnerId == 1)
                {
                    EventManager.RaiseEventOnDeploymentStart(2);
                    if(!GameManagerController.Instance.isPlayer2Human)
                    {
                        leftArrow.SetActive(true);
                        rightArrow.SetActive(true);
                    }
                }
                else if(BattleManager.Instance.turnOwnerId == 2)
                {
                    EventManager.RaiseEventOnDeploymentStart(3);
                    leftArrow.SetActive(true);
                    rightArrow.SetActive(true);
                }
                return;
            }
            if (mode == 2)  // if displays End Turn
            {
                SoundManagerController.Instance.PlayClick();
                isClicked = true;
                leftArrow.GetComponent<LookArrowController>().ChangeActivityState();
                rightArrow.GetComponent<LookArrowController>().ChangeActivityState();
                EventManager.RaiseEventOnTurnEnd();
                return;
            }
            if (mode == 3) // if displays Attack
            {
                BattleManager.Instance.isInputBlocked = true;
                EventManager.RaiseEventOnAttackOrdered(LastClickedAttack);
                myText.text = "Close Result";
                return;
            }
            if (mode == 4) // if displays Close Result (attack result)
            {
                //SoundManagerController.Instance.ResumeMusic();
                SoundManagerController.Instance.PlayClick();
                myText.text = "";
                EventManager.RaiseEventResultMenuClosed("attack");
                return;
            }
            if(mode == 5) // if displays Close Result (rout test result)
            {
                SoundManagerController.Instance.PlayClick();
                myText.text = "";
                EventManager.RaiseEventResultMenuClosed("routtest");
                return;
            }
        }
    }

    private void ChangeToMode2()
    {
        myText.text = "End Turn";
        mode = 2;
    }

    private void ChangeToMode1(int i)
    {
        if (i == 1)
        {
            mode = 1;
            myText.text = "End Deployment";
            if (isShifted) ShiftMe();
        }
    }

    private void GameStart()
    {
        ChangeToMode2();
        if (!isShifted) ShiftMe();
    }

    private void RouteTestEnded(string resultDescription, int result, int morale)
    {
        if (resultDescription == "noResult") AttackResolved();
        else
        {
            myText.text = "Close Result";
            mode = 5;
            if (resultDescription == "frenchFlee") EventManager.RaiseEventGameOver(1);
            else if (resultDescription == "imperialFlee") EventManager.RaiseEventGameOver(2);
        }
    }

    public void AttackResolved()
    {
        myText.text = "End Turn";
        mode = 2;
        if (BattleManager.Instance.turnOwnerId == 1 && !GameManagerController.Instance.isPlayer1Human || BattleManager.Instance.turnOwnerId == 2 && !GameManagerController.Instance.isPlayer2Human) ButtonPressed();
        else StartCoroutine(WaitForClick());
    }

    public void AttackClicked(int arrowId, bool isCounterAttack)
    {
        List<Attack> tempAttacks;
        Attack tempAttack;

        if (isCounterAttack)
        {
            myText.text = "";
            mode = 0;
            return;
        }
        tempAttacks = BattleManager.Instance.GetAttacksByArrowId(arrowId);
        tempAttack = null;
        foreach (Attack a in tempAttacks)
        {
            if (a.IsActive()) tempAttack = a;
        }
        if (tempAttack == null) return;
        if (BattleManager.Instance.turnOwnerId == tempAttack.GetOwner().GetArmyId() && !BattleManager.Instance.hasTurnOwnerAttacked)
        {
            if (BattleManager.Instance.turnOwnerId == 1 && GameManagerController.Instance.isPlayer1Human || BattleManager.Instance.turnOwnerId == 2 && GameManagerController.Instance.isPlayer2Human)
            {
                myText.text = "Attack";
                mode = 3;
                LastClickedAttack = tempAttack.GetId();
                if (!isShifted) ShiftMe();
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
        int attackerArmyId, defenderArmyId;
        attackerArmyId = BattleManager.Instance.turnOwnerId;
        if (BattleManager.Instance.turnOwnerId == 1)
        {
            defenderArmyId = 2;
        }
        else defenderArmyId = 1;
        if (st.attackerStrengthChange != 0 && BattleManager.Instance.GetArmyMorale(attackerArmyId) <= 30 || st.defenderStrengthChange != 0 && BattleManager.Instance.GetArmyMorale(defenderArmyId) <= 30) myText.text = "Rout Test";
        else myText.text = "Close Result";
        mode = 4;
    }

    private void UnitClicked(int uId)
    {
        if (uId == lastClickedUnit && !isShifted) ShiftMe();
        else lastClickedUnit = uId;
        if (mode == 3)
        {
            ChangeToMode2();
        }
    }

    public void Reset(int i)
    {
        if (mode == 3)
        {
            ChangeToMode2();
        }
        if(!isShifted) ShiftMe();
    }

    public void ShiftMe()
    {
        if (!isShifted && BattleManager.Instance.gameMode != "deploy")
        {
            transform.DOMoveX(startingPosition.x - 440.0f, 0.3f).SetEase(Ease.InOutQuint);
            isShifted = true;
        }
        else
        {
            transform.DOMoveX(startingPosition.x, 0.3f).SetEase(Ease.InOutQuint);
            isShifted = false;
        }
    }
}
