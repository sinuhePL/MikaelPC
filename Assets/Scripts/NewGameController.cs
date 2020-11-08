using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewGameController : MonoBehaviour
{
    private Button backButton;
    private Button startBattleButton;
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private RectTransform difficultyText;
    [SerializeField] private RectTransform easyToggle;
    [SerializeField] private RectTransform mediumToggle;
    [SerializeField] private RectTransform hardToggle;

    // Start is called before the first frame update
    void Start()
    {
        Button[] myButtons = GetComponentsInChildren<Button>();
        foreach (Button b in myButtons)
        {
            if (b.name == "backButton") backButton = b;
            if (b.name == "startBattleButton") startBattleButton = b;
        }
        DisplayDifficultyLevel(false);
    }

    public void BackClicked()
    {
        SoundManagerController.Instance.PlayClick();
        Sequence mmSequence2 = DOTween.Sequence();
        mmSequence2.Append(backButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mmSequence2.Append(transform.DOScale(0.0f, 0.0f));
        mmSequence2.Append(menuPanel.transform.DOScale(1.0f, 0.0f));
    }

    public void StartBattleClicked()
    {
        SoundManagerController.Instance.PlayClick();
        startBattleButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20).OnComplete(() => GameManagerController.Instance.LoadLevel("BattleScene")); ;
    }

    public void DisplayDifficultyLevel(bool show)
    {
        if (!show)
        {
            difficultyText.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            easyToggle.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            mediumToggle.localScale = new Vector3(0.0f, 0.0f, 0.0f);
            hardToggle.localScale = new Vector3(0.0f, 0.0f, 0.0f);
        }
        else
        {
            difficultyText.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            easyToggle.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            mediumToggle.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            hardToggle.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
}
