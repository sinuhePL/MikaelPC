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

        endPosition = myCamera.WorldToScreenPoint(BattleManager.Instance.GetAttack(idAttack).GetPosition());
        if(isVisible) mySequence.Append(transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack));
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
        isVisible = true;
        GetComponentInChildren<ActionButtonController>().LastClickedAttack = idAttack;
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

    private void ActionButtonPressed(int idAttack)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
    }

    private void DiceThrown(ThrowResult result)
    {
        GetComponentInChildren<ActionButtonController>().LastClickedAttack = 0;
        attackNameText.text = "Result";
        attackText.text = "Attacker:";
        attackDiceNumberText.text = "";
        if (result.defenderStrengthHits > 0) attackDiceNumberText.text = "Strength: -" + result.defenderStrengthHits.ToString();
        if(result.defenderMoraleHits > 0) attackDiceNumberText.text += " Morale: -"+result.defenderMoraleHits.ToString();
        defenceText.text = "Defender:";
        defenceDiceNumberText.text = "";
        if (result.attackerStrengthHits > 0) defenceDiceNumberText.text = "Strength: -" + result.attackerStrengthHits.ToString();
        if (result.attackerMoraleHits > 0) defenceDiceNumberText.text += " Morale: -" + result.attackerMoraleHits.ToString();
        transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack);
        isVisible = true;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onTileClicked -= TileClicked;
        EventManager.onActionButtonPressed -= ActionButtonPressed;
        EventManager.onDiceThrow -= DiceThrown;
    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onTileClicked += TileClicked;
        EventManager.onActionButtonPressed += ActionButtonPressed;
        EventManager.onDiceThrow += DiceThrown;
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
