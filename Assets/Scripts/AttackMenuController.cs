using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackMenuController : MonoBehaviour
{
    private Camera myCamera;
    private bool isVisible;

    private void AttackClicked(int idAttack)
    {
        Sequence mySequence = DOTween.Sequence();
        Vector3 endPosition;

        endPosition = myCamera.WorldToScreenPoint(BattleManager.Instance.GetAttack(idAttack).GetPosition());
        if(isVisible) mySequence.Append(transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack));
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(0.45f, 0.3f).SetEase(Ease.OutBack));
        isVisible = true;
    }

    private void UnitClicked(int idUnit)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
    }

    private void TileClicked(int idTile)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
    }

    private void ActionButtonPressed(int idTile)
    {
        transform.DOScale(0.0f, 0.3f).SetEase(Ease.InBack);
        isVisible = false;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onTileClicked -= TileClicked;
        EventManager.onActionButtonPressed -= ActionButtonPressed;
    }

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onTileClicked += TileClicked;
        EventManager.onActionButtonPressed += ActionButtonPressed;
        isVisible = false;
    }
}
