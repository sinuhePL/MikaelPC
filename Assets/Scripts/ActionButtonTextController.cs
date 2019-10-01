using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonTextController : MonoBehaviour
{
    private Text myText;

    private void AttackClicked(int attackId)
    {
        myText.text = "Attack!";
    }

    private void UnitClicked(int unitId)
    {
        myText.text = "End Turn";
    }

    private void TileClicked(int tileId)
    {
        myText.text = "End Turn";
    }

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<Text>();
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onTileClicked += TileClicked;
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onTileClicked -= TileClicked;
    }
}
