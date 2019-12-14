using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptionController : MonoBehaviour
{
    [SerializeField] private Sprite gendarmes;
    [SerializeField] private Sprite landsknechte;
    private Image myImage;
    // Start is called before the first frame update

    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        tempUnit = BattleManager.Instance.GetUnit(unitId);
        switch(tempUnit.GetUnitType())
        {
            case "Gendarmes":
                myImage.sprite = gendarmes;
                break;
            case "Landsknechte":
                myImage.sprite = landsknechte;
                break;
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

    void Start()
    {
        myImage = GetComponent<Image>();
    }
}
