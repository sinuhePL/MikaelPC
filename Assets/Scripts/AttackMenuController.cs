using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AttackMenuController : MonoBehaviour
{
    private Camera myCamera;

    private void AttackClicked(int idAttack)
    {
        Sequence mySequence = DOTween.Sequence();
        Vector3 endPosition;
        endPosition = myCamera.WorldToScreenPoint(BattleManager.Instance.GetAttack(idAttack).GetPosition()) + new Vector3(2.0f, 0.0f, 0.0f);
        mySequence.Append(transform.DOScale(0.0f, 0.5f).SetEase(Ease.InElastic));
        mySequence.Append(transform.DOMove(endPosition, 0.01f));
        mySequence.Append(transform.DOScale(0.45f, 0.5f).SetEase(Ease.OutElastic));
    }

    private void UnitClicked(int idUnit)
    {
        transform.DOScale(0.0f, 0.5f).SetEase(Ease.InElastic);
    }

    private void TileClicked(int idTile)
    {
        transform.DOScale(0.0f, 0.5f).SetEase(Ease.InElastic);
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= AttackClicked;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onTileClicked -= TileClicked;
    }

    // Start is called before the first frame update
    void Start()
    {
        myCamera = Camera.main;
        gameObject.SetActive(false);
        EventManager.onAttackClicked += AttackClicked;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onTileClicked += TileClicked;
    }
}
