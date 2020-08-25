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
    [SerializeField] private TextMeshPro tileTypeText;


    private TextMeshPro fieldCaption;
    private int keyFieldId;
    private bool isBlinking;
    private int ownerId;
    private int lastClickedUnit;
    private int myDeployedUnitId;
    private int possibleArmyDeployment;
    private string tileType;
    private bool initiallyDeploymentNotPossible;
    private int forwardUnitId;
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
        if(pArmDpl == 0) initiallyDeploymentNotPossible = true;
        else initiallyDeploymentNotPossible = false;
        forwardUnitId = 0;
        tileType = tType;
        tileTypeText.text = tType;
        tileTypeText.gameObject.SetActive(false);
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
            case "Town":
                myRenderer.material.mainTexture = town1Texture;
                if(keyFieldId != 0) fieldCaption.text = "Mirabello Castle";
                break;
            case "Forest":
                myRenderer.material.mainTexture = forest1Texture;
                if (keyFieldId != 0) fieldCaption.text = "Deep Forest";
                break;
            case "Field":
                if (randomInt == 1) myRenderer.material.mainTexture = field1Texture;
                if (randomInt == 2) myRenderer.material.mainTexture = field2Texture;
                if (keyFieldId != 0) fieldCaption.text = "Ogden's Field";
                break;
            case "Hill":
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
            if(!IsPointerOverGameObject() && myDeployedUnitId == 0 && BattleManager.Instance.turnOwnerId == possibleArmyDeployment && lastClickedUnit != forwardUnitId && !BattleManager.Instance.isUnitControllerBlocked(lastClickedUnit))
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

    private void SetLastClickedUnit(int a, int p, int uId, string uType, string commander)
    {
        TileController tc;

        lastClickedUnit = uId;
        if (a == possibleArmyDeployment && myDeployedUnitId == 0 && !initiallyDeploymentNotPossible)
        {
            tc = null;
            if (a == 1) tc = BattleManager.Instance.GetTile(tileId - 2);
            else tc = BattleManager.Instance.GetTile(tileId + 2);
            tileInfluenceDescription.gameObject.SetActive(true);
            tileTypeText.gameObject.SetActive(true);
            tileInfluenceDescription.text = "";
            if (uType == "Imperial Cavalery" || uType == "Gendarmes")
            {
                if (tc.tileType == "Hill") tileInfluenceDescription.text = "-1 Attack Die";
                else tileInfluenceDescription.text = "";
            }
            if (uType == "Artillery")
            {
                if (tc.tileType == "Town") tileInfluenceDescription.text = "+1 Attack Die";
                else tileInfluenceDescription.text = "";
            }
            if (uType == "Arquebusiers")
            {
                if (tileType == "Forest") tileInfluenceDescription.text = "+1 Defence Die";
            }
            if (tileType == "Hill")
            {
                if (a == 1) tileInfluenceDescription.text += "-1 Attack Die for opposing Imperial Cavalry\n\n";
                if (a == 2) tileInfluenceDescription.text += "-1 Attack Die for opposing Gendarmes\n\n";
            }
            if (tileType == "Town")
            {
                tileInfluenceDescription.text += "+1 Attack Die for opposing Artillery\n\n";
            }
            if (tc.tileType == "Forest") tileInfluenceDescription.text += "+1 Defence Die for opposing Arquebusiers\n\n";
            /*if(tileType == "forest")
            {
                if(uType == "Arquebusiers") tileInfluenceDescription.text = "+1 Attack Die";
                if (uType == "Suisse" || uType == "Landsknechte" || uType == "Arquebusiers" || uType == "Artillery") tileInfluenceDescription.text += "\n\n+1 Defence Die against Cavalery";
            }
            if(tileType == "town")
            {
                if (uType == "Suisse" || uType == "Landsknechte" || uType == "Arquebusiers") tileInfluenceDescription.text += "\n\n+1 Defence Die against Arquebusiers and Artillery";
            }*/
        }
        else
        {
            tileInfluenceDescription.gameObject.SetActive(false);
            tileTypeText.gameObject.SetActive(false);
        }
    }

    private void anyUnitDeployed(int uId, int tId)
    {
        TileController tc;

        if (uId == myDeployedUnitId && tId != tileId)   // if unit moved to another tile
        {
            myDeployedUnitId = 0;
            if (!initiallyDeploymentNotPossible)
            {
                tileInfluenceDescription.gameObject.SetActive(true);
                if(BattleManager.Instance.turnOwnerId == 2) topParticleSystem.Play();
                if(BattleManager.Instance.turnOwnerId == 1) bottomParticleSystem.Play();
            }
        }
        else if (myDeployedUnitId == 0 && tId == tileId)    // if unit placed on this tile
        {
            myDeployedUnitId = uId;
            tileInfluenceDescription.gameObject.SetActive(false);
            if (BattleManager.Instance.turnOwnerId == 1 && (tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 2)
            {
                bottomParticleSystem.Stop();
                bottomParticleSystem.Clear();
            }
            if (BattleManager.Instance.turnOwnerId == 2 && (tileId - 1) % BattleManager.Instance.boardHeight == 1)
            {
                topParticleSystem.Stop();
                topParticleSystem.Clear();
            }
        }
        if(possibleArmyDeployment == 0) // checks if deployed unit was deployed on tile in front of this tile
        {
            if (BattleManager.Instance.turnOwnerId == 1 && tileId == tId + 1 && (tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1)
            {
                bottomParticleSystem.Play();
                tc = BattleManager.Instance.GetTile(tileId - BattleManager.Instance.boardHeight);   // looks for left neighbour
                if (!tc.DeploymentPossible(1) || tc.GetForwardUnitId() == uId) leftParticleSystem.Play();
                else tc.DisableHighlight("right");
                tc = BattleManager.Instance.GetTile(tileId + BattleManager.Instance.boardHeight);   // looks for right neighbour
                if (!tc.DeploymentPossible(1) || tc.GetForwardUnitId() == uId) rightParticleSystem.Play();
                else tc.DisableHighlight("left");
                possibleArmyDeployment = 1;
                forwardUnitId = uId;
            }
            if (BattleManager.Instance.turnOwnerId == 2 && tileId == tId - 1 && (tileId - 1) % BattleManager.Instance.boardHeight == 0)
            {
                topParticleSystem.startColor = Color.yellow;
                topParticleSystem.Play();
                tc = BattleManager.Instance.GetTile(tileId - BattleManager.Instance.boardHeight);
                if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)  // looks for left neighbour
                {
                    leftParticleSystem.startColor = Color.yellow;
                    leftParticleSystem.Play();
                }
                else tc.DisableHighlight("right");
                tc = BattleManager.Instance.GetTile(tileId + BattleManager.Instance.boardHeight);   // looks for right neighbour
                if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)
                {
                    rightParticleSystem.startColor = Color.yellow;
                    rightParticleSystem.Play();
                }
                else tc.DisableHighlight("left");
                possibleArmyDeployment = 2;
                forwardUnitId = uId;
            }
        }
        else if(possibleArmyDeployment != 0 && initiallyDeploymentNotPossible)  // if tile has unit on tile in front of it
        {
            if (forwardUnitId == uId)   // if moved unit in front of tile
            {
                leftParticleSystem.Stop();
                leftParticleSystem.Clear();
                rightParticleSystem.Stop();
                rightParticleSystem.Clear();
                if (BattleManager.Instance.turnOwnerId == 1)
                {
                    bottomParticleSystem.Stop();
                    bottomParticleSystem.Clear();
                    possibleArmyDeployment = 0;
                    forwardUnitId = 0;
                }
                if (BattleManager.Instance.turnOwnerId == 2)
                {
                    topParticleSystem.Stop();
                    topParticleSystem.Clear();
                    possibleArmyDeployment = 0;
                    forwardUnitId = 0;
                }
            }
            else
            {
                if (BattleManager.Instance.turnOwnerId == 1 && (tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1)
                {
                    tc = BattleManager.Instance.GetTile(tileId - BattleManager.Instance.boardHeight);   // looks for left neighbour
                    if (!tc.DeploymentPossible(1) || tc.GetForwardUnitId() == uId) leftParticleSystem.Play();
                    tc = BattleManager.Instance.GetTile(tileId + BattleManager.Instance.boardHeight);   // looks for right neighbour
                    if (!tc.DeploymentPossible(1) || tc.GetForwardUnitId() == uId) rightParticleSystem.Play();
                    if(tId == tileId - 1 - BattleManager.Instance.boardHeight)
                    {
                        leftParticleSystem.Stop();
                        leftParticleSystem.Clear();
                    }
                    if(tId == tileId - 1 + BattleManager.Instance.boardHeight)
                    {
                        rightParticleSystem.Stop();
                        rightParticleSystem.Clear();
                    }
                }
                if (BattleManager.Instance.turnOwnerId == 2 && (tileId - 1) % BattleManager.Instance.boardHeight == 0)
                {
                    tc = BattleManager.Instance.GetTile(tileId - BattleManager.Instance.boardHeight);
                    if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)  // looks for left neighbour
                    {
                        leftParticleSystem.startColor = Color.yellow;
                        leftParticleSystem.Play();
                    }
                    tc = BattleManager.Instance.GetTile(tileId + BattleManager.Instance.boardHeight);   // looks for right neighbour
                    if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)
                    {
                        rightParticleSystem.startColor = Color.yellow;
                        rightParticleSystem.Play();
                    }
                    if (tId == tileId + 1 - BattleManager.Instance.boardHeight)
                    {
                        leftParticleSystem.Stop();
                        leftParticleSystem.Clear();
                    }
                    if (tId == tileId + 1 + BattleManager.Instance.boardHeight)
                    {
                        rightParticleSystem.Stop();
                        rightParticleSystem.Clear();
                    }
                }
            }
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
        TileController tc;

        tc = null;
        if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 2)    // if tile in first line of army 1
        {
            tc = BattleManager.Instance.GetTile(tileId - 2);
        }
        else if ((tileId - 1) % BattleManager.Instance.boardHeight == 1)    // if tile in first line of army 2
        {
            tc = BattleManager.Instance.GetTile(tileId + 2);
        }
        if (uType == "Imperial Cavalery" || uType == "Gendarmes")
        {
            if (tc.tileType == "Hill") return -1;
            else return 0;
        }
        if(uType == "Artillery")
        {
            if (tc.tileType == "Town") return 1;
            else return 0;
        }
        return 0;
    }

    public int ChangeDefenceStrength(string defenderType, string attackerType)
    {
        if (tileType == "Forest")
        {
            if (defenderType == "Arquebusiers") return 1;
        }
        /*if (tileType == "hill")
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
        }*/
        return 0;
    }

    public bool DeploymentPossible(int aId)
    {
        if (aId == possibleArmyDeployment) return true;
        else return false;
    }

    public int GetUnitValue(string uType)
    {
        TileController tc;
        int score;

        score = 50;
        tc = null;
        if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 2)    // if tile in first line of army 1
        {
            tc = BattleManager.Instance.GetTile(tileId - 2);
        }
        else if ((tileId - 1) % BattleManager.Instance.boardHeight == 1)    // if tile in first line of army 2
        {
            tc = BattleManager.Instance.GetTile(tileId + 2);
        }
        if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (tileId - 1) % BattleManager.Instance.boardHeight == 0) // if tile in second line
        {
            if (uType == "Coustilliers" || uType == "Stradioti") return score - 10;
            else return score + 10;
        }
        if (tileType == "Hill") score -= 10;
        if (tileType == "Town") score += 10;
        /*if (uType == "Imperial Cavalery" || uType == "Gendarmes")
        {
            if (tc.tileType == "hill") score += 20;
        }
        else if(uType == "Artillery")
        {
            if (tc.tileType == "town") score -= 30;
        }*/
        return score;

        /*if(tileType == "forest")
        {
            if (uType == "Arquebusiers") return 10;
            if (uType == "Suisse" || uType == "Landsknechte" || uType == "Artillery") return 40;
        }
        if(tileType == "town")
        {
            if (uType == "Suisse" || uType == "Landsknechte" || uType == "Arquebusiers") return 30;
        }*/
    }

    public int GetOpposingUnitValue(string uType)
    {
        if (uType == "Imperial Cavalery" || uType == "Gendarmes")
        {
            if (tileType == "Hill") return 20;
        }
        else if (uType == "Artillery")
        {
            if (tileType == "Town") return -30;
        }
        return 0;
        /*if (tileType == "hill")
        {
            if (uType == "Gendarmes" || uType == "Imperial Cavalery") return 20;
            if (uType == "Artillery") return 80;
            if (uType == "Landsknechte" || uType == "Suisse") return 30;
            else return 50;*/
        /*if (tileType == "forest")
        {
            if (uType == "Gendarmes" || uType == "Imperial Cavalery") return 40;
            else return 10;
        }
        if (tileType == "town")
        {
            if (uType == "Artillery" || uType == "Arquebusiers") return 20;
            else return 0;
        }*/
    }

    public void DisableHighlight(string side)
    {
        if(side == "right")
        {
            rightParticleSystem.Stop();
            rightParticleSystem.Clear();
        }
        if(side == "left")
        {
            leftParticleSystem.Stop();
            leftParticleSystem.Clear();
        }
    }

    public int GetForwardUnitId()
    {
        return forwardUnitId;
    }
}
