using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureController : MonoBehaviour
{
    [SerializeField] private Sprite cavalery;
    private Image myImage;

    private void UnitClicked(int unitId)
    {
        Unit tempUnit = null;
        tempUnit = BattleManager.Instance.GetUnit(unitId);
        switch (tempUnit.GetUnitType())
        {
            case "French Cavalery":
                myImage.sprite = cavalery;
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
