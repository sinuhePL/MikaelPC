using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    private int tutorialStep;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private Text titleText;
    [SerializeField] private Text mainText;
    [SerializeField] private Text swordText;
    [SerializeField] private Text flagText;
    [SerializeField] private Text starText;
    [SerializeField] private Image swordImage;
    [SerializeField] private Image flagImage;
    [SerializeField] private Image starImage;
    [SerializeField] private Sprite quitSprite;

    private void Start()
    {
        EventManager.onUnitDeployed += tutorialUnitDeployed;
        EventManager.onAttackClicked += tutorialAttackClicked;
        EventManager.onResultMenuClosed += tutorialResultMenuClosed;
        EventManager.onDiceResult += tutorialDiceResult;
        EventManager.onRouteTestOver += tutorialRoutTestEnd;
        BattleManager.Instance.ignoreRoutTest = true;
        tutorialStep = 1;
        titleText.text = "Welcome!";
        mainText.text = "     Welcome to Renaissance Battles: Pavia 1525. In this  tutorial you will learn the rules of the game.\n    The game consist of two phases: deployment phase when you deploy your troops on the battlefield and battle phase when you lead your army to victory (or defeat). Actions in battle phase are taken in alternating turns. Every turn you can order one of your units to attack. To win the game you have to destroy your opponent's forces or force them to retreat. ";
    }

    private void OnDestroy()
    {
        EventManager.onUnitDeployed -= tutorialUnitDeployed;
        EventManager.onAttackClicked -= tutorialAttackClicked;
        EventManager.onResultMenuClosed -= tutorialResultMenuClosed;
        EventManager.onDiceResult -= tutorialDiceResult;
        EventManager.onRouteTestOver -= tutorialRoutTestEnd;
    }

    public void NextClicked()
    {
        Sequence tutorialSequence = DOTween.Sequence();
        SoundManagerController.Instance.PlayClick();
        if (tutorialStep == 3 || tutorialStep == 4 || tutorialStep == 5 || tutorialStep == 6 || tutorialStep == 10 || tutorialStep == 12 || tutorialStep == 13 || tutorialStep == 14)
        {
            BattleManager.Instance.isInputBlocked = false;
            tutorialSequence.Append(nextButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
            tutorialSequence.Append(transform.DOScale(0.0f, 0.0f).SetEase(Ease.InBack));
            return;
        }
        if(tutorialStep == 1)
        {
            tutorialSequence.Append(nextButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
            titleText.text = "Controls";
            mainText.text = "    You can move your view over battlefield by clicking and dragging cursor over screen and zoom in and out by moving mouse wheel. During battle phase you can also rotate your view by clicking arrows on the left and right side of the screen. \n    You can enter game menu by clicking right bottom button and make game actions by clicking left bottom button.";
            tutorialStep++;
            return;
        }
        if (tutorialStep == 2)
        {
            tutorialSequence.Append(nextButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
            titleText.text = "Deployment phase";
            mainText.text = "    In the deployment phase on right side of the screen you can see a list of your available units. Selected unit is enlarged and it's description is visible in the right window. On the battlefield borders of tiles available to deployment are highlighted in army's color. Influence of every available battlefield tile on selected unit is displayed on this tile.\n    Now click one unit from the list on the right side of screen to select it and deploy it on a battlefield by clicking selected tile.";
            tutorialStep++;
            return;
        }
        if (tutorialStep == 9)
        {
            tutorialSequence.Append(nextButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
            titleText.text = "Attacking";
            mainText.text = "     Every turn in the Battle phase you can order one unit to make attack. Click an unit to select it. Selected unit is highlighted and in the left top corner you can see selected unit picture, name, strength and morale. Click the picture to see more information about this unit. In front of selected unit arrows appear representing available attacks. Click one of arrows to select it. Detailed information about attack appear next to unit picture.";
            tutorialStep++;
            return;
        }
        if (tutorialStep == 11)
        {
            tutorialSequence.Append(nextButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20));
            swordImage.gameObject.SetActive(false);
            flagImage.gameObject.SetActive(false);
            starImage.gameObject.SetActive(false);
            swordText.gameObject.SetActive(false);
            flagText.gameObject.SetActive(false);
            starText.gameObject.SetActive(false);
            titleText.text = "Attacking";
            mainText.text = "     Additionally on the battlefield appeared arrows representing attacks activated by attack you just selected. So be careful choosing your attack because it may activate more deadly enemy attack. Notice that a key field indicator on a battlefield bliks when you select attack that can capture it. Now click on the button in the left bottom corner of the screen to make attack.";
            tutorialStep++;
            return;
        }
        if(tutorialStep == 15)
        {
            tutorialSequence.Append(nextButton.transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20)).OnComplete(() => GameManagerController.Instance.LoadLevel("MenuScene"));
        }
    }

    private void tutorialUnitDeployed(int uId, int tId)
    {
        tutorialStep++;
        if (tutorialStep != 7 )
        {
            transform.DOScale(1.0f, 0.0f).SetEase(Ease.OutBack);
            BattleManager.Instance.isInputBlocked = true;
        }
        if (tutorialStep == 4)
        {
            titleText.text = "Deployment";
            mainText.text = "    Excellent! Notice that now you can deploy unit in second row, behind unit that you placed before. Units on second row can replace units in first row in case of their destruction. Unit is destroyed when it's morale or it's strength drops to zero. When there is no unit to replace destroyed unit, enemy unit that destroyed unit activates flanking attacks.\n    Now select another unit from the list on the right side of screen and deploy it on a battlefield on selected tile.";
            return;
        }
        if (tutorialStep == 5)
        {
            titleText.text = "Deployment";
            mainText.text = "    OK! Notice that now you can select units already deployed on battlefield and change their position. However you cannot do this with unit in first row when behind it another unit is placed.\n    Now select last undeployed unit from the list on the right side of screen and deploy it on a battlefield on selected tile.";
            return;
        }
        if (tutorialStep == 6)
        {
            titleText.text = "Deployment";
            mainText.text = "    All right! Some tiles between army lines are key fields. They have name and place for owner marker. Capturing them gives your unit one more attacking die. Now, click End Deployment button on the left bottom corner of the screen to finish deployment phase.";
            return;
        }
        if (tutorialStep == 8)
        {
            titleText.text = "Battle phase";
            mainText.text = "     In order to defeat enemy you have to destroy all of it's units or force them to retreat. Every time a unit loose it's strength or retreats and morale of whole army is 30 or less a rout test is triggered - a number between 3 and 30 is drawn and if it's less than morale of the whole army the test is failed, whole army retreats and the battle is over. You can check morale of both army in the right top corner of the screen.";
            tutorialStep++;
            return;
        }
    }

    private void tutorialAttackClicked(int idArrow, bool isCounterAttack)
    {
        if(tutorialStep == 10)
        {
            transform.DOScale(1.0f, 0.0f).SetEase(Ease.OutBack);
            BattleManager.Instance.isInputBlocked = true;
            titleText.text = "Attacking";
            mainText.text = "     In the window next to unit picture attack description appeared. Making attack means throwing two sets of dice - attack dice and defence dice. To make a successful attack you have to get at least one pair of the same symbols on attack dice. Pair of the same symbols on defence dice means that defender inflicted strength or morale damage. Meaning of dice symbols is explained below.";
            swordImage.gameObject.SetActive(true);
            flagImage.gameObject.SetActive(true);
            starImage.gameObject.SetActive(true);
            swordText.gameObject.SetActive(true);
            flagText.gameObject.SetActive(true);
            starText.gameObject.SetActive(true);
            tutorialStep++;
            return;
        }
    }

    private void tutorialResultMenuClosed(string mode)
    {
        if (tutorialStep == 12)
        {
            transform.DOScale(1.4f, 0.0f).SetEase(Ease.OutBack);
            BattleManager.Instance.isInputBlocked = true;
            titleText.text = "Attack result";
            mainText.text = "     After every attack window with attack result appear. Besides strength and morale loose attack may result in capturing key field, triggering rout test, changing attack target or adding morale to neighbours. Click end turn button in the left bottom corner of the screen to end your turn. Keep attacking until someone does strength damage.";
            tutorialStep++;
            BattleManager.Instance.ignoreRoutTest = false;
            return;
        }
    }

    private void tutorialDiceResult(StateChange result)
    {
        if(tutorialStep == 13 && (result.attackerStrengthChange < 0 || result.defenderStrengthChange < 0))
        {
            transform.DOScale(1.4f, 0.0f).SetEase(Ease.OutBack);
            BattleManager.Instance.isInputBlocked = true;
            titleText.text = "Route test";
            mainText.text = "     If an unit loses strength or retreats rout test is made. Number between 3 and 30 is drawn and if result is higher then army morale this army retreats and the battle is over. Army morale is visible in top right corner of the screen.";
            tutorialStep++;
            return;
        }
    }

    private void tutorialRoutTestEnd(string description, int result, int morale)
    {
        if (tutorialStep == 14)
        {
            transform.DOScale(1.4f, 0.0f).SetEase(Ease.OutBack);
            BattleManager.Instance.isInputBlocked = true;
            titleText.text = "Congratulations!";
            mainText.text = "     You've just finished tutorial for Renaissance Battles: Pavia 1525 and you're ready to start a game. Click Quit button to return to main menu.";
            tutorialStep++;
            nextButton.GetComponent<Image>().sprite = quitSprite;
            return;
        }
    }
}
