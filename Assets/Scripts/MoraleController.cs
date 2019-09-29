using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoraleController : MonoBehaviour
{
    private Text myText;

    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        tempUnit = BattleManager.Instance.GetUnit(unitId);
        myText.text = tempUnit.morale.ToString();
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
    }

    void Start()
    {
        myText = GetComponent<Text>();
        EventManager.onUnitClicked += UnitClicked;
    }
}
