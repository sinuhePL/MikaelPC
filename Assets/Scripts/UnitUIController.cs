using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitUIController : MonoBehaviour
{
    private int lastClickedUnit;
    private bool isHidden;

    private void TileClicked(int idTile)
    {
        transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
        isHidden = true;
    }

    private void UpdateBoard()
    {
        TileClicked(0);
    }

    private void UnitClicked(int unitId)
    {
        if (!BattleManager.isInputBlocked)
        {
            if(unitId != lastClickedUnit || isHidden)
            {
                transform.DOMoveX(190.0f, 0.3f).SetEase(Ease.OutBack);
                lastClickedUnit = unitId;
                isHidden = false;
            }
            else
            {
                transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
                isHidden = true;
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.onTileClicked -= TileClicked;
        EventManager.onResultMenuClosed -= UpdateBoard;
        EventManager.onUnitClicked -= UnitClicked;
    }

    private void OnEnable()
    {
        EventManager.onTileClicked += TileClicked;
        EventManager.onResultMenuClosed += UpdateBoard;
        EventManager.onUnitClicked += UnitClicked;
    }

    void Start()
    {
        lastClickedUnit = -1;
        isHidden = true;
    }
}
