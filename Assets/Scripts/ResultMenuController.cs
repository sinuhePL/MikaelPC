using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResultMenuController : MonoBehaviour
{
    private Text header;
    private Text attackResultText;
    private Text defenceResultText;
    private Text attackText;
    private Text defenceText;
    private Text attackerRoutText;
    private Text defenderRoutText;
    private Text attackerStarText;
    private Image Line2;

    // Start is called before the first frame update
    void Start()
    {
        header = transform.Find("Header").GetComponent<Text>();
        attackResultText = transform.Find("AttackResult").GetComponent<Text>();
        defenceResultText = transform.Find("DefenceResult").GetComponent<Text>();
        attackText = transform.Find("AttackingArmyName").GetComponent<Text>();
        defenceText = transform.Find("DefendingArmyName").GetComponent<Text>();
        attackerRoutText = transform.Find("AttackerRoutMessage").GetComponent<Text>();
        defenderRoutText = transform.Find("DefenderRoutMessage").GetComponent<Text>();
        attackerStarText = transform.Find("AttackerStarMessage").GetComponent<Text>();
        Line2 = transform.Find("Line 2").GetComponent<Image>();
    }

    private void DiceThrown(StateChange result)
    {
        Vector3 endPosition;
        Sequence mySequence = DOTween.Sequence();

        header.text = "Attack Result";
        Line2.gameObject.SetActive(true);
        defenceText.rectTransform.sizeDelta = new Vector2(386.0f, 71.0f);
        attackResultText.rectTransform.sizeDelta = new Vector2(226.0f, 96.0f);
        attackResultText.fontSize = 40;
        attackText.text = BattleManager.Instance.GetArmyName(result.attackerId) + ":";
        Line2.gameObject.SetActive(true);
        attackResultText.text = "";
        if (result.attackerStrengthChange != 0)
        {
            attackResultText.text = "Strength: " + result.attackerStrengthChange.ToString();
            attackerRoutText.text = "Rout test imminent!";
        }
        else attackerRoutText.text = "";
        if (result.attackerMoraleChanged != 0) attackResultText.text += " Morale: " + result.attackerMoraleChanged.ToString();
        defenceText.text = BattleManager.Instance.GetArmyName(result.defenderId) + ":"; 
        defenceResultText.text = "";
        if (result.defenderStrengthChange != 0)
        {
            defenceResultText.text = "Strength: " + result.defenderStrengthChange.ToString();
            defenderRoutText.text = "Rout test imminent!";
        }
        else defenderRoutText.text = "";
        if (result.defenderMoraleChanged != 0) defenceResultText.text += " Morale: " + result.defenderMoraleChanged.ToString();
        endPosition = new Vector3(Screen.width/2, Screen.height/2);
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(0.7f, 0.3f).SetEase(Ease.OutBack));
        attackerStarText.text = result.specialOutcomeDescription;
    }

    private void RouteTestOver(string resultDesription, int result, int morale)
    {
        Vector3 endPosition;
        Sequence mySequence2 = DOTween.Sequence();
        if (resultDesription != "noResult")
        {
            header.text = "Rout Test Result";
            Line2.gameObject.SetActive(false);
            attackResultText.rectTransform.sizeDelta = new Vector2(386.0f, 142.0f);
            attackResultText.fontSize = 55;
            attackResultText.text = "Test result: " + result.ToString() + " Army morale: " + morale.ToString();
            defenceText.rectTransform.sizeDelta = new Vector2(386.0f, 142.0f);
            if (resultDesription == "frenchFlee") defenceText.text = "French army flees!";
            else if (resultDesription == "imperialFlee") defenceText.text = "Imperial army flees!";
            else if (resultDesription == "frenchStays") defenceText.text = "French army stands its ground!";
            else if (resultDesription == "imperialStays") defenceText.text = "Imperial army stands its ground!";
            attackText.text = "";
            defenceResultText.text = "";
            attackerStarText.text = "";
            attackerRoutText.text = "";
            defenderRoutText.text = "";
            endPosition = new Vector3(Screen.width / 2, Screen.height / 2);
            mySequence2.Append(transform.DOMove(endPosition, 0.01f));
            mySequence2.Append(transform.DOScale(0.7f, 0.3f).SetEase(Ease.OutBack));
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

    public void AttackResultClosed(string mode)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
    }
}
