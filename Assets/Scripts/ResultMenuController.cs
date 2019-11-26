using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultMenuController : MonoBehaviour
{
    private Text attackResultText;
    private Text defenceResultText;
    private Text attackText;
    private Text defenceText;

    // Start is called before the first frame update
    void Start()
    {
        attackResultText = transform.Find("AttackResult").GetComponent<Text>();
        defenceResultText = transform.Find("DefenceResult").GetComponent<Text>();
        attackText = transform.Find("AttackingArmyName").GetComponent<Text>();
        defenceText = transform.Find("DefendingArmyName").GetComponent<Text>();
    }

    private void DiceThrown(StateChange result)
    {
        Vector3 endPosition;
        Sequence mySequence = DOTween.Sequence();

        attackText.text = BattleManager.Instance.GetArmyName(result.attackerId) + ":";
        attackResultText.text = "";
        if (result.attackerStrengthChange != 0) attackResultText.text = "Strength: " + result.attackerStrengthChange.ToString();
        if (result.attackerMoraleChanged != 0) attackResultText.text += " Morale: " + result.attackerMoraleChanged.ToString();
        defenceText.text = BattleManager.Instance.GetArmyName(result.defenderId) + ":"; 
        defenceResultText.text = "";
        if (result.defenderStrengthChange != 0) defenceResultText.text = "Strength: " + result.defenderStrengthChange.ToString();
        if (result.defenderMoraleChanged != 0) defenceResultText.text += " Morale: " + result.defenderMoraleChanged.ToString();
        endPosition = new Vector3(Screen.width/2, Screen.height/2);
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= DiceThrown;
        EventManager.onResultMenuClosed -= AttackResultClosed;
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += DiceThrown;
        EventManager.onResultMenuClosed += AttackResultClosed;
    }

    public void AttackResultClosed()
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
    }
}
