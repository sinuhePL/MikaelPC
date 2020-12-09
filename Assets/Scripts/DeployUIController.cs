using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DeployUIController : MonoBehaviour
{
    private PictureController myPictureController;
    private WidgetController myWidgetController;
    private CaptionController myCaptionController;
    private SMController[] mySMControllers;
    private bool isEnlarged;
    private int myPosition;
    private int unitId;
    private int armyId;
    private string unitType;
    private string unitCommander;
    private Vector3 startingPosition;
    [SerializeField] private GameObject panelMovedDummy;

    // Start is called before the first frame update
    void Awake()
    {
        myPictureController = GetComponentInChildren<PictureController>();
        myWidgetController = GetComponentInChildren<WidgetController>();
        myCaptionController = GetComponentInChildren<CaptionController>();
        mySMControllers = GetComponentsInChildren<SMController>();
        isEnlarged = false;
    }

    private void OnEnable()
    {
        EventManager.onUIDeployPressed += MoveDown;
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onDeploymentStart += DeploymentStart;
    }

    private void OnDestroy()
    {
        EventManager.onUIDeployPressed -= MoveDown;
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onDeploymentStart -= DeploymentStart;
    }

    private void MoveDown(int aId, int p, int uId, string uType, string commander)
    {
        if (aId == armyId && p == myPosition) return;
        if (isEnlarged)
        {
            isEnlarged = false;
            transform.DOScale(0.28f, 0.25f).SetEase(Ease.OutBack);
            transform.DOMoveX(490.0f, 0.25f).SetEase(Ease.OutBack);
            transform.DOMoveY(transform.position.y + 25.0f, 0.25f).SetEase(Ease.OutBack);
            if (aId == armyId && p < myPosition) transform.DOMoveY(startingPosition.y - 42.0f, 0.25f).SetEase(Ease.OutBack);
            return;
        }
        if(aId == armyId && p < myPosition) transform.DOMoveY(startingPosition.y - 42.0f, 0.25f).SetEase(Ease.OutBack);
        if(aId == armyId && p > myPosition) transform.DOMoveY(startingPosition.y, 0.25f).SetEase(Ease.OutBack);
    }

    private void UnitClicked(int uId)
    {
        if (uId == unitId) WidgetPressed(false);
    }

    private void DeploymentStart(int aId)
    {
        if (aId > armyId) Destroy(gameObject);
    }

    public void InitializeDeploy(string uType, int strength, int morale, int position, int aId, int myId, string commander)
    {
        myPictureController.InitialPicture(uType);
        myWidgetController.InitalColor(aId);
        myCaptionController.InitialCaption(uType);
        mySMControllers[0].InitialSM(strength, morale);
        mySMControllers[1].InitialSM(strength, morale);
        myPosition = position;
        unitCommander = commander;
        startingPosition = transform.position;
        armyId = aId;
        unitId = myId;
        unitType = uType;
        if (position == 0)
        {
            WidgetPressed(false);
        }
        else transform.position = new Vector3(transform.position.x, transform.position.y - 42.0f, transform.position.z);
    }

    public void WidgetPressed(bool playSound)
    {
        if (!isEnlarged && !BattleManager.Instance.isInputBlocked)
        {
            if(playSound) SoundManagerController.Instance.PlayUnitReport();
            transform.DOScale(0.45f, 0.25f).SetEase(Ease.OutBack);
            transform.DOMoveX(panelMovedDummy.transform.position.x, 0.25f).SetEase(Ease.OutBack);
            transform.DOMoveY(startingPosition.y - 25.0f, 0.25f).SetEase(Ease.OutBack);
            isEnlarged = true;
            EventManager.RaiseEventOnUIDeployPressed(armyId, myPosition, unitId, unitType, unitCommander);
        }
    }
}
