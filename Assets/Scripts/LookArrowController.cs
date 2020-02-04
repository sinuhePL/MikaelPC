using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookArrowController : MonoBehaviour
{
    private Camera myCamera;

    public bool isActive;
    public Button otherButton;
    public enum directionEnum { left, right};
    [SerializeField] private directionEnum directionType;
    private Button myButton;

    // zmienia kąt patrzenia kamery obracajac ją wokół punktu na który aktualnie patrzy kamera
    public void ChangeViewAngle()
    {
        if (isActive && !BattleManager.isInputBlocked)
        {
            if (BattleManager.viewType == "isometric")
            {
                myCamera.GetComponent<PanZoom>().ChangeViewAngle("arrow");
                otherButton.GetComponent<LookArrowController>().isActive = true;
                isActive = false;
            }
        }
    }

    public void ArrowReleased()
    {
        if (BattleManager.viewType == "perspective")
        {
            myCamera.GetComponent<PanZoom>().StopRotate();
            BattleManager.isInputBlocked = false;
        }
    }

    public void ArrowPressed()
    {
        if(BattleManager.viewType == "perspective")
        {
            BattleManager.isInputBlocked = true;
            if (directionType == directionEnum.right) myCamera.GetComponent<PanZoom>().ArrowPressed("right");
            if (directionType == directionEnum.left) myCamera.GetComponent<PanZoom>().ArrowPressed("left");
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
        myButton = GetComponent<Button>();
    }
}
