using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitUIController : MonoBehaviour
{
    private int lastClickedUnit;
    private bool isHidden;
    private Vector3 startingPosition;
    private bool isShifted;

    private void TileClicked(int idTile)
    {
        transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InOutQuint);
        isHidden = true;
        isShifted = false;
    }

    private void UpdateBoard(string mode)
    {
        TileClicked(0);
    }

    public void WidgetPressed()
    {
        if (!isShifted) transform.DOMoveX(startingPosition.x + 840.0f, 0.3f).SetEase(Ease.InOutQuint);
        else transform.DOMoveX(startingPosition.x + 380.0f, 0.3f).SetEase(Ease.InOutQuint);
        isShifted = !isShifted;
    }

    private void UnitClicked(int unitId)
    {
        if (!BattleManager.isInputBlocked && BattleManager.gameMode != "deploy")
        {
            if(unitId != lastClickedUnit || isHidden)
            {
                if(isHidden) transform.DOMoveX(startingPosition.x + 380.0f, 0.3f).SetEase(Ease.OutBack);
                lastClickedUnit = unitId;
                isHidden = false;
            }
            else
            {
                transform.DOMoveX(startingPosition.x, 0.3f).SetEase(Ease.InOutQuint);
                isHidden = true;
                isShifted = false;
            }
        }
    }

    private void ShiftBack(int a, bool b)
    {
        transform.DOMoveX(startingPosition.x + 380.0f, 0.3f).SetEase(Ease.InOutQuint);
        isShifted = false;
    }

    private void OnDestroy()
    {
        EventManager.onTileClicked -= TileClicked;
        EventManager.onResultMenuClosed -= UpdateBoard;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onAttackClicked -= ShiftBack;
    }

    private void OnEnable()
    {
        EventManager.onTileClicked += TileClicked;
        EventManager.onResultMenuClosed += UpdateBoard;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onAttackClicked += ShiftBack;
    }

    void Start()
    {
        lastClickedUnit = -1;
        isHidden = true;
        isShifted = false;
        startingPosition = transform.position;
    }
}
