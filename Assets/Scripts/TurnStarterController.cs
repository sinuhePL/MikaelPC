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


    private void OnEnable()
    {
        EventManager.onTurnEnd += TurnEnd;
        EventManager.onGameStart += TurnEnd;
        EventManager.onGameOver += GameEnded;
        turnCounter = 0;
        turnCounterText = GetComponentInChildren<Text>();
        helmetImage = transform.Find("helmet").GetComponent<Image>();
        shieldImage = transform.Find("shield").GetComponent<Image>();
        captionImage = transform.Find("caption").GetComponent<Image>();
        turnCounterText.text = turnCounter.ToString();
        ChangeImages(BattleManager.turnOwnerId);
        isGameEnded = false;
    }

    private void OnDestroy()
    {
        EventManager.onTurnEnd -= TurnEnd;
        EventManager.onGameStart -= TurnEnd;
        EventManager.onGameOver -= GameEnded;
    }

    void ChangeImages(int armyId)
    {
        if (armyId == 1)
        {
            helmetImage.sprite = franceHelmet;
            shieldImage.sprite = franceShield;
            captionImage.sprite = franceCaption;
        }
        else if (armyId == 2)
        {
            helmetImage.sprite = hreHelmet;
            shieldImage.sprite = hreShield;
            captionImage.sprite = hreCaption;
        }
    }

    private void GameEnded(int winnerId)
    {
        isGameEnded = true;
    }

    private void TurnEnd()
    {
        if (!isGameEnded)
        {
            ChangeImages(BattleManager.turnOwnerId);
            if (BattleManager.turnOwnerId == 1) turnCounter++;
            turnCounterText.text = turnCounter.ToString();
            Sequence turnStarterControllerSequence = DOTween.Sequence();
            turnStarterControllerSequence.Insert(0.3f, helmetImage.transform.DOLocalMoveY(124.0f, 0.2f));
            turnStarterControllerSequence.Insert(0.3f, captionImage.transform.DOLocalMoveY(-184.1f, 0.2f));
            turnStarterControllerSequence.Insert(0.3f, turnCounterText.transform.DOLocalMoveY(-164.39f, 0.2f));
            turnStarterControllerSequence.Insert(0.3f, shieldImage.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutBack));
            turnStarterControllerSequence.InsertCallback(0.3f, () => { SoundManagerController.Instance.PlayStartTurn(); });
            turnStarterControllerSequence.AppendInterval(1.7f);
            turnStarterControllerSequence.Insert(2.2f, helmetImage.transform.DOLocalMoveY(714.0f, 0.2f));
            turnStarterControllerSequence.Insert(2.2f, captionImage.transform.DOLocalMoveY(-609.0f, 0.2f));
            turnStarterControllerSequence.Insert(2.2f, turnCounterText.transform.DOLocalMoveY(-589.0f, 0.2f));
            turnStarterControllerSequence.Insert(2.2f, shieldImage.transform.DOScale(0.0f, 0.2f));
            turnStarterControllerSequence.AppendCallback(() => { EventManager.RaiseEventOnTurnStart(); });
        }
    }
}
