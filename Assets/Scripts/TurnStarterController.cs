using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TurnStarterController : MonoBehaviour
{
    private Image helmetImage;
    private Image shieldImage;
    private Image captionImage;
    private Text turnCounterText;
    private bool isGameEnded;
    private int turnCounter;
    [SerializeField] private Sprite hreHelmet;
    [SerializeField] private Sprite franceHelmet;
    [SerializeField] private Sprite hreShield;
    [SerializeField] private Sprite franceShield;
    [SerializeField] private Sprite hreCaption;
    [SerializeField] private Sprite franceCaption;
    [SerializeField] private Sprite hreDeployCaption;
    [SerializeField] private Sprite franceDeployCaption;


    private void OnEnable()
    {
        EventManager.onTurnEnd += TurnEnd;
        EventManager.onGameStart += TurnEnd;
        EventManager.onGameOver += GameEnded;
        EventManager.onDeploymentStart += StartDeployment;
        turnCounter = 0;
        turnCounterText = GetComponentInChildren<Text>();
        helmetImage = transform.Find("helmet").GetComponent<Image>();
        shieldImage = transform.Find("shield").GetComponent<Image>();
        captionImage = transform.Find("caption").GetComponent<Image>();
        turnCounterText.text = turnCounter.ToString();
        turnCounterText.gameObject.SetActive(false);
        ChangeImages(BattleManager.Instance.turnOwnerId);
        isGameEnded = false;
    }

    private void OnDestroy()
    {
        EventManager.onTurnEnd -= TurnEnd;
        EventManager.onGameStart -= TurnEnd;
        EventManager.onGameOver -= GameEnded;
        EventManager.onDeploymentStart -= StartDeployment;
    }

    void ChangeImages(int armyId)
    {
        if (armyId == 1)
        {
            helmetImage.sprite = franceHelmet;
            shieldImage.sprite = franceShield;
            if (BattleManager.Instance.gameMode == "deploy") captionImage.sprite = franceDeployCaption;
            else captionImage.sprite = franceCaption;
        }
        else if (armyId == 2)
        {
            helmetImage.sprite = hreHelmet;
            shieldImage.sprite = hreShield;
            if (BattleManager.Instance.gameMode == "deploy") captionImage.sprite = hreDeployCaption;
            else captionImage.sprite = hreCaption;
        }
    }

    private void GameEnded(int winnerId)
    {
        isGameEnded = true;
    }

    private void StartDeployment(int aId)
    {
        if (aId < 3) TurnEnd();
    }

    private void TurnEnd()
    {
        turnCounterText.gameObject.SetActive(true);
        if (!isGameEnded)
        {
            ChangeImages(BattleManager.Instance.turnOwnerId);
            if (BattleManager.Instance.gameMode != "deploy")
            {
                if (BattleManager.Instance.turnOwnerId == 1) turnCounter++;
                turnCounterText.text = turnCounter.ToString();
            }
            Sequence turnStarterControllerSequence = DOTween.Sequence();
            turnStarterControllerSequence.Insert(0.3f, helmetImage.transform.DOLocalMoveY(174.0f, 0.2f));
            turnStarterControllerSequence.Insert(0.3f, captionImage.transform.DOLocalMoveY(-116.0f, 0.2f));
            if (BattleManager.Instance.gameMode != "deploy") turnStarterControllerSequence.Insert(0.3f, turnCounterText.transform.DOLocalMoveY(-96.0f, 0.2f));
            turnStarterControllerSequence.Insert(0.3f, shieldImage.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack));
            turnStarterControllerSequence.InsertCallback(0.3f, () => { SoundManagerController.Instance.PlayStartTurn(); });
            turnStarterControllerSequence.AppendInterval(2.0f);
            turnStarterControllerSequence.Insert(2.5f, helmetImage.transform.DOLocalMoveY(689.0f, 0.2f));
            turnStarterControllerSequence.Insert(2.5f, captionImage.transform.DOLocalMoveY(-640.0f, 0.2f));
            if (BattleManager.Instance.gameMode != "deploy") turnStarterControllerSequence.Insert(2.5f, turnCounterText.transform.DOLocalMoveY(-620.0f, 0.2f));
            turnStarterControllerSequence.Insert(2.5f, shieldImage.transform.DOScale(0.0f, 0.2f));
            if (BattleManager.Instance.gameMode != "deploy") turnStarterControllerSequence.AppendCallback(() => { EventManager.RaiseEventOnTurnStart(); });
        }
    }
}
