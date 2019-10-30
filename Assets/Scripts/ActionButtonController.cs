using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    private int lastClickedAttack;

    public int LastClickedAttack { get => lastClickedAttack; set => lastClickedAttack = value; }

    public void ButtonPressed()
    {
        EventManager.RaiseEventOnActionButtonPressed(LastClickedAttack);
        if (LastClickedAttack > 0) BattleManager.Instance.MakeAttack(LastClickedAttack);
        else EventManager.RaiseEventUpdateBoard();
    }

    // Start is called before the first frame update
    private void Start()
    {
        LastClickedAttack = 0;
    }
}
