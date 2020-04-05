using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TileController : MonoBehaviour
{
    public int tileId;
    private MeshRenderer myRenderer;

    [SerializeField] private Material imperialMaterial;
    [SerializeField] private Material frenchMaterial;
    [SerializeField] private GameObject pawn;
    [SerializeField] private GameObject border;
    [SerializeField] private GameObject floor;
    [SerializeField] private Texture forest1Texture;
    [SerializeField] private Texture hill1Texture;
    [SerializeField] private Texture hill2Texture;
    [SerializeField] private Texture town1Texture;
    [SerializeField] private Texture field1Texture;
    [SerializeField] private Texture field2Texture;

    private TextMeshPro fieldCaption;
    private int keyFieldId;
    private bool isBlinking;
    private int ownerId;
    // Start is called before the first frame update

    public void InitializeTile(int i, int id, string tileType)
    {
        myRenderer = GetComponent<MeshRenderer>();
        tileId = i;
        keyFieldId = id;
        pawn.SetActive(false);
        fieldCaption = GetComponentInChildren<TextMeshPro>();
        isBlinking = false;
        ownerId = 0;
        if (keyFieldId == 0)
        {
            fieldCaption.text = "";
            border.SetActive(false);
            floor.SetActive(false);
        }
        else
        {
            switch (tileType)
            {
                case "town":
                    fieldCaption.text = "Mirabello Castle";
                    myRenderer.material.mainTexture = town1Texture;
                    break;
                case "forest":
                    fieldCaption.text = "Deep Forest";
                    myRenderer.material.mainTexture = forest1Texture;
                    break;
                case "field":
                    fieldCaption.text = "Ogden's Field";
                    myRenderer.material.mainTexture = field1Texture;
                    break;
                case "hill":
                    fieldCaption.text = "Windy Peak";
                    myRenderer.material.mainTexture = hill1Texture;
                    break;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!IsPointerOverGameObject() && !BattleManager.isInputBlocked)
        {
            EventManager.RaiseEventOnTileClicked(tileId);
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

    private void ChangeFieldOwner(StateChange sc)
    {
        if(sc.keyFieldChangeId == keyFieldId)
        {
            ownerId = sc.keyFieldNewOccupantId;
            if (ownerId == 1)
            {
                pawn.SetActive(true);
                pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            }
            else if(ownerId == 2)
            {
                pawn.SetActive(true);
                pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            }
        }
    }

    private IEnumerator Blink()
    {
        while(isBlinking)
        {
            if (ownerId == 0) pawn.SetActive(true);
            else if (ownerId == 1) pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            else if (ownerId == 2) pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            yield return new WaitForSeconds(0.5f);
            if(ownerId == 0) pawn.SetActive(false);
            else if (ownerId == 1) pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            else if (ownerId == 2) pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void BlinkKeyField(int idAttack, bool isCounterAttack)
    {
        Attack tempAttack;
        tempAttack = BattleManager.Instance.GetAttack(idAttack);
        if(tempAttack.GetKeyFieldId() == keyFieldId && keyFieldId != 0 && ownerId != tempAttack.GetArmyId())
        {
            isBlinking = true;
            if (tempAttack.GetArmyId() == 1) pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            else pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            StartCoroutine(Blink());
        }
        else
        {
            isBlinking = false;
        }
    }

    private void StopBlinking(int i)
    {
        isBlinking = false;
        if (ownerId == 1)
        {
            pawn.SetActive(true);
            pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
        }
        else if (ownerId == 2)
        {
            pawn.SetActive(true);
            pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
        }
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += ChangeFieldOwner;
        EventManager.onAttackClicked += BlinkKeyField;
        EventManager.onTileClicked += StopBlinking;
        EventManager.onUnitClicked += StopBlinking;
        EventManager.onAttackOrdered += StopBlinking;
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= ChangeFieldOwner;
        EventManager.onAttackClicked -= BlinkKeyField;
        EventManager.onTileClicked -= StopBlinking;
        EventManager.onUnitClicked -= StopBlinking;
        EventManager.onAttackOrdered -= StopBlinking;
    }

    public int GetKeyFieldId()
    {
        return keyFieldId;
    }

    public string GetKeyFieldName()
    {
        return fieldCaption.text;
    }
}
