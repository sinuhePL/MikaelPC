using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptionController : MonoBehaviour
{
    [SerializeField] private Sprite gendarmes;
    [SerializeField] private Sprite landsknechte;
    [SerializeField] private Sprite suisse;
    [SerializeField] private Sprite manatarms;
    private Image myImage;
    // Start is called before the first frame update

    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        if (!BattleManager.isInputBlocked)
        {
            tempUnit = BattleManager.Instance.GetUnit(unitId);
            switch (tempUnit.GetUnitType())
            {
                case "Gendarmes":
                    myImage.sprite = gendarmes;
                    break;
                case "Landsknechte":
                    myImage.sprite = landsknechte;
                    break;
                case "Suisse":
                    myImage.sprite = suisse;
                    break;
                case "Imperial Cavalery":
                    myImage.sprite = manatarms;
                    break;
            }
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
