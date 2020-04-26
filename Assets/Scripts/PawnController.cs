using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class PawnController : MonoBehaviour
{
    private int _unitId;

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

    private void OnMouseDown()
    {
        if (!IsPointerOverGameObject() && ((BattleManager.turnOwnerId == 1 && BattleManager.isPlayer1Human || BattleManager.turnOwnerId == 2 && BattleManager.isPlayer2Human) || BattleManager.gameMode == "deploy"))
        {
            EventManager.RaiseEventOnUnitClicked(_unitId);
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

    /*private void OnMouseOver()
    {
        EventManager.RaiseEventOnUnitClicked(_unitId);
    }

    private void OnMouseExit()
    {
        EventManager.RaiseEventOnUnitClicked(-1);
    }*/

    // called when unit receives casualty
    public void Disable()
    {
        transform.DOScale(0.0f, 0.75f).SetEase(Ease.InBack);
        //this.GetComponent<Renderer>().enabled = false;
    }

    // called whenever user clicks on unit
    public void EnableOutline()
    {
        transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f, 10, 1);
        if(BattleManager.viewType == "isometric") GetComponent<Renderer>().materials[0].SetFloat("_OutlineWidth", 0.001f);
        else GetComponent<Renderer>().materials[0].SetFloat("_OutlineWidth", 0.001f);
    }

    // called whenever user clicks on another unit
    public void DisableOutline()
    {
        GetComponent<Renderer>().materials[0].SetFloat("_OutlineWidth", 0.0f);
    }
}
