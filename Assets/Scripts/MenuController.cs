using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    private Button resumeButton;
    private Button settingsButton;
    private Button quitButton;
    [SerializeField] private RectTransform settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        Button[] myButtons = GetComponentsInChildren<Button>();
        foreach(Button b in myButtons)
        {
            if (b.name == "ResumeButton") resumeButton = b;
            if (b.name == "SettingsButton") settingsButton = b;
            if (b.name == "QuitButton") quitButton = b;
        }
    }

    public void ResumeClicked()
    {
        SoundManagerController.Instance.PlayClick();
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(resumeButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mySequence.Append(transform.DOScale(0.0f, 0.25f).SetEase(Ease.InBack));
        BattleManager.Instance.isInputBlocked = false;
    }

    public void OpenMenuClicked()
    {
        transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack);
    }

    public void SettingsClicked()
    {
        SoundManagerController.Instance.PlayClick();
        Sequence mySequence2 = DOTween.Sequence();
        mySequence2.Append(settingsButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mySequence2.Append(transform.DOScale(0.0f, 0.0f));
        mySequence2.Append(settingsPanel.transform.DOScale(1.0f, 0.0f));
    }

    public void QuitClicked()
    {
        SoundManagerController.Instance.PlayClick();
        quitButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
        GameManagerController.Instance.LoadLevel("MenuScene");
    }
}
