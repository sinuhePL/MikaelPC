using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArrowController : MonoBehaviour
{
    private int _attackId;
    private bool _isArrowActive;
    private MeshRenderer myRenderer;
    public const string YellowColor = "#766800";
    public const string RedColor = "#6A0006";
    [SerializeField] private Texture arrowRightTexture;
    [SerializeField] private Texture arrowForwardTexture;
    [SerializeField] private Texture arrowLeftTexture;
    [SerializeField] private Texture arrowRightEmptyTexture;
    [SerializeField] private Texture arrowForwardEmptyTexture;
    [SerializeField] private Texture arrowLeftEmptyTexture;

    private void OnMouseDown()
    {
        if (_isArrowActive && !IsPointerOverGameObject())
        {
            EventManager.RaiseEventOnAttackClicked(_attackId);
        }
    }
    // dodać obsługę zdarzenia kliknięcia na strzałkę ataku, dodać dodanych ataku współrzędne każdego ataku. usunąć poprzednie rozwiązanie

    private bool IsPointerOverGameObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
        _isArrowActive = false;
        
    }

    public void InitializeArrow(string direction, string color, string type)
    {
        Color myColor;

        if (direction == "right")
        {
            if (type == "solid") myRenderer.material.mainTexture = arrowRightTexture;
            else if(type == "empty") myRenderer.material.mainTexture = arrowRightEmptyTexture;
        }
        else if(direction == "forward")
        {
            if (type == "solid") myRenderer.material.mainTexture = arrowForwardTexture;
            else if (type == "empty") myRenderer.material.mainTexture = arrowForwardEmptyTexture;
        }
        else if(direction == "left")
        {
            if (type == "solid") myRenderer.material.mainTexture = arrowLeftTexture;
            else if (type == "empty") myRenderer.material.mainTexture = arrowLeftEmptyTexture;
        }
        myRenderer.material.EnableKeyword("_EmisColor");
        if (color == "red")
        {
            ColorUtility.TryParseHtmlString(RedColor, out myColor);
            myRenderer.material.SetColor("_EmisColor", Color.red);
        }
        else if (color == "blue")
        {
            myRenderer.material.SetColor("_EmisColor", Color.blue);
        }
        else if (color == "yellow")
        {
            ColorUtility.TryParseHtmlString(YellowColor, out myColor);
            myRenderer.material.SetColor("_EmisColor", myColor);
        }
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
