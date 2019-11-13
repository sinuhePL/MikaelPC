using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArrowController : MonoBehaviour
{
    private int _attackId;
    private bool _isArrowActive;
    private Renderer myRenderer;

    private void OnMouseDown()
    {
        if (_isArrowActive && !EventSystem.current.IsPointerOverGameObject())
        {
            EventManager.RaiseEventOnAttackClicked(_attackId);
        }
    }
    // dodać obsługę zdarzenia kliknięcia na strzałkę ataku, dodać dodanych ataku współrzędne każdego ataku. usunąć poprzednie rozwiązanie

    public void Awake()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.enabled = false;
        _isArrowActive = false;
    }

    public int AttackId
    {
        get
        {
            return _attackId;
        }
        set
        {
            _attackId = value;
        }
    }

    public bool isArrowActive
    {
        get
        {
            return _isArrowActive;
        }
        set
        {
            _isArrowActive = value;
        }
    }

    public void ShowArrow()
    {
        if(isArrowActive) myRenderer.enabled = true;
    }

    public void HideArrow()
    {
        if (isArrowActive)
        {
            myRenderer.enabled = false;
        }
    }

    public void ShowArrow(int aId)
    {
        if (isArrowActive && AttackId == aId) myRenderer.enabled = true;
        else myRenderer.enabled = false;
    }

    public void HideArrow(int aId)
    {
        if (isArrowActive && AttackId == aId)
        {
            myRenderer.enabled = false;
        }
    }
}
