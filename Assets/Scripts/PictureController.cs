using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PictureController : MonoBehaviour
{
    [SerializeField] private Sprite cavalery;
    private Image myImage;
    private int lastClickedUnit;
    private bool isHidden;
    // Start is called before the first frame update

    private void UnitClicked(int unitId)
    {
        Unit tempUnit = null;
        tempUnit = BattleManager.Instance.GetUnit(unitId);
        switch (tempUnit.GetUnitType())
        {
            case "French Cavalery":
                myImage.sprite = cavalery;
                break;
        }
        if (unitId != lastClickedUnit || isHidden)
        {
            transform.DOMoveX(205.0f, 0.3f).SetEase(Ease.OutBack);
            lastClickedUnit = unitId;
            isHidden = false;
        }
        else
        {
            transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
            isHidden = true;
        }
    }

    private void TileClicked(int idTile)
    {
        transform.DOMoveX(-190.0f, 0.3f).SetEase(Ease.InBack);
        isHidden = true;
    }

    private void UpdateBoard()
    {
        TileClicked(0);
    }

    private void OnDestroy()
    {
        EventManager.onUnitClicked -= UnitClicked;
        EventManager.onTileClicked -= TileClicked;
        EventManager.onUpdateBoard -= UpdateBoard;
    }

    private void OnEnable()
    {
        EventManager.onUnitClicked += UnitClicked;
        EventManager.onTileClicked += TileClicked;
        EventManager.onUpdateBoard += UpdateBoard;
    }

    void Start()
    {
        myImage = GetComponent<Image>();
        lastClickedUnit = -1;
        isHidden = true;
    }
}
