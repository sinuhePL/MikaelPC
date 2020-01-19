using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookArrowController : MonoBehaviour
{
    private Camera myCamera;

    public bool isActive;
    public Button otherButton;

    // zmienia kąt patrzenia kamery obracajac ją wokół punktu na który aktualnie patrzy kamera
    public void ChangeViewAngle()
    {
        if (isActive && !BattleManager.isInputBlocked)
        {
            myCamera.GetComponent<PanZoom>().ChangeViewAngle("arrow");
            otherButton.GetComponent<LookArrowController>().isActive = true;
            isActive = false;
        }
    }

    public void ChangeActivityState()
    {
        if (isActive) isActive = false;
        else isActive = true;
    }

    public void Start()
    {
        myCamera = Camera.main;
    }
}
