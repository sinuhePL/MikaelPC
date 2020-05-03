using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UnitArrowController : MonoBehaviour
{
    private Vector3 startingPosition;

    private void MoveArrow(int aId, int p, int uId, string uType)
    {
        transform.DOMoveY(startingPosition.y - p * 116.0f, 0.25f).SetEase(Ease.InOutQuint);
    }

    private void OnEnable()
    {
        EventManager.onUIDeployPressed += MoveArrow;
        EventManager.onGameStart += positionOnGameStart;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(460.0f, -90.0f, 0.0f);
        startingPosition = transform.position;
    }

    private void positionOnGameStart()
    {
        transform.DOMoveY(startingPosition.y - 116.0f, 0.0f);
    }

    private void OnDestroy()
    {
        EventManager.onUIDeployPressed -= MoveArrow;
        EventManager.onGameStart -= positionOnGameStart;
    }
}
