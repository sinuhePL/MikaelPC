using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToggle : MonoBehaviour
{
    private Toggle myToggle = null;

    public enum playerToggleEnum { franceHuman, franceAi, hreHuman, hreAi };
    [SerializeField] private playerToggleEnum enumType;
    [SerializeField] private GameObject NewGamePanel;
    private NewGameController myNewGameController;

    void Awake()
    {
        myToggle = GetComponent<Toggle>();
        myNewGameController = NewGamePanel.GetComponent<NewGameController>();
    }

    private void Start()
    {
        if (enumType == playerToggleEnum.franceHuman)
        {
            if (GameManagerController.Instance.isPlayer1Human) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (enumType == playerToggleEnum.franceAi)
        {
            if (GameManagerController.Instance.isPlayer1Human) myToggle.isOn = false;
            else myToggle.isOn = true;
        }
        else if (enumType == playerToggleEnum.hreHuman)
        {
            if (GameManagerController.Instance.isPlayer2Human) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (enumType == playerToggleEnum.hreAi)
        {
            if (GameManagerController.Instance.isPlayer2Human) myToggle.isOn = false;
            else myToggle.isOn = true;
        }
    }

    public void ToggleClicked()
    {
        if (myToggle != null && myToggle.isOn)
        {
            if (enumType == playerToggleEnum.franceHuman)
            {
                GameManagerController.Instance.isPlayer1Human = true;
                if (GameManagerController.Instance.isPlayer2Human) myNewGameController.DisplayDifficultyLevel(false);
            }
            else if (enumType == playerToggleEnum.franceAi)
            {
                GameManagerController.Instance.isPlayer1Human = false;
                myNewGameController.DisplayDifficultyLevel(true);
            }
            else if (enumType == playerToggleEnum.hreHuman)
            {
                GameManagerController.Instance.isPlayer2Human = true;
                if (GameManagerController.Instance.isPlayer1Human) myNewGameController.DisplayDifficultyLevel(false);
            }
            else if (enumType == playerToggleEnum.hreAi)
            {
                GameManagerController.Instance.isPlayer2Human = false;
                myNewGameController.DisplayDifficultyLevel(true);
            }
        }
    }
}
