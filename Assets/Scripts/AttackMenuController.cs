using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackMenuController : MonoBehaviour
{
    private bool isVisible;
    private Text attackNameText;
    private Text attackDiceNumberText;
    private Text defenceDiceNumberText;
    //private Text attackerText;
    //private Text defenderText;

    private void AttackClicked(int idAttack)
    {
        Sequence mySequence = DOTween.Sequence();
        Attack tempAttack;

        if (BattleManager.turnOwnerId == 1 && !BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && !BattleManager.isPlayer2Human) return;
        if(isVisible) mySequence.Append(transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack));
        mySequence.Append(transform.DOMoveX(410.0f, 0.3f).SetEase(Ease.OutBack));
        isVisible = true;
        tempAttack = BattleManager.Instance.GetAttack(idAttack);
        attackNameText.text = tempAttack.GetName();
        //attackerText.text = BattleManager.Instance.GetArmyName("attacker", tempAttack);
        //defenderText.text = BattleManager.Instance.GetArmyName("defender", tempAttack);
        attackDiceNumberText.text = tempAttack.GetAttackDiceNumber().ToString();
        defenceDiceNumberText.text = tempAttack.GetDefenceDiceNumber().ToString();
    }

    private void UnitClicked(int idUnit)
    {
        if (isVisible)
        {
            transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
    }

    private void TileClicked(int idTile)
    {
        if (isVisible)
        {
            transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
    }

    private void AttackButtonPressed(int idAttack)
    {
        if (isVisible)
        {
            transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
    }


    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onTileClicked -= TileClicked;
        EventManager.onAttackOrdered -= AttackButtonPressed;
    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onTileClicked += TileClicked;
        EventManager.onAttackOrdered += AttackButtonPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        isVisible = false;
        attackNameText = transform.Find("AttackName").GetComponent<Text>();
        attackDiceNumberText = transform.Find("AttackDiceNumber").GetComponent<Text>();
        defenceDiceNumberText = transform.Find("DefenceDiceNumber").GetComponent<Text>();
        //attackerText = transform.Find("AttackingArmyName").GetComponent<Text>();
        //defenderText = transform.Find("DefendingArmyName").GetComponent<Text>();
    }
}
