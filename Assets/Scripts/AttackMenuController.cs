using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackMenuController : MonoBehaviour
{
    private Camera myCamera;
    private bool isVisible;
    private Text attackNameText;
    private Text attackDiceNumberText;
    private Text defenceDiceNumberText;
    private Text attackText;
    private Text defenceText;

    private void AttackClicked(int idAttack)
    {
        Sequence mySequence = DOTween.Sequence();
        Vector3 endPosition;
        Attack tempAttack;
        Color a1Color, a2Color;

        if (BattleManager.turnOwnerId == 1 && !BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && !BattleManager.isPlayer2Human) return;
        endPosition = myCamera.WorldToScreenPoint(BattleManager.Instance.GetAttack(idAttack).GetPosition());
        if(isVisible) mySequence.Append(transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack));
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
        isVisible = true;
        GetComponentInChildren<AttackButtonController>().LastClickedAttack = idAttack;
        tempAttack = BattleManager.Instance.GetAttack(idAttack);
        attackNameText.text = tempAttack.GetName();
        ColorUtility.TryParseHtmlString(BattleManager.Army1Color, out a1Color);
        ColorUtility.TryParseHtmlString(BattleManager.Army2Color, out a2Color);
        attackText.text = "Attack Dice: ";
        defenceText.text = "Defence Dice: ";
        if (tempAttack.GetArmyId() == 1)
        {
            attackDiceNumberText.color = a1Color;
            attackText.color = a1Color;
            defenceDiceNumberText.color = a2Color;
            defenceText.color = a2Color;
        }
        else
        {
            attackDiceNumberText.color = a2Color;
            attackText.color = a2Color;
            defenceDiceNumberText.color = a1Color;
            defenceText.color = a1Color;
        }
        attackDiceNumberText.text = tempAttack.GetAttackDiceNumber().ToString();
        defenceDiceNumberText.text = tempAttack.GetDefenceDiceNumber().ToString();
    }

    private void UnitClicked(int idUnit)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
    }

    private void TileClicked(int idTile)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
    }

    private void AttackButtonPressed(int idAttack)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
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
        myCamera = Camera.main;
        isVisible = false;
        Text[] children = GetComponentsInChildren<Text>();
        foreach(Text child in children)
        {
            if (child.name == "AttackName") attackNameText = child;
        }
        attackDiceNumberText = transform.Find("AttackDiceNumber").GetComponent<Text>();
        defenceDiceNumberText = transform.Find("DefenceDiceNumber").GetComponent<Text>();
        attackText = transform.Find("AttackCaption").GetComponent<Text>();
        defenceText = transform.Find("DefenceCaption").GetComponent<Text>();
    }
}
