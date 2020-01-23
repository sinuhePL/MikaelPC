using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    private Toggle myToggle = null;

    public enum difficultyEnum { easy, medium, hard};
    [SerializeField] private difficultyEnum difficultyType;

    // Start is called before the first frame update
    void Awake()
    {
        myToggle = GetComponent<Toggle>();
    }

    public void ToggleChanged()
    {
        if(myToggle != null && myToggle.isOn)
        {
            if (difficultyType == difficultyEnum.easy) BattleManager.minimaxLimit = 10.0f;
            else if (difficultyType == difficultyEnum.medium) BattleManager.minimaxLimit = 20.0f;
            else if (difficultyType == difficultyEnum.hard) BattleManager.minimaxLimit = 30.0f;
        }
    }
}
