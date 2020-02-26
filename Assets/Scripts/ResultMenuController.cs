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

    private void RouteTestOver(int loserId)
    {
        Vector3 endPosition;
        Sequence mySequence2 = DOTween.Sequence();
        if (loserId > 0)
        {
            attackText.text = "Route test result:";
            if (loserId == 1) attackResultText.text = "French army flees!";
            else if (loserId == 2) attackResultText.text = "Imperial army flees!";
            else if (loserId == 3) attackResultText.text = "French army stands its ground!";
            else if (loserId == 4) attackResultText.text = "Imperial army stands its ground!";
            defenceText.text = "";
            defenceResultText.text = "";
            endPosition = new Vector3(Screen.width / 2, Screen.height / 2);
            mySequence2.Append(transform.DOMove(endPosition, 0.01f));
            mySequence2.Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
        }
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= DiceThrown;
        EventManager.onResultMenuClosed -= AttackResultClosed;
        EventManager.onRouteTestOver -= RouteTestOver;
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += DiceThrown;
        EventManager.onResultMenuClosed += AttackResultClosed;
        EventManager.onRouteTestOver += RouteTestOver;
    }

    public void AttackResultClosed()
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
    }
}
