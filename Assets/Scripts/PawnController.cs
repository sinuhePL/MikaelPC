using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        //Debug.Log("Clicked me! Id: " + _unitId);
        EventManager.RaiseEventOnUnitClicked(_unitId);
    }

    /*private void OnMouseOver()
    {
        EventManager.RaiseEventOnUnitClicked(_unitId);
    }

    private void OnMouseExit()
    {
        EventManager.RaiseEventOnUnitClicked(-1);
    }*/

    public void Disable()
    {
        transform.DOScale(0.0f, 0.75f).SetEase(Ease.InElastic);
        //this.GetComponent<Renderer>().enabled = false;
    }

    // called whenever user clicks on unit
    public void EnableOutline()
    {
        transform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.75f, 10, 1);
        GetComponent<Renderer>().materials[0].SetFloat("_Outline", 0.003f);
    }

    // called whenever user clicks on another unit
    public void DisableOutline()
    {
        GetComponent<Renderer>().materials[0].SetFloat("_Outline", 0.0f);
    }
}
