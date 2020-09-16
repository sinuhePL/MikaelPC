using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private RectTransform newGamePanel;
    [SerializeField] private RectTransform tutorialPanel;
    [SerializeField] private RectTransform settingsPanel;
    [SerializeField] private RectTransform creditsPanel;
    [SerializeField] private RectTransform quitPanel;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button tutorialButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;

    public void ButtonClicked(string clickedButton)
    {
        Sequence mmSequence1 = DOTween.Sequence();
        if (clickedButton == "newgame") mmSequence1.Append(newGameButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        if (clickedButton == "tutorial") mmSequence1.Append(tutorialButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        if (clickedButton == "settings") mmSequence1.Append(settingsButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        if (clickedButton == "credits") mmSequence1.Append(creditsButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        if (clickedButton == "quit") mmSequence1.Append(quitButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mmSequence1.Append(transform.DOScale(0.0f, 0.0f));
        if (clickedButton == "newgame") mmSequence1.Append(newGamePanel.transform.DOScale(1.0f, 0.0f));
        if (clickedButton == "tutorial") mmSequence1.Append(tutorialPanel.transform.DOScale(1.0f, 0.0f));
        if (clickedButton == "settings") mmSequence1.Append(settingsPanel.transform.DOScale(1.0f, 0.0f));
        if (clickedButton == "credits") mmSequence1.Append(creditsPanel.transform.DOScale(1.1f, 0.0f));
        if (clickedButton == "quit") mmSequence1.Append(quitPanel.transform.DOScale(0.7f, 0.0f));
    }
}
