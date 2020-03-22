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
    private Text starText;
    private Text defenceDiceNumberText;
    private int lastClickedUnitId;

    private void AttackClicked(int idAttack)
    {
        Sequence mySequence = DOTween.Sequence();
        Attack tempAttack;

        if (BattleManager.turnOwnerId == 1 && !BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && !BattleManager.isPlayer2Human) return;
        if (isVisible)  // hide previous attack
        {
            mySequence.Append(transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack));
            mySequence.Join(transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack));
        }
        mySequence.Append(transform.DOMoveX(510.0f, 0.3f).SetEase(Ease.OutBack));
        mySequence.Join(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
        isVisible = true;
        tempAttack = BattleManager.Instance.GetAttack(idAttack);
        attackNameText.text = tempAttack.GetName();
        attackDiceNumberText.text = tempAttack.GetAttackDiceNumber().ToString();
        starText.text = tempAttack.GetSpecialOutcomeDescription();
        defenceDiceNumberText.text = tempAttack.GetDefenceDiceNumber().ToString();
    }

    private void UnitClicked(int idUnit)
    {
        if (isVisible && lastClickedUnitId != idUnit)
        {
            transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
        else if(isVisible && lastClickedUnitId == idUnit)
        {
            transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
        lastClickedUnitId = idUnit;
    }

    private void TileClicked(int idTile)
    {
        if (isVisible)
        {
            transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
    }

    private void AttackButtonPressed(int idAttack)
    {
        if (isVisible)
        {
            transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onTileClicked += TileClicked;
        EventManager.onAttackOrdered += AttackButtonPressed;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onTileClicked -= TileClicked;
        EventManager.onAttackOrdered -= AttackButtonPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        isVisible = false;
        attackNameText = transform.Find("AttackName").GetComponent<Text>();
        attackDiceNumberText = transform.Find("AttackDiceNumber").GetComponent<Text>();
        starText = transform.Find("StarText").GetComponent<Text>();
        defenceDiceNumberText = transform.Find("DefenceDiceNumber").GetComponent<Text>();
        lastClickedUnitId = 0;
    }
}
