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

    public void ToggleClicked()
    {
        if (myToggle != null && myToggle.isOn)
        {
            if (enumType == playerToggleEnum.franceHuman)
            {
                GameManagerController.isPlayer1Human = true;
                if (GameManagerController.isPlayer2Human) myNewGameController.DisplayDifficultyLevel(false);
            }
            else if (enumType == playerToggleEnum.franceAi)
            {
                GameManagerController.isPlayer1Human = false;
                myNewGameController.DisplayDifficultyLevel(true);
            }
            else if (enumType == playerToggleEnum.hreHuman)
            {
                GameManagerController.isPlayer2Human = true;
                if (GameManagerController.isPlayer1Human) myNewGameController.DisplayDifficultyLevel(false);
            }
            else if (enumType == playerToggleEnum.hreAi)
            {
                GameManagerController.isPlayer2Human = false;
                myNewGameController.DisplayDifficultyLevel(true);
            }
        }
    }
}
