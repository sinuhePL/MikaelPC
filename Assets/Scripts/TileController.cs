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
    // Start is called before the first frame update

    public void InitializeTile(int i, int id, string tileType)
    {
        myRenderer = GetComponent<MeshRenderer>();
        tileId = i;
        keyFieldId = id;
        pawn.SetActive(false);
        fieldCaption = GetComponentInChildren<TextMeshPro>();
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
            if(sc.keyFieldNewOccupantId == 1)
            {
                pawn.SetActive(true);
                pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            }
            else if(sc.keyFieldNewOccupantId == 2)
            {
                pawn.SetActive(true);
                pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            }
        }
    }

    private void OnEnable()
    {
        EventManager.onDiceResult += ChangeFieldOwner;
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= ChangeFieldOwner;
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
