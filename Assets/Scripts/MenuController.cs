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
        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(resumeButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mySequence.Append(transform.DOScale(0.0f, 0.25f).SetEase(Ease.InBack));
    }

    public void OpenMenuClicked()
    {
        transform.DOScale(1.0f, 0.25f).SetEase(Ease.OutBack);
    }

    public void SettingsClicked()
    {
        settingsButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
    }

    public void QuitClicked()
    {
        quitButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
    }
}
