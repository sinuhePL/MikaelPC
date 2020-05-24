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
        Unit tempUnit = null;
        if (unitId > 0 && BattleManager.Instance.gameMode != "deploy")
        {
            tempUnit = BattleManager.Instance.GetUnit(unitId);
            if (tempUnit == null) return;
            if (isStrength) myText.text = tempUnit.strength.ToString();
            else myText.text = tempUnit.morale.ToString();
            presentedUnitId = unitId;
        }
    }

    private void UpdateMe(string mode)
    {
        UnitClicked(presentedUnitId);
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onResultMenuClosed -= UpdateMe;
    }

    private void OnEnable()
    {
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onResultMenuClosed += UpdateMe;
    }

    public void InitialSM(int iStrength, int iMorale)
    {
        if (isStrength) myText.text = iStrength.ToString();
        else myText.text = iMorale.ToString();
    }

    void Awake()
    {
        myText = GetComponent<Text>();
        presentedUnitId = 0;
    }
}
