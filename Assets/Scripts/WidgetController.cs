using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WidgetController : MonoBehaviour
{
    [SerializeField] private Sprite widgetArmy1;
    [SerializeField] private Sprite widgetArmy2;
    private Image myImage;
    
    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        if (!BattleManager.isInputBlocked && BattleManager.gameMode != "deploy")
        {
            tempUnit = BattleManager.Instance.GetUnit(unitId);
            if (tempUnit.GetArmyId() == 1) myImage.sprite = widgetArmy1;
            else myImage.sprite = widgetArmy2;
        }
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
    }

    private void OnEnable()
    {
        EventManager.onUnitClicked += UnitClicked;
    }

    public void InitalColor(int armyId)
    {
        if(armyId == 1 ) myImage.sprite = widgetArmy1;
        else myImage.sprite = widgetArmy2;
    }

    void Awake()
    {
        myImage = GetComponent<Image>();
    }
}
