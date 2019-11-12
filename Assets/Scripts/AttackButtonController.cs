using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonController : MonoBehaviour
{
    private int lastClickedAttack;

    public int LastClickedAttack { get => lastClickedAttack; set => lastClickedAttack = value; }

    public void ButtonPressed()
    {
        EventManager.RaiseEventOnAttackOrdered(LastClickedAttack);
        BattleManager.Instance.MakeAttack(LastClickedAttack);   
    }


    // Start is called before the first frame update
    void Start()
    {
        LastClickedAttack = 0;
    }
}
