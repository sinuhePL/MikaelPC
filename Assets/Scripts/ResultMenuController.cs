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
        attackText = transform.Find("AttackCaption").GetComponent<Text>();
        defenceText = transform.Find("DefenceCaption").GetComponent<Text>();
    }

    private void DiceThrown(StateChange result)
    {
        attackText.text = "Attacker:";
        attackResultText.text = "";
        if (result.attackerStrengthChange != 0) attackResultText.text = "Strength: " + result.attackerStrengthChange.ToString();
        if (result.attackerMoraleChanged != 0) attackResultText.text += " Morale: " + result.attackerMoraleChanged.ToString();
        defenceText.text = "Defender:";
        defenceResultText.text = "";
        if (result.defenderStrengthChange != 0) defenceResultText.text = "Strength: " + result.defenderStrengthChange.ToString();
        if (result.defenderMoraleChanged != 0) defenceResultText.text += " Morale: " + result.defenderMoraleChanged.ToString();
        transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack);
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
