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
    [SerializeField] private ParticleSystem topParticleSystem;
    [SerializeField] private ParticleSystem rightParticleSystem;
    [SerializeField] private ParticleSystem bottomParticleSystem;
    [SerializeField] private ParticleSystem leftParticleSystem;
    [SerializeField] private Texture forest1Texture;
    [SerializeField] private Texture hill1Texture;
    [SerializeField] private Texture hill2Texture;
    [SerializeField] private Texture town1Texture;
    [SerializeField] private Texture field1Texture;
    [SerializeField] private Texture field2Texture;
    [SerializeField] private TextMeshPro tileInfluenceDescription;


    private TextMeshPro fieldCaption;
    private int keyFieldId;
    private bool isBlinking;
    private int ownerId;
    private int lastClickedUnit;
    private int myDeployedUnitId;
    private int possibleArmyDeployment;
    private string tileType;
    // Start is called before the first frame update

    public void InitializeTile(int ti, int kfid, string tType, int pArmDpl)
    {
        int randomInt;
        myRenderer = GetComponent<MeshRenderer>();
        tileId = ti;
        keyFieldId = kfid;
        pawn.SetActive(false);
        fieldCaption = GetComponentInChildren<TextMeshPro>();
        isBlinking = false;
        ownerId = 0;
        myDeployedUnitId = 0;
        lastClickedUnit = 0;
        possibleArmyDeployment = pArmDpl;
        tileType = tType;
        tileInfluenceDescription.gameObject.SetActive(false);
        if(possibleArmyDeployment > 0)
        {
            if(possibleArmyDeployment == 1)
            {
                topParticleSystem.startColor = Color.blue;
                bottomParticleSystem.startColor = Color.blue;
                rightParticleSystem.startColor = Color.blue;
                leftParticleSystem.startColor = Color.blue;
            }
            else if (possibleArmyDeployment == 2)
            {
                topParticleSystem.startColor = Color.yellow;
                bottomParticleSystem.startColor = Color.yellow;
                rightParticleSystem.startColor = Color.yellow;
                leftParticleSystem.startColor = Color.yellow;
            }
        }
        if (keyFieldId == 0)
        {
            fieldCaption.text = "";
            border.SetActive(false);
            floor.SetActive(false);
        }
        randomInt = Random.Range(1, 3);
        switch (tileType)
        {
            case "town":
                myRenderer.material.mainTexture = town1Texture;
                if(keyFieldId != 0) fieldCaption.text = "Mirabello Castle";
                break;
            case "forest":
                myRenderer.material.mainTexture = forest1Texture;
                if (keyFieldId != 0) fieldCaption.text = "Deep Forest";
                break;
            case "field":
                if (randomInt == 1) myRenderer.material.mainTexture = field1Texture;
                if (randomInt == 2) myRenderer.material.mainTexture = field2Texture;
                if (keyFieldId != 0) fieldCaption.text = "Ogden's Field";
                break;
            case "hill":
                if (randomInt == 1) myRenderer.material.mainTexture = hill1Texture;
                if (randomInt == 2) myRenderer.material.mainTexture = hill2Texture;
                if (keyFieldId != 0) fieldCaption.text = "Windy Peak";
                break;
        }
    }

    private void OnMouseDown()
    {
        if(BattleManager.Instance.gameMode == "deploy")
        {
            if(!IsPointerOverGameObject() && myDeployedUnitId == 0 && BattleManager.Instance.turnOwnerId == possibleArmyDeployment)
            {
                EventManager.RaiseEventOnUnitDeployed(lastClickedUnit, tileId);
            }
        }
        else if (!IsPointerOverGameObject() && !BattleManager.Instance.isInputBlocked)
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

    private void SetLastClickedUnit(int a, int p, int uId, string uType)
    {
        lastClickedUnit = uId;
        if(a == possibleArmyDeployment && myDeployedUnitId == 0)
        {
            tileInfluenceDescription.gameObject.SetActive(true);
            tileInfluenceDescription.text = "";
            if (tileType == "hill")
            {
                if (uType == "Imperial Cavalery" || uType == "Gendarmes") tileInfluenceDescription.text = "+1 Attack Die";
                tileInfluenceDescription.text += "\n\n+1 Defence Die";
            }
            if(tileType == "forest")
            {
                if(uType == "Arquebusiers") tileInfluenceDescription.text = "+1 Attack Die";
                if (uType == "Suisse" || uType == "Landsknechte" || uType == "Arquebusiers" || uType == "Artillery") tileInfluenceDescription.text += "\n\n+1 Defence Die against Cavalery";
            }
            if(tileType == "town")
            {
                if (uType == "Suisse" || uType == "Landsknechte" || uType == "Arquebusiers") tileInfluenceDescription.text += "\n\n+1 Defence Die against Arquebusiers and Artillery";
            }
        }
        else tileInfluenceDescription.gameObject.SetActive(false);
    }

    private void anyUnitDeployed(int uId, int tId)
    {
        if (uId == myDeployedUnitId && tId != tileId)
        {
            myDeployedUnitId = 0;
            tileInfluenceDescription.gameObject.SetActive(true);
        }
        if (myDeployedUnitId == 0 && tId == tileId)
        {
            myDeployedUnitId = uId;
            tileInfluenceDescription.gameObject.SetActive(false);
        }       
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

    private void HighlightField(int aId)
    {
        if(aId == possibleArmyDeployment)
        {
            topParticleSystem.Play();
            bottomParticleSystem.Play();
            if(BattleManager.Instance.boardHeight + 2 == tileId && aId == 2 || BattleManager.Instance.boardHeight + 4 == tileId && aId == 1) leftParticleSystem.Play();
            if ((BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth) - BattleManager.Instance.boardHeight - 1 == tileId && aId == 1 ||
            (BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth) - BattleManager.Instance.boardHeight - 3 == tileId && aId == 2)
            {
                rightParticleSystem.Play();
            }
        }
        else
        {
            topParticleSystem.Stop();
            bottomParticleSystem.Stop();
            rightParticleSystem.Stop();
            leftParticleSystem.Stop();
            topParticleSystem.Clear();
            bottomParticleSystem.Clear();
            rightParticleSystem.Clear();
            leftParticleSystem.Clear();
            tileInfluenceDescription.gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        EventManager.onDiceResult += ChangeFieldOwner;
        EventManager.onAttackClicked += BlinkKeyField;
        EventManager.onTileClicked += StopBlinking;
        EventManager.onUnitClicked += StopBlinking;
        EventManager.onAttackOrdered += StopBlinking;
        EventManager.onUIDeployPressed += SetLastClickedUnit;
        EventManager.onUnitDeployed += anyUnitDeployed;
        EventManager.onDeploymentStart += HighlightField;
    }

    private void OnDestroy()
    {
        EventManager.onDiceResult -= ChangeFieldOwner;
        EventManager.onAttackClicked -= BlinkKeyField;
        EventManager.onTileClicked -= StopBlinking;
        EventManager.onUnitClicked -= StopBlinking;
        EventManager.onAttackOrdered -= StopBlinking;
        EventManager.onUIDeployPressed -= SetLastClickedUnit;
        EventManager.onUnitDeployed -= anyUnitDeployed;
        EventManager.onDeploymentStart -= HighlightField;
    }

    public int GetKeyFieldId()
    {
        return keyFieldId;
    }

    public string GetKeyFieldName()
    {
        return fieldCaption.text;
    }

    public int ChangeAttackStrength(string uType)
    {
        if (tileType == "hill")
        {
            if (uType == "Imperial Cavalery" || uType == "Gendarmes") return 1;
        }
        if (tileType == "forest")
        {
            if (uType == "Arquebusiers") return 1;
        }
        return 0;
    }

    public int ChangeDefenceStrength(string defenderType, string attackerType)
    {
        if (tileType == "hill")
        {
            return 1;
        }
        if (tileType == "forest")
        {
            if ((defenderType == "Suisse" || defenderType == "Landsknechte" || defenderType == "Arquebusiers" || defenderType == "Artillery") && (attackerType == "Gendarmes" || attackerType == "Imperial Cavalery")) return 1;
        }
        if (tileType == "town")
        {
            if ((defenderType == "Suisse" || defenderType == "Landsknechte" || defenderType == "Arquebusiers") && (attackerType == "Arquebusiers" || attackerType == "Artillery")) return 1;
        }
        return 0;
    }

    public bool DeploymentPossible(int aId)
    {
        if (aId == possibleArmyDeployment) return true;
        else return false;
    }
}
