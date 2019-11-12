using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndTurnController : MonoBehaviour
{
    private Camera myCamera;
    private bool isClicked;

    [SerializeField] private GameObject leftArrow;
    [SerializeField] private GameObject rightArrow;

    private IEnumerator WaitForClick()
    {
        isClicked = false;
        while (!isClicked)
        {
            transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.0f), 0.5f);
            yield return new WaitForSeconds(2.0f);
        }
    }

    private void Start()
    {
        myCamera = Camera.main;
        isClicked = false;
    }

    public void ButtonPressed()
    {
        isClicked = true;
        myCamera.GetComponent<PanZoom>().ChangeViewAngle("button");
        if (BattleManager.turnOwnerId == 1) BattleManager.turnOwnerId = 2;
        else BattleManager.turnOwnerId = 1;
        BattleManager.playerAttacked = false;
        leftArrow.GetComponent<LookArrowController>().ChangeActivityState();
        rightArrow.GetComponent<LookArrowController>().ChangeActivityState();
    }

    public void AttackResolved()
    {
        StartCoroutine(WaitForClick());
    }
}
