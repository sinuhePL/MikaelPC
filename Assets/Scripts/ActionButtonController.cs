using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonController : MonoBehaviour
{
    private Text myText;
    int lastClickedAttack;

    private void AttackClicked(int attackId)
    {
        myText.text = "Attack!";
        lastClickedAttack = attackId;
    }

    private void UnitClicked(int unitId)
    {
        myText.text = "End Turn";
        lastClickedAttack = -1;
    }

    private void TileClicked(int tileId)
    {
        myText.text = "End Turn";
        lastClickedAttack = -1;
    }

    public void ButtonPressed()
    {
        EventManager.RaiseEventOnActionButtonPressed(lastClickedAttack);
        if(lastClickedAttack > 0)
        {
            BattleManager.Instance.MakeAttack(lastClickedAttack);
        }
    }   

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponentInChildren<Text>();
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onTileClicked += TileClicked;
        lastClickedAttack = -1;
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onTileClicked -= TileClicked;
    }
}
