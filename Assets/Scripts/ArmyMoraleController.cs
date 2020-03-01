using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmyMoraleController : MonoBehaviour
{
    private Text myText;
    [SerializeField] private int myArmyId;

    private void ResultClosed(string mode)
    {
        if (mode == "attack")
        {
            UpdateMe();
        }
    }

    private void UpdateMe()
    {
        int m;
        m = BattleManager.Instance.GetArmyMorale(myArmyId);
        myText.text = m.ToString();
    }

    private void OnEnable()
    {
        EventManager.onResultMenuClosed += ResultClosed;
        EventManager.onGameStart += UpdateMe;
        myText = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnDestroy()
    {
        EventManager.onResultMenuClosed -= ResultClosed;
        EventManager.onGameStart -= UpdateMe;
    }
}
