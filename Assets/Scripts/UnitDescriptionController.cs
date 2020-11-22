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
    [SerializeField] private Sprite garrisonSprite;
    [SerializeField] private Sprite suisseCaption;
    [SerializeField] private Sprite gendarmesCaption;
    [SerializeField] private Sprite landsknechtsCaption;
    [SerializeField] private Sprite imperialCavaleryCaption;
    [SerializeField] private Sprite arquebusiersCaption;
    [SerializeField] private Sprite artilleryCaption;
    [SerializeField] private Sprite stradiotiCaption;
    [SerializeField] private Sprite coustilliersCaption;
    [SerializeField] private Sprite garrisonCaption;
    [SerializeField] private Image unitImage;
    [SerializeField] private Image unitCaption;
    [SerializeField] private Text unitDescription;
    [SerializeField] private Text unitCommander;
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

    private void ShowMe(int deploymentPhase)
    {
        if (deploymentPhase == 1) ShowHide(true);
    }

    private void ChangeDescription(string uType, string commander)
    {
        switch (uType)
        {
            case "Gendarmes":
                unitImage.sprite = gendarmesSprite;
                unitCaption.sprite = gendarmesCaption;
                unitDescription.text = "    A gendarme was a heavy cavalryman of noble birth, primarily serving in the French army from the Late Medieval to the Early Modern periods of European history. They provided the Kings of France with a potent regular force of heavily-armoured lance-armed cavalry which, when properly employed in combination with pikemen and artillery, could dominate the battlefield. They fought exclusively on horseback, generally in a very thin line (en haye), usually two or even just one rank deep";
                unitRules.text = "- Weak attack against targets on hills.\n- Weak against Landsknechts and Suisse.\n- Every kill adds 1 point of morale to neighbours.";
                break;
            case "Landsknechte":
                unitImage.sprite = landsknechtsSprite;
                unitCaption.sprite = landsknechtsCaption;
                unitDescription.text = "    The Landsknechte were German-speaking mercenaries, consisting predominantly of pikemen and supporting foot soldiers, who became an important military force in early modern Europe. Their front line was formed by doppelsöldner, renowned for their use of arquebus and zweihänder in the early modern period. They formed the bulk of the Imperial Army (Holy Roman Empire) from the late 1400s to the early 1600s.";
                unitRules.text = "- Good defence againt cavalry.\n- Disables enemy attacks targeting other units.\n- Good attack against arquebusiers.";
                break;
            case "Suisse":
                unitImage.sprite = suisseSprite;
                unitCaption.sprite = suisseCaption;
                unitDescription.text = "    The Suisse were considered the best mercenery infrantry during late Medieval Age. They formed up into three dense columns, up to eight thousand men strong, for an attack. Each column was composed of pikemen, carrying their pikes at shoulder height as they advanced, with halberdiers and double-handed swordsmen in the center. The key to victory, the Swiss believed, was to advance, regardless of cost, regardless of obstacles.";
                unitRules.text = "- Good defence againt cavalry.\n- Disables enemy attacks targeting other units.\n- Good attack against arquebusiers.";
                break;
            case "Imperial Cavalery":
                unitImage.sprite = imperialCavalerySprite;
                unitCaption.sprite = imperialCavaleryCaption;
                unitDescription.text = "    Imperial cavalry consisted of heavy cavalryman of noble birth from Spain, Germany, Low Countries and Burgundy. They were armed with lances, fought on horseback and were intended to deliver a battlefield charge. Spanish cavalryman were usually lighter armored (often without barding) and with lighter lances. Spanish cavalryman also deployed in line while their German counterparts preferred a deeper, wedge-edged formation.";
                unitRules.text = "- Weak attack against targets on hills.\n- Weak against Landsknechts and Suisse.\n- Every kill adds 1 point of morale to neighbours."; 
                break;
            case "Arquebusiers":
                unitImage.sprite = arquebusiersSprite;
                unitCaption.sprite = arquebusiersCaption;
                unitDescription.text = "    Arquebusiers were foot soldiers armed with arquebus. Arquebus was a form of long gun that appeared in Europe and the Ottoman Empire during the 15th century. The addition of a shoulder stock, priming pan, and matchlock mechanism in the late 15th century turned the arquebus into a handheld firearm and also the first firearm equipped with a trigger. Arquebuses replaced crossbows as easier to handle and more deadly. ";
                unitRules.text = "- Good defence in woods\n- Powerful counterattack.\n- Weak attack and defence";
                break;
            case "Artillery":
                unitImage.sprite = artillerySprite;
                unitCaption.sprite = artilleryCaption;
                unitDescription.text = "    Cannons appeared in Europe in 14th century, by the 16th century they were made in a great variety of lengths and bore diameters but field Artillery was still young invention during battle of Pavia. Used for the first time by Charles VIII of France in 1494 during his italian campaign. Cannons where placed on wooden carts pulled by horses and oxes. While very slow and with low fire rate field artillery proved useful on battlefields during Italian Wars (1494 - 1529). ";
                unitRules.text = "- Effective against Landsknechts and Suisse.\n- Good attack against targets in buildings.\n- Weak defence against light cavalery.\n- Can change attack target.\n- Can't capture key fields.";
                break;
            case "Stradioti":
                unitImage.sprite = stradiotiSprite;
                unitCaption.sprite = stradiotiCaption;
                unitDescription.text = "    The stradioti or stratioti were mercenary units from the Balkans recruited mainly by states of southern and central Europe. The stradioti were pioneers of light cavalry tactics during renaissance era. They employed hit-and-run tactics, ambushes, feigned retreats and other complex maneuvers. In some ways, these tactics echoed those of the Ottoman sipahis and akinci. They had some notable successes also against French heavy cavalry during the Italian Wars";
                unitRules.text = "- Can attack from second line.\n- Good attack against artillery units.\n- Counterattack captures key points.\n- Weak defence against Imperial Gendarmes";
                break;
            case "Coustilliers":
                unitImage.sprite = coustilliersSprite;
                unitCaption.sprite = coustilliersCaption;
                unitDescription.text = "    The coustillier (also coutillier, coutilier) was a title of a low-ranking professional soldier in Medieval French armies. Since mid 15th century coustelliers were a lightly armoured horseman.  A French coutilier of 1446 was equipped with a helmet, leg armour, a haubergeon, jack or brigandine, a dagger, sword and either a demilance or a voulge. Although unable to stand up against heavy cavalry in battle, these soldiers made excellent skirmishers, raiders and scouts.";
                unitRules.text = "- Can attack from second line.\n- Good attack against artillery units.\n- Counterattack captures key points.\n- Weak defence against Imperial Cavalery";
                break;
            case "Garrison":
                unitImage.sprite = garrisonSprite;
                unitCaption.sprite = garrisonCaption;
                unitDescription.text = "    During battle of Pavia, it's garrison was 9000 men strong, well equipped and prepared for long siege. Before battle garrison managed to repell two french assaults and the only concern of its commander was paying wages - garrison consisted mainly of mercenaries whom Antonio de Leyva was able to pay only by melting the church plates.";
                unitRules.text = " - Good defence againt cavalry.\n - Disables enemy attacks targeting other units.\n - Weak attack";
                break;
        }
        switch(commander)
        {
            case "Francis I":
                unitCommander.text = "    Francis I -  King of France from 1515 until his death in 1547. He succeeded his first cousin once removed and father-in-law Louis XII, who died without a son. Captured during battle of Pavia by imperial troops.";
                break;
            case "de Lorraine":
                unitCommander.text = "    Francois de Lorraine (1506 - 1525) was the Lord of Lambesc. He commanded the Black Band of renegade Landsknechts at the Battle of Pavia, and in the combat that ensued between his unit and Imperial Landsknechts, Lorraine was killed.";
                break;
            case "de La Marck":
                unitCommander.text = "    Robert III de La Marck (1491 – 1537), Seigneur of Fleuranges, Marshal of France and historian. Taken prisoner during battle of Pavia with Francis I";
                break;
            case "de la Pole":
                unitCommander.text = "    Richard de la Pole (1480 – 24 February 1525) duke of Suffolk was a pretender to the English crown - he was the last Yorkist claimant to actively seek the crown of England. Commanded french infrantry during battle of Pavia where he was killed.";
                break;
            case "de Genouillac":
                unitCommander.text = "    Jacques Ricard de Genouillac, called Galiot de Genouillac , (1465 - 1546) was a French diplomat and warlord from the Ricard de Genouillac family. He was involved in the Battle of Pavia as Grand Master of Artillery of France .";
                break;
            case "Tiercelin":
                unitCommander.text = "    Charles Tiercelin - french nobleman, commander of light cavalery unit during battle of Pavia.";
                break;
            case "von Frundsberg":
                unitCommander.text = "    Georg von Frundsberg (1473 – 1528) was a German military and Landsknecht leader in the service of the Holy Roman Empire and Imperial House of Habsburg.";
                break;
            case "de Lannoy":
                unitCommander.text = "    Charles de Lannoy (1487 – 1527) was a soldier and statesman from the Low Countries in service of the Habsburg Emperors Maximilian I and Charles V.";
                break;
            case "de Vasto":
                unitCommander.text = "    Alfonso d'Avalos d'Aquino, VI marquis of Pescara and II of Vasto (1502 – 1546), was an Italian condottiero of Spanish origins, renowned for his service in favor of Charles V, Holy Roman Emperor and King of Spain. ";
                break;
            case "Pescara":
                unitCommander.text = "    Fernando Francesco d'Ávalos, 5th marquis of Pescara (1489 – 1525), was an Italian condottiero of Aragonese extraction. He was the chief commander of the Habsburg armies of Charles V in Italy during italian wars. ";
                break;
            case "de Leyva":
                unitCommander.text = "    Antonio de Leyva, Duke of Terranova, Prince of Ascoli (1480–1536) was a Spanish general during the Italian Wars. He commanded Pavia during the siege of the city by Francis I of France, and took part in the Battle of Pavia in 1525.";
                break;
            case "d'Alencon":
                unitCommander.text = "    Charles IV of Alençon,  Duke of Alençon etc. (1489 – 1525) As first prince of the blood, Charles was a prominent figure in the early part of Francis I (his brother-in-law) reign. He commanded rear guard during battle of Pavia.";
                break;
            case "":
                unitCommander.text = "    Unknown";
                break;
        }
    }

    private void UIDeployClicked(int armyId, int position, int uId, string uType, string commander)
    {
        ChangeDescription(uType, commander);
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
                ChangeDescription(tempUnit.GetUnitType(), tempUnit.GetCommander());
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
        EventManager.onDeploymentStart += ShowMe;
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
        EventManager.onDeploymentStart -= ShowMe;
    }

    public void ShiftUnitDescription()
    {
        ShowHide(!isShifted);
    }
}
