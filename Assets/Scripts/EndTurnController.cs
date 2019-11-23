using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EndTurnController : MonoBehaviour
{
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

    private void OnEnable()
    {
        EventManager.onResultMenuClosed += AttackResolved;
    }

    private void Start()
    {
        isClicked = false;
    }

    private void OnDestroy()
    {
        EventManager.onResultMenuClosed -= AttackResolved;
    }

    public void ButtonPressed()
    {
        isClicked = true;
        leftArrow.GetComponent<LookArrowController>().ChangeActivityState();
        rightArrow.GetComponent<LookArrowController>().ChangeActivityState();
        EventManager.RaiseEventOnTurnEnd();
    }

    public void AttackResolved()
    {
        if (BattleManager.turnOwnerId == 1 && !BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && !BattleManager.isPlayer2Human) ButtonPressed();
        else StartCoroutine(WaitForClick());
    }
}
