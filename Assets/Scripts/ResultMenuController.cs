using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        int attackerArmyId, defenderArmyId;
        Unit u;

        //SoundManagerController.Instance.SilenceMusic();
        if (result.defenderMoraleChanged + result.defenderStrengthChange < result.attackerStrengthChange + result.attackerMoraleChanged || result.keyFieldChangeId != 0) SoundManagerController.Instance.PlayResult("won");
        if (result.defenderMoraleChanged + result.defenderStrengthChange == result.attackerStrengthChange + result.attackerMoraleChanged) SoundManagerController.Instance.PlayResult("stalemate");
        if (result.defenderMoraleChanged + result.defenderStrengthChange > result.attackerStrengthChange + result.attackerMoraleChanged) SoundManagerController.Instance.PlayResult("lost");
        header.text = "Attack Result";
        Line2.gameObject.SetActive(true);
        defenceText.rectTransform.sizeDelta = new Vector2(386.0f, 71.0f);
        attackResultText.rectTransform.sizeDelta = new Vector2(226.0f, 96.0f);
        attackResultText.fontSize = 40;
        if (result.attackerId != 0) attackText.text = BattleManager.Instance.GetArmyName(result.attackerId) + ":";
        else attackText.text = "";
        Line2.gameObject.SetActive(true);
        attackResultText.text = "";
        attackerArmyId = BattleManager.Instance.turnOwnerId;
        if (BattleManager.Instance.turnOwnerId == 1)
        {
            defenderArmyId = 2;
        }
        else defenderArmyId = 1;
        attackerRoutText.text = "";
        if (result.attackerStrengthChange != 0)
        {
            attackResultText.text = "Strength: " + result.attackerStrengthChange.ToString();
            if (BattleManager.Instance.GetArmyMorale(attackerArmyId) <= 30) attackerRoutText.text = "Rout test imminent!";
        }
        if (result.attackerMoraleChanged != 0)
        {
            attackResultText.text += "\nMorale: " + result.attackerMoraleChanged.ToString();
            if(BattleManager.Instance.GetUnit(result.attackerId).morale + result.attackerMoraleChanged <= 0) attackerRoutText.text = "Rout test imminent!";
        }
        u = BattleManager.Instance.GetUnit(result.attackerId);
        if ((u.GetUnitType() == "Gendarmes" || u.GetUnitType() == "Imperial Cavalery") && result.defenderStrengthChange < 0)
        {
            attackResultText.text += "\n+1 morale for neighbours";
        }
        if (result.defenderId != 0) defenceText.text = BattleManager.Instance.GetArmyName(result.defenderId) + ":";
        else defenceText.text = "";
        defenceResultText.text = "";
        defenderRoutText.text = "";
        if (result.defenderStrengthChange != 0)
        {
            defenceResultText.text = "Strength: " + result.defenderStrengthChange.ToString();
            if (BattleManager.Instance.GetArmyMorale(defenderArmyId) <= 30) defenderRoutText.text = "Rout test imminent!";
        }
        if (result.defenderMoraleChanged != 0)
        {
            defenceResultText.text += "\nMorale: " + result.defenderMoraleChanged.ToString();
            if (BattleManager.Instance.GetUnit(result.defenderId).morale + result.defenderMoraleChanged <= 0) defenderRoutText.text = "Rout test imminent!";
        }
        if (result.defenderId != 0)
        {
            u = BattleManager.Instance.GetUnit(result.defenderId);
            if ((u.GetUnitType() == "Gendarmes" || u.GetUnitType() == "Imperial Cavalery") && result.attackerStrengthChange < 0)
            {
                defenceResultText.text += "\n+1 morale for neighbours";
            }
        }
        endPosition = new Vector3(Screen.width/2, Screen.height/2);
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(1.0f, 0.3f).SetEase(Ease.OutBack));
        attackerStarText.text = result.specialOutcomeDescription;
    }

    private void RouteTestOver(string resultDesription, int result, int morale)
    {
        Vector3 endPosition;
        Sequence mySequence2 = DOTween.Sequence();
        if (resultDesription != "noResult" && SceneManager.GetActiveScene().name != "Tutorial")
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
