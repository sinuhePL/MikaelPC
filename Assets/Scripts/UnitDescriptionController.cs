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
                break;
            case "Landsknechte":
                unitImage.sprite = landsknechtsSprite;
                unitCaption.sprite = landsknechtsCaption;
                break;
            case "Suisse":
                unitImage.sprite = suisseSprite;
                unitCaption.sprite = suisseCaption;
                break;
            case "Imperial Cavalery":
                unitImage.sprite = imperialCavalerySprite;
                unitCaption.sprite = imperialCavaleryCaption;
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
        if (!BattleManager.isInputBlocked && BattleManager.gameMode != "deploy")
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
