using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptionController : MonoBehaviour
{
    [SerializeField] private Sprite frenchCavalery;
    private Image myImage;
    // Start is called before the first frame update

    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        tempUnit = BattleManager.Instance.GetUnit(unitId);
        switch(tempUnit.GetUnitType())
        {
            case "French Cavalery":
                myImage.sprite = frenchCavalery;
                break;
        }
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
    }

    void Start()
    {
        myImage = GetComponent<Image>();
        EventManager.onUnitClicked += UnitClicked;
    }
}
