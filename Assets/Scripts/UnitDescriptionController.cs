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
    [SerializeField] private Sprite arquebusiersSprite;
    [SerializeField] private Sprite artillerySprite;
    [SerializeField] private Sprite stradiotiSprite;
    [SerializeField] private Sprite coustilliersSprite;
    [SerializeField] private Sprite suisseCaption;
    [SerializeField] private Sprite gendarmesCaption;
    [SerializeField] private Sprite landsknechtsCaption;
    [SerializeField] private Sprite imperialCavaleryCaption;
    [SerializeField] private Sprite arquebusiersCaption;
    [SerializeField] private Sprite artilleryCaption;
    [SerializeField] private Sprite stradiotiCaption;
    [SerializeField] private Sprite coustilliersCaption;
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
                unitDescription.text = "    Imperial cavalry consisted of heavy cavalryman of noble birth from Spain, Germany, Low Countries and Burgundy. They were armed with lances, fought on horseback and were intended to deliver a battlefield charge. Spanish cavalryman were usually lighter armored (often without barding) and with lighter lances. Spanish cavalryman also deployed in line while their German counterparts preferred a deeper, wedge-edged formation.";
                break;
            case "Arquebusiers":
                unitImage.sprite = arquebusiersSprite;
                unitCaption.sprite = arquebusiersCaption;
                unitDescription.text = "    Arquebusiers were foot soldiers armed with arquebus. Arquebus was a form of long gun that appeared in Europe and the Ottoman Empire during the 15th century. The addition of a shoulder stock, priming pan, and matchlock mechanism in the late 15th century turned the arquebus into a handheld firearm and also the first firearm equipped with a trigger. Arquebuses replaced crossbows as easier to handle and more deadly. ";
                break;
            case "Artillery":
                unitImage.sprite = artillerySprite;
                unitCaption.sprite = artilleryCaption;
                unitDescription.text = "    Cannons appeared in Europe in 14th century, by the 16th century they were made in a great variety of lengths and bore diameters but field Artillery was still young invention during battle of Pavia. Used for the first time by Charles VIII of France in 1494 during his italian campaign. Cannons where placed on wooden carts pulled by horses and oxes. While very slow and with low fire rate field artillery proved useful on battlefields during Italian Wars (1494 - 1529). ";
                break;
            case "Stradioti":
                unitImage.sprite = stradiotiSprite;
                unitCaption.sprite = stradiotiCaption;
                unitDescription.text = "    The stradioti or stratioti were mercenary units from the Balkans recruited mainly by states of southern and central Europe. The stradioti were pioneers of light cavalry tactics during renaissance era. They employed hit-and-run tactics, ambushes, feigned retreats and other complex maneuvers. In some ways, these tactics echoed those of the Ottoman sipahis and akinci. They had some notable successes also against French heavy cavalry during the Italian Wars";
                break;
            case "Coustilliers":
                unitImage.sprite = coustilliersSprite;
                unitCaption.sprite = coustilliersCaption;
                unitDescription.text = "    The coustillier (also coutillier, coutilier) was a title of a low-ranking professional soldier in Medieval French armies. Since mid 15th century coustelliers were a lightly armoured horseman.  A French coutilier of 1446 was equipped with a helmet, leg armour, a haubergeon, jack or brigandine, a dagger, sword and either a demilance or a voulge. Although unable to stand up against heavy cavalry in battle, these soldiers made excellent skirmishers, raiders and scouts.";
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
