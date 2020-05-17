using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewGameController : MonoBehaviour
{
    private Button backButton;
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
        }
        difficultyText.gameObject.SetActive(false);
        easyToggle.gameObject.SetActive(false);
        mediumToggle.gameObject.SetActive(false);
        hardToggle.gameObject.SetActive(false);
    }

    public void BackClicked()
    {
        Sequence mmSequence2 = DOTween.Sequence();
        mmSequence2.Append(backButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mmSequence2.Append(transform.DOScale(0.0f, 0.0f));
        mmSequence2.Append(menuPanel.transform.DOScale(1.0f, 0.0f));
    }

    public void DisplayDifficultyLevel(bool condition)
    {
        difficultyText.gameObject.SetActive(condition);
        easyToggle.gameObject.SetActive(condition);
        mediumToggle.gameObject.SetActive(condition);
        hardToggle.gameObject.SetActive(condition);
    }
}
