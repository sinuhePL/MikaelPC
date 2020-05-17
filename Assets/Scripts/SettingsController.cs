using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsController : MonoBehaviour
{
    private Button backButton;
    [SerializeField] private RectTransform menuPanel;


    // Start is called before the first frame update
    void Start()
    {
        Button[] myButtons = GetComponentsInChildren<Button>();
        foreach (Button b in myButtons)
        {
            if (b.name == "backButton") backButton = b;
        }
    }

    public void BackClicked()
    {
        Sequence mySequence3 = DOTween.Sequence();
        mySequence3.Append(backButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
        mySequence3.Append(transform.DOScale(0.0f, 0.0f));
        mySequence3.Append(menuPanel.transform.DOScale(1.0f, 0.0f));
        //BattleManager.isInputBlocked = false;
    }
}
