using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PawnController : MonoBehaviour
{
    private int _unitId;
    private Vector3 initialScale;

    public int UnitId
    {
        get
        {
            return _unitId;
        }
        set
        {
            _unitId = value;
        }
    }

    private void Start()
    {
        initialScale = transform.localScale;
    }

    private void OnMouseDown()
    {
        if (!IsPointerOverGameObject() && ((BattleManager.Instance.turnOwnerId == 1 && GameManagerController.Instance.isPlayer1Human || BattleManager.Instance.turnOwnerId == 2 && GameManagerController.Instance.isPlayer2Human) || BattleManager.Instance.gameMode == "deploy"))
        {
            EventManager.RaiseEventOnUnitClicked(_unitId);
            SoundManagerController.Instance.PlayUnitReport();
        }
    }

    private bool IsPointerOverGameObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // called when unit receives casualty
    public void Disable()
    {
        transform.DOScale(0.0f, 0.75f).SetEase(Ease.InBack);
    }

    public void Enable()
    {
        transform.DOScaleX(initialScale.x, 0.75f).SetEase(Ease.OutBack);
        transform.DOScaleY(initialScale.y, 0.75f).SetEase(Ease.OutBack);
        transform.DOScaleZ(initialScale.z, 0.75f).SetEase(Ease.OutBack);
    }

    // called whenever user clicks on unit
    public void EnableOutline()
    {
        transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f, 10, 1);
        if(GameManagerController.Instance.viewType == GameManagerController.viewTypeEnum.isometric) GetComponent<Renderer>().materials[0].SetFloat("_OutlineWidth", 0.002f);
        else GetComponent<Renderer>().materials[0].SetFloat("_OutlineWidth", 0.002f);
    }

    // called whenever user clicks on another unit
    public void DisableOutline()
    {
        GetComponent<Renderer>().materials[0].SetFloat("_OutlineWidth", 0.0f);
    }
}
