using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndGameController : MonoBehaviour
{

    private Button quitButton;
    private int winnerId;
    [SerializeField] private Text winnerText;
    [SerializeField] private Image helmet;
    [SerializeField] private Image shield;
    [SerializeField] private Sprite hreHelmet;
    [SerializeField] private Sprite franceHelmet;
    [SerializeField] private Sprite hreShield;
    [SerializeField] private Sprite franceShield;

    // Start is called before the first frame update
    void Start()
    {
        quitButton = GetComponentInChildren<Button>();
        EventManager.onGameOver += SetWinnerId;
        EventManager.onResultMenuClosed += ShowEndGamePanel;
        winnerId = 0;
    }

    public void QuitClicked()
    {
        quitButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
    }

    private void ShowEndGamePanel(string mode)
    {
        if (winnerId != 0)
        {
            BattleManager.Instance.isInputBlocked = true;
            if (winnerId == 1)
            {
                winnerText.text = "Kingdom of France";
                helmet.sprite = franceHelmet;
                shield.sprite = franceShield;
            }
            else if (winnerId == 2)
            {
                winnerText.text = "Holy Roman Empire";
                helmet.sprite = hreHelmet;
                shield.sprite = hreShield;
            }
            transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack);
        }
    }

    private void SetWinnerId(int wId)
    {
        winnerId = wId;
    }

    private void OnDestroy()
    {
        EventManager.onGameOver -= SetWinnerId;
        EventManager.onResultMenuClosed -= ShowEndGamePanel;
    }
}
