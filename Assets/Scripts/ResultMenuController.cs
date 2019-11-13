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

    private void DiceThrown(ThrowResult result)
    {
        attackText.text = "Attacker:";
        attackResultText.text = "";
        if (result.defenderStrengthHits > 0) attackResultText.text = "Strength: -" + result.defenderStrengthHits.ToString();
        if (result.defenderMoraleHits > 0) attackResultText.text += " Morale: -" + result.defenderMoraleHits.ToString();
        defenceText.text = "Defender:";
        defenceResultText.text = "";
        if (result.attackerStrengthHits > 0) defenceResultText.text = "Strength: -" + result.attackerStrengthHits.ToString();
        if (result.attackerMoraleHits > 0) defenceResultText.text += " Morale: -" + result.attackerMoraleHits.ToString();
        transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack);
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= DiceThrown;
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += DiceThrown;
    }

    public void AttackResultClosed()
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
    }
}
