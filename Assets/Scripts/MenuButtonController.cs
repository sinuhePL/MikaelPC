using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuButtonController : MonoBehaviour
{
    public void ButtonPressed()
    {
        if (!BattleManager.Instance.isInputBlocked)
        {
            SoundManagerController.Instance.PlayClick();
            BattleManager.Instance.isInputBlocked = true;
            transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20);
        }
    }
}
