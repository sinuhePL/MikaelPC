using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlagController : MonoBehaviour
{
    Transform myBanner;
    float initialYPosition;

    void Start()
    {
        myBanner = transform.Find("banner");
        initialYPosition = myBanner.transform.localPosition.y;
    }

    public void ChangeBannerHeight(int max, int target)
    {
        float p;
        p = ((float) target)/((float) max);
        myBanner.transform.DOLocalMoveY(p * initialYPosition, 0.75f).SetEase(Ease.OutQuad);
    }
}
