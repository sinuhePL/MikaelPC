using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackMenuController : MonoBehaviour
{
    private bool isVisible;
    public Text attackerText;
    public Image die1Image;
    public Image starImage;
    public Text colonText;
    public Text defenderText;
    public Image die2Image;
    public GameObject dummy;
    public GameObject dummy2;
    private Text attackNameText;
    private Text attackDiceNumberText;
    private Text starText;
    private Text defenceDiceNumberText;
    private int lastClickedUnitId;

    private void AttackClicked(int idArrow, bool isCounterAttack)
    {
        Sequence mySequence = DOTween.Sequence();
        Attack tempAttack;
        List<Attack> tempAttacks;
        string an;

        if (BattleManager.Instance.turnOwnerId == 1 && !GameManagerController.Instance.isPlayer1Human || BattleManager.Instance.turnOwnerId == 2 && !GameManagerController.Instance.isPlayer2Human) return;
        if (isVisible)  // hide previous attack
        {
            //mySequence.Append(transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack));
            mySequence.Append(transform.DOMoveX(dummy.transform.position.x, 0.3f).SetEase(Ease.InBack));
            mySequence.Join(transform.DOScale(0.1f, 0.3f).SetEase(Ease.InBack));
        }
        tempAttacks = BattleManager.Instance.GetAttacksByArrowId(idArrow);
        tempAttack = null;
        foreach(Attack a in tempAttacks)
        {
            if (a.IsActive()) tempAttack = a;
        }
        if (tempAttack == null) return;
        mySequence.Append(transform.DOMoveX(510.0f, 0.3f).SetEase(Ease.OutBack));
        mySequence.Join(transform.DOScale(1.1f, 0.3f).SetEase(Ease.OutBack));
        isVisible = true;
        an = tempAttack.GetName();
        attackNameText.text = an;
        if (an == "Aim")
        {
            attackerText.text = "Enables attack in next turn";
            attackerText.alignment = TextAnchor.UpperCenter;
            attackerText.fontSize = 40;
            colonText.text = "";
            defenderText.text = "";
            die1Image.enabled = false;
            die2Image.enabled = false;
            starImage.enabled = false;
            attackDiceNumberText.text = "";
            starText.text = "";
            defenceDiceNumberText.text = "";
        }
        else if (an == "Capture")
        {
            attackerText.text = tempAttack.GetSpecialOutcomeDescription();
            attackerText.alignment = TextAnchor.UpperCenter;
            attackerText.fontSize = 25;
            colonText.text = "";
            defenderText.text = "";
            die1Image.enabled = false;
            die2Image.enabled = false;
            starImage.enabled = false;
            attackDiceNumberText.text = "";
            starText.text = "";
            defenceDiceNumberText.text = "";
        }
        else if(an == "Move")
        {
            attackerText.text = "Move Rear Guard to battlefield";
            attackerText.alignment = TextAnchor.UpperCenter;
            attackerText.fontSize = 25;
            colonText.text = "";
            defenderText.text = "";
            die1Image.enabled = false;
            die2Image.enabled = false;
            starImage.enabled = false;
            attackDiceNumberText.text = "";
            starText.text = "";
            defenceDiceNumberText.text = "";
        }
        else
        {
            attackerText.text = "Attacker:";
            attackerText.alignment = TextAnchor.UpperLeft;
            attackerText.fontSize = 40;
            colonText.text = ":";
            defenderText.text = "Defender:";
            die1Image.enabled = true;
            die2Image.enabled = true;
            starImage.enabled = true;
            attackDiceNumberText.text = tempAttack.GetAttackDiceNumber().ToString();
            starText.text = tempAttack.GetSpecialOutcomeDescription();
            defenceDiceNumberText.text = tempAttack.GetDefenceDiceNumber().ToString();
        }
    }

    private void UnitClicked(int idUnit)
    {
        if (isVisible && lastClickedUnitId != idUnit)
        {
            //transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOMoveX(dummy.transform.position.x, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
        else if(isVisible && lastClickedUnitId == idUnit)
        {
            //transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOMoveX(dummy2.transform.position.x, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
        lastClickedUnitId = idUnit;
    }

    private void TileClicked(int idTile)
    {
        if (isVisible)
        {
            //transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOMoveX(dummy2.transform.position.x, 0.3f).SetEase(Ease.InBack);
            transform.DOScale(0.65f, 0.3f).SetEase(Ease.InBack);
            isVisible = false;
        }
    }

    private void AttackButtonPressed(int idAttack)
    {
        if (isVisible)
        {
            //transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.InBack);
            transform.DOMoveX(dummy.transform.position.x, 0.3f).SetEase(Ease.InBack);
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
