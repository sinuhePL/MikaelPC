using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UnitDescriptionController : MonoBehaviour
{
    [SerializeField] private Sprite suisseSprite;
    [SerializeField] private Sprite gendarmesSprite;
    [SerializeField] private Sprite landsknechtsSprite;
    [SerializeField] private Sprite imperialCavalerySprite;
    [SerializeField] private Sprite suisseCaption;
    [SerializeField] private Sprite gendarmesCaption;
    [SerializeField] private Sprite landsknechtsCaption;
    [SerializeField] private Sprite imperialCavaleryCaption;
    [SerializeField] private Image unitImage;
    [SerializeField] private Image unitCaption;
    [SerializeField] private Text unitDescription;
    [SerializeField] private Text unitRules;
    private Vector3 startingPosition;
    private bool isShifted;
    private int lastUnitClicked;

    private void ShowHide(bool show)
    {
        isShifted = show;
        if (!show) transform.DOMoveX(startingPosition.x - 500.0f, 0.3f).SetEase(Ease.InOutQuint);
        else transform.DOMoveX(startingPosition.x, 0.3f).SetEase(Ease.InOutQuint);
    }

    private void HideMe()
    {
        ShowHide(false);
    }

    private void HideMe(int i)
    {
        ShowHide(false);
    }

    private void HideMe(int i, bool b)
    {
        ShowHide(false);
    }

    private void ChangeDescription(string uType)
    {
        switch (uType)
        {
            case "Gendarmes":
                unitImage.sprite = gendarmesSprite;
                unitCaption.sprite = gendarmesCaption;
                unitDescription.text = "    A gendarme was a heavy cavalryman of noble birth, primarily serving in the French army from the Late Medieval to the Early Modern periods of European history. They provided the Kings of France with a potent regular force of heavily-armoured lance-armed cavalry which, when properly employed in combination with pikemen and artillery, could dominate the battlefield. They fought exclusively on horseback, generally in a very thin line (en haye), usually two or even just one rank deep";
                break;
            case "Landsknechte":
                unitImage.sprite = landsknechtsSprite;
                unitCaption.sprite = landsknechtsCaption;
                unitDescription.text = "    The Landsknechte were German-speaking mercenaries, consisting predominantly of pikemen and supporting foot soldiers, who became an important military force in early modern Europe. Their front line was formed by doppelsöldner, renowned for their use of arquebus and zweihänder in the early modern period. They formed the bulk of the Imperial Army (Holy Roman Empire) from the late 1400s to the early 1600s.";
                break;
            case "Suisse":
                unitImage.sprite = suisseSprite;
                unitCaption.sprite = suisseCaption;
                unitDescription.text = "    The Suisse were considered the best mercenery infrantry during late Medieval Age. They formed up into three dense columns, up to eight thousand men strong, for an attack. Each column was composed of pikemen, carrying their pikes at shoulder height as they advanced, with halberdiers and double-handed swordsmen in the center. The key to victory, the Swiss believed, was to advance, regardless of cost, regardless of obstacles.";
                break;
            case "Imperial Cavalery":
                unitImage.sprite = imperialCavalerySprite;
                unitCaption.sprite = imperialCavaleryCaption;
                unitDescription.text = "   Imperial cavalry consisted of heavy cavalryman of noble birth from Spain, Germany, Low Countries and Burgundy. They were armed with lances, fought on horseback and were intended to deliver a battlefield charge. Spanish cavalryman were usually lighter armored (often without barding) and with lighter lances. Spanish cavalryman also deployed in line while their German counterparts preferred a deeper, wedge-edged formation.";
                break;
        }
    }

    private void UIDeployClicked(int armyId, int position, int uId, string uType)
    {
        ChangeDescription(uType);
    }

    private void UnitClicked(int unitId)
    {
        Unit tempUnit = null;
        if (!BattleManager.Instance.isInputBlocked && BattleManager.Instance.gameMode != "deploy")
        {
            if (unitId == lastUnitClicked) ShowHide(false);
            else
            {
                tempUnit = BattleManager.Instance.GetUnit(unitId);
                ChangeDescription(tempUnit.GetUnitType());
                lastUnitClicked = unitId;
            }
        }
    }

    private void OnEnable()
    {
        EventManager.onGameStart += HideMe;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onUIDeployPressed += UIDeployClicked;
        EventManager.onTileClicked += HideMe;
        EventManager.onAttackClicked += HideMe;
    }

    private void Start()
    {
        startingPosition = transform.position;
        isShifted = false;
        lastUnitClicked = 0;
    }

    private void OnDestroy()
    {
        EventManager.onGameStart -= HideMe;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onUIDeployPressed -= UIDeployClicked;
        EventManager.onTileClicked -= HideMe;
        EventManager.onAttackClicked -= HideMe;
    }

    public void ShiftUnitDescription()
    {
        ShowHide(!isShifted);
    }
}
