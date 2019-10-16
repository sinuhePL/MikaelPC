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
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            EventManager.RaiseEventOnUnitClicked(_unitId);
        }
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
        transform.DOScale(0.0f, 0.75f).SetEase(Ease.InElastic);
        //this.GetComponent<Renderer>().enabled = false;
    }

    // called whenever user clicks on unit
    public void EnableOutline()
    {
        transform.DOPunchScale(new Vector3(1.5f, 1.5f, 1.5f), 0.5f, 10, 1);
        GetComponent<Renderer>().materials[0].SetFloat("_Outline", 0.2f);
    }

    // called whenever user clicks on another unit
    public void DisableOutline()
    {
        GetComponent<Renderer>().materials[0].SetFloat("_Outline", 0.0f);
    }
}
