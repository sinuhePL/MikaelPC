using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureController : MonoBehaviour
{
    [SerializeField] private Sprite heavyCavalery;
    [SerializeField] private Sprite landsknechte;
    [SerializeField] private Sprite suisse;
    [SerializeField] private Sprite arquebusiers;
    [SerializeField] private Sprite artillery;
    [SerializeField] private Sprite stradioti;
    [SerializeField] private Sprite coustilliers;
    private Image myImage;

    private void UnitClicked(int unitId)
    {
        Unit tempUnit = null;
        if (!BattleManager.Instance.isInputBlocked && BattleManager.Instance.gameMode != "deploy")
        {
            tempUnit = BattleManager.Instance.GetUnit(unitId);
            switch (tempUnit.GetUnitType())
            {
                case "Gendarmes":
                    myImage.sprite = heavyCavalery;
                    break;
                case "Landsknechte":
                    myImage.sprite = landsknechte;
                    break;
                case "Suisse":
                    myImage.sprite = suisse;
                    break;
                case "Imperial Cavalery":
                    myImage.sprite = heavyCavalery;
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

    public void InitialPicture(string pictureType)
    {
        switch (pictureType)
        {
            case "Gendarmes":
                myImage.sprite = heavyCavalery;
                break;
            case "Landsknechte":
                myImage.sprite = landsknechte;
                break;
            case "Suisse":
                myImage.sprite = suisse;
                break;
            case "Imperial Cavalery":
                myImage.sprite = heavyCavalery;
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

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
    }

    private void OnEnable()
    {
        EventManager.onUnitClicked += UnitClicked;
    }

    void Awake()
    {
        myImage = GetComponent<Image>();
    }
}
