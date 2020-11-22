using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuitController : MonoBehaviour
{
    public void ExitProgram()
    {
        transform.DOPunchScale(new Vector3(0.1f, 0.1f), 0.15f, 20).OnComplete(() => Application.Quit());
    }
}
