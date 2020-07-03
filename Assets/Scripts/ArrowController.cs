using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ArrowController : MonoBehaviour
{
    public int _attackId;
    private bool _isArrowActive;    // active arrow is displayed when owning unit is clicked
    private bool _isBlocked;         // arrow is blocked when unit is placed in support line
    private MeshRenderer myRenderer;
    private List<int> activatingAttacks;
    private string arrowType;
    private bool isShownAsCounterAttack;
    public const string YellowColor = "#766800";
    public const string RedColor = "#6A0006";
    [SerializeField] private Texture arrowRightTexture;
    [SerializeField] private Texture arrowForwardTexture;
    [SerializeField] private Texture arrowLeftTexture;
    [SerializeField] private Texture arrowRightEmptyTexture;
    [SerializeField] private Texture arrowForwardEmptyTexture;
    [SerializeField] private Texture arrowLeftEmptyTexture;
    [SerializeField] private Texture arrowFarTexture;
    [SerializeField] private Texture arrowFarEmptyTexture;

    private void OnMouseDown()
    {
        if ((_isArrowActive || isShownAsCounterAttack) && !IsPointerOverGameObject())
        {
            EventManager.RaiseEventOnAttackClicked(_attackId, !_isArrowActive);
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

    private void CheckAndShowArrow(int aId, bool isCounterAttack)   // shows empty arrow when it represents counterattack
    {
        if (!_isBlocked)
        {
            if (arrowType == "empty" && activatingAttacks.Contains(aId))
            {
                gameObject.SetActive(true);
                myRenderer.enabled = true;
                isShownAsCounterAttack = true;
                transform.position = new Vector3(transform.position.x, 0.002f, transform.position.z);
            }
            else if (arrowType == "solid" && aId != AttackId)
            {
                myRenderer.enabled = false;
                transform.position = new Vector3(transform.position.x, -0.002f, transform.position.z);
            }
            else if (arrowType == "empty" && !isArrowActive)
            {
                myRenderer.enabled = false;
                transform.position = new Vector3(transform.position.x, -0.002f, transform.position.z);
            }
        }
    }

    private void AnyAttackOrdered(int aId)
    {
        if (aId != _attackId) myRenderer.enabled = false;
    }

    private void Awake()
    {
        myRenderer = GetComponent<MeshRenderer>();
        myRenderer.enabled = false;
        _isArrowActive = false;
        activatingAttacks = new List<int>();
        arrowType = "";
        isShownAsCounterAttack = false;
    }

    private void OnEnable()
    {
        EventManager.onAttackClicked += CheckAndShowArrow;
        EventManager.onAttackOrdered += AnyAttackOrdered;
    }

    private void OnDestroy()
    {
        EventManager.onAttackClicked -= CheckAndShowArrow;
        EventManager.onAttackOrdered -= AnyAttackOrdered;
    }

    public void InitializeArrow(string direction, string color, string type)
    {
        Color myColor;

        arrowType = type;
        _isBlocked = false;
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
        else if (direction == "far")
        {
            if (type == "solid") myRenderer.material.mainTexture = arrowFarTexture;
            else if (type == "empty") myRenderer.material.mainTexture = arrowFarEmptyTexture;
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
        get { return _isArrowActive; }
        set { _isArrowActive = value; }
    }

    public bool isArrowBlocked
    {
        get { return _isBlocked; }
        set { _isBlocked = value; }
    }

    public void ShowArrow()
    {
        if (isArrowActive)
        {
            myRenderer.enabled = true;
            transform.position = new Vector3(transform.position.x, 0.002f, transform.position.z);
        }
    }

    public void HideArrow()
    {
        myRenderer.enabled = false;
        isShownAsCounterAttack = false;
        transform.position = new Vector3(transform.position.x, -0.002f, transform.position.z);
    }

    public void ShowArrow(int aId, bool isCounterAttack)
    {
        if (isCounterAttack && arrowType == "solid" && AttackId == aId)
        {
            myRenderer.enabled = true;
            gameObject.SetActive(true);
            transform.position = new Vector3(transform.position.x, 0.002f, transform.position.z);
            return;
        }
        if (isArrowActive && AttackId == aId)
        {
            myRenderer.enabled = true;
            transform.position = new Vector3(transform.position.x, 0.002f, transform.position.z);
        }
    }

    public void HideArrow(int aId)
    {
        if (isArrowActive && AttackId == aId)
        {
            myRenderer.enabled = false;
            transform.position = new Vector3(transform.position.x, -0.002f, transform.position.z);
        }
    }

    public void AddActivatingAttack(int a)
    {
        activatingAttacks.Add(a);
    }
}
