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
    [SerializeField] private Sprite arquebusiers;
    [SerializeField] private Sprite artillery;
    [SerializeField] private Sprite stradioti;
    [SerializeField] private Sprite coustilliers;
    private Image myImage;
    // Start is called before the first frame update

    private void UnitClicked(int unitId)
    {
        Unit tempUnit;
        if (!BattleManager.Instance.isInputBlocked && BattleManager.Instance.gameMode != "deploy")
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
                case "Arquebusiers":
                    myImage.sprite = arquebusiers;
                    break;
                case "Artillery":
                    myImage.sprite = artillery;
                    break;
                case "Stradioti":
                    myImage.sprite = stradioti;
                    break;
                case "Coustilliers":
                    myImage.sprite = coustilliers;
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

    public void InitialCaption(string unitType)
    {
        switch (unitType)
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
            case "Arquebusiers":
                myImage.sprite = arquebusiers;
                break;
            case "Artillery":
                myImage.sprite = artillery;
                break;
            case "Stradioti":
                myImage.sprite = stradioti;
                break;
            case "Coustilliers":
                myImage.sprite = coustilliers;
                break;
        }
    }

    void Awake()
    {
        myImage = GetComponent<Image>();
    }
}
