using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SMController : MonoBehaviour
{
    private Text myText;
    private int presentedUnitId;
    [SerializeField] private bool isStrength;

    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        if (unitId > 0)
        {
            tempUnit = BattleManager.Instance.GetUnit(unitId);
            if (isStrength) myText.text = tempUnit.strength.ToString();
            else myText.text = tempUnit.morale.ToString();
            presentedUnitId = unitId;
        }
    }

    private void UpdateMe()
    {
        UnitClicked(presentedUnitId);
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onUpdateBoard -= UpdateMe;
    }

    private void OnEnable()
    {
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onUpdateBoard += UpdateMe;
    }

    void Start()
    {
        myText = GetComponent<Text>();
        presentedUnitId = 0;
    }
}
