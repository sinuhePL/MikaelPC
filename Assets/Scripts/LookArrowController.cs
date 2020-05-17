using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LookArrowController : MonoBehaviour
{
    private Camera myCamera;

    public bool isActive;
    public Button otherButton;
    public enum directionEnum { left, right};
    [SerializeField] private directionEnum directionType;
    private Button myButton;
    private bool isShifted;
    private int lastClickedUnit;
    private Vector3 startingPosition;

    private void ArrowUnitClicked(int uId)
    {
        if (directionType == directionEnum.left)
        {
            if (lastClickedUnit == uId && isShifted)
            {
                transform.DOMoveX(startingPosition.x, 0.3f).SetEase(Ease.InOutQuint);
                isShifted = false;
            }
            lastClickedUnit = uId;
        }
    }

    private void TileClicked(int tId)
    {
        if (directionType == directionEnum.left)
        {
            transform.DOMoveX(startingPosition.x, 0.3f).SetEase(Ease.InOutQuint);
            isShifted = false;
        }
    }

    public void ShiftMe()
    {
        if (directionType == directionEnum.left)
        {
            if(!isShifted) transform.DOMoveX(startingPosition.x + 430.0f, 0.3f).SetEase(Ease.InOutQuint);
            else transform.DOMoveX(startingPosition.x, 0.3f).SetEase(Ease.InOutQuint);
            isShifted = !isShifted;
        }
    }

    private void AttackClicked(int aId, bool b)
    {
        TileClicked(0);
    }

    // zmienia kąt patrzenia kamery obracajac ją wokół punktu na który aktualnie patrzy kamera
    public void ChangeViewAngle()
    {
        if (isActive && !BattleManager.isInputBlocked)
        {
            if (GameManagerController.viewType == "isometric")
            {
                myCamera.GetComponent<PanZoom>().ChangeViewAngle("arrow");
                otherButton.GetComponent<LookArrowController>().isActive = true;
                isActive = false;
            }
        }
    }

    public void ArrowReleased()
    {
        if (GameManagerController.viewType == "perspective")
        {
            myCamera.GetComponent<PanZoom>().StopRotate();
            BattleManager.isInputBlocked = false;
        }
    }

    public void ArrowPressed()
    {
        if(GameManagerController.viewType == "perspective")
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

    private void OnDestroy()
    {
        if (directionType == directionEnum.left)
        {
            EventManager.onUnitClicked -= ArrowUnitClicked;
            EventManager.onTileClicked -= TileClicked;
            EventManager.onAttackClicked -= AttackClicked;
        }
    }

    public void Start()
    {
        myCamera = Camera.main;
        myButton = GetComponent<Button>();
        gameObject.SetActive(false);
        isShifted = false;
        lastClickedUnit = 0;
        startingPosition = transform.position;
        if (directionType == directionEnum.left)
        {
            EventManager.onUnitClicked += ArrowUnitClicked;
            EventManager.onTileClicked += TileClicked;
            EventManager.onAttackClicked += AttackClicked;
        }
    }
}
