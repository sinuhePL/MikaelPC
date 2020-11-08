using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

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
    [SerializeField] private Texture riverTexture;
    [SerializeField] private Texture pavia1Texture;
    [SerializeField] private Texture pavia2Texture;
    [SerializeField] private Texture castleTexture;
    [SerializeField] private Texture[] wallBottomTexture;
    [SerializeField] private Texture[] wallRightTexture;
    [SerializeField] private Texture[] wallTopTexture;
    [SerializeField] private Texture wallLeft1Texture;
    [SerializeField] private Texture wallLeft2Texture;
    [SerializeField] private Texture wallLeftBottomTexture;
    [SerializeField] private Texture wallLeftTopTexture;
    [SerializeField] private Texture wallRightTopTexture;
    [SerializeField] private Texture wallRightBottomTexture;
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
        if (GameManagerController.Instance.terrainType == GameManagerController.terrainTypeEnum.random) keyFieldId = kfid;
        else keyFieldId = 0;
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
            if(possibleArmyDeployment == 2)
            {
                topParticleSystem.startColor = Color.blue;
                bottomParticleSystem.startColor = Color.blue;
                rightParticleSystem.startColor = Color.blue;
                leftParticleSystem.startColor = Color.blue;
            }
            else if (possibleArmyDeployment == 1)
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
        if (ti < 6)
        {
            if (ti == 3) myRenderer.material.mainTexture = pavia1Texture;
            else myRenderer.material.mainTexture = riverTexture;
        }
        else if (ti < 11)
        {
            if (ti == 6) myRenderer.material.mainTexture = wallLeftTopTexture;
            if (ti == 7) myRenderer.material.mainTexture = wallLeft1Texture;
            if (ti == 8) myRenderer.material.mainTexture = pavia2Texture;
            if (ti == 9) myRenderer.material.mainTexture = wallLeft2Texture;
            if (ti == 10) myRenderer.material.mainTexture = wallLeftBottomTexture;
        }
        else if (ti == (BattleManager.Instance.boardHeight) * (BattleManager.Instance.boardWidth-1) + 1) myRenderer.material.mainTexture = wallRightTopTexture;
        else if (ti == BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth) myRenderer.material.mainTexture = wallRightBottomTexture;
        else if (ti > BattleManager.Instance.boardHeight * (BattleManager.Instance.boardWidth-1) + 1) myRenderer.material.mainTexture = wallRightTexture[Random.Range(0, wallRightTexture.Length)];
        else if ((ti - 1) % BattleManager.Instance.boardHeight == 0) myRenderer.material.mainTexture = wallTopTexture[Random.Range(0, wallTopTexture.Length)];
        else if ((ti - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) myRenderer.material.mainTexture = wallBottomTexture[Random.Range(0, wallBottomTexture.Length)];
        else
        {
            if (GameManagerController.Instance.terrainType == GameManagerController.terrainTypeEnum.random)
            {
                randomInt = Random.Range(1, 3);
                switch (tileType)
                {
                    case "Town":
                        myRenderer.material.mainTexture = town1Texture;
                        if (keyFieldId != 0) fieldCaption.text = "Mirabello Castle";
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
            else
            {
                fieldCaption.text = "";
                border.SetActive(false);
                floor.SetActive(false);
                if (SceneManager.GetActiveScene().name == "Tutorial")
                {
                    switch (ti)
                    {
                        case 12:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 13:
                            myRenderer.material.mainTexture = field2Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 14:
                            myRenderer.material.mainTexture = forest1Texture;
                            tileType = "Forest";
                            tileTypeText.text = "Forest";
                            break;
                        case 17:
                            myRenderer.material.mainTexture = hill1Texture;
                            tileType = "Hill";
                            tileTypeText.text = "Hill";
                            break;
                        case 18:
                            myRenderer.material.mainTexture = castleTexture;
                            tileType = "Town";
                            tileTypeText.text = "Town";
                            fieldCaption.text = "Mirabello Castle";
                            border.SetActive(true);
                            floor.SetActive(true);
                            keyFieldId = 1;
                            break;
                        case 19:
                            myRenderer.material.mainTexture = field2Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                    }
                }
                else
                {
                    switch (ti)
                    {
                        case 12:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 13:
                            myRenderer.material.mainTexture = field2Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 14:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 17:
                            myRenderer.material.mainTexture = forest1Texture;
                            tileType = "Forest";
                            tileTypeText.text = "Forest";
                            break;
                        case 18:
                            myRenderer.material.mainTexture = field2Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 19:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 22:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 23:
                            myRenderer.material.mainTexture = castleTexture;
                            tileType = "Town";
                            tileTypeText.text = "Town";
                            fieldCaption.text = "Mirabello Castle";
                            border.SetActive(true);
                            floor.SetActive(true);
                            keyFieldId = 1;
                            break;
                        case 24:
                            myRenderer.material.mainTexture = forest1Texture;
                            tileType = "Forest";
                            tileTypeText.text = "Forest";
                            break;
                        case 27:
                            myRenderer.material.mainTexture = forest1Texture;
                            tileType = "Forest";
                            tileTypeText.text = "Forest";
                            break;
                        case 28:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 29:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 32:
                            myRenderer.material.mainTexture = field1Texture;
                            tileType = "Field";
                            tileTypeText.text = "Field";
                            break;
                        case 33:
                            myRenderer.material.mainTexture = forest1Texture;
                            tileType = "Forest";
                            tileTypeText.text = "Forest";
                            break;
                        case 34:
                            myRenderer.material.mainTexture = forest1Texture;
                            tileType = "Forest";
                            tileTypeText.text = "Forest";
                            break;
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if(BattleManager.Instance.gameMode == "deploy")
        {
            if(!IsPointerOverGameObject() && myDeployedUnitId == 0 && BattleManager.Instance.turnOwnerId == possibleArmyDeployment && lastClickedUnit != forwardUnitId && !BattleManager.Instance.isUnitControllerBlocked(lastClickedUnit))
            {
                EventManager.RaiseEventOnUnitDeployed(lastClickedUnit, tileId);
                SoundManagerController.Instance.PlayUnitPlaced();
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
        if(possibleArmyDeployment != 0) tileTypeText.gameObject.SetActive(true);
        if (a == possibleArmyDeployment && myDeployedUnitId == 0 && !initiallyDeploymentNotPossible)
        {
            tc = null;
            if (a == 1) tc = BattleManager.Instance.GetTile(tileId - 2);
            else tc = BattleManager.Instance.GetTile(tileId + 2);
            tileInfluenceDescription.gameObject.SetActive(true);
            //tileTypeText.gameObject.SetActive(true);
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
        }
        else
        {
            tileInfluenceDescription.gameObject.SetActive(false);
            //tileTypeText.gameObject.SetActive(false);
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
            tileTypeText.gameObject.SetActive(false);
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
                topParticleSystem.startColor = Color.blue;
                topParticleSystem.Play();
                tc = BattleManager.Instance.GetTile(tileId - BattleManager.Instance.boardHeight);
                if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)  // looks for left neighbour
                {
                    leftParticleSystem.startColor = Color.blue;
                    leftParticleSystem.Play();
                }
                else tc.DisableHighlight("right");
                tc = BattleManager.Instance.GetTile(tileId + BattleManager.Instance.boardHeight);   // looks for right neighbour
                if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)
                {
                    rightParticleSystem.startColor = Color.blue;
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
                        leftParticleSystem.startColor = Color.blue;
                        leftParticleSystem.Play();
                    }
                    tc = BattleManager.Instance.GetTile(tileId + BattleManager.Instance.boardHeight);   // looks for right neighbour
                    if (!tc.DeploymentPossible(2) || tc.GetForwardUnitId() == uId)
                    {
                        rightParticleSystem.startColor = Color.blue;
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
            if (ownerId == 2)
            {
                pawn.SetActive(true);
                pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            }
            else if(ownerId == 1)
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
            else if (ownerId == 1) pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            else if (ownerId == 2) pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            yield return new WaitForSeconds(0.5f);
            if(ownerId == 0) pawn.SetActive(false);
            else if (ownerId == 1) pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            else if (ownerId == 2) pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void BlinkKeyField(int idArrow, bool isCounterAttack)
    {
        Attack tempAttack;
        List<Attack> tempAttacks;

        tempAttacks = BattleManager.Instance.GetAttacksByArrowId(idArrow);
        tempAttack = null;
        foreach (Attack a in tempAttacks)
        {
            if (a.IsActive()) tempAttack = a;
        }
        if (tempAttack == null) return;
        if(tempAttack.GetKeyFieldId() == keyFieldId && keyFieldId != 0 && ownerId != tempAttack.GetArmyId())
        {
            isBlinking = true;
            if (tempAttack.GetArmyId() == 1) pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
            else pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
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
            pawn.GetComponent<MeshRenderer>().material = imperialMaterial;
        }
        else if (ownerId == 2)
        {
            pawn.SetActive(true);
            pawn.GetComponent<MeshRenderer>().material = frenchMaterial;
        }
    }

    private void HighlightField(int aId)
    {
        if(aId == possibleArmyDeployment)
        {
            topParticleSystem.Play();
            bottomParticleSystem.Play();
            if(BattleManager.Instance.boardHeight + 7 == tileId && aId == 2 || BattleManager.Instance.boardHeight + 9 == tileId && aId == 1) leftParticleSystem.Play();
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
            if(aId == 3) tileTypeText.gameObject.SetActive(false);
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
        return 0;
    }

    public bool DeploymentPossible(int aId)
    {
        if (aId == possibleArmyDeployment) return true;
        else return false;
    }

    public int GetUnitValue(string uType)
    {
        int score;
        TileController tc;

        score = 50;
        if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 2 || (tileId - 1) % BattleManager.Instance.boardHeight == 1)    // if tile in first line 
        {
            if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.easy)
            {
                if (uType == "Coustilliers" || uType == "Stradioti") score += 10;
                if (tileType == "Hill") score += 10;    // better because protected from Heavy cavalry
                if (tileType == "Town") score -= 10;    // worse because easy target for artillery
                if (tileType == "Forest" && uType == "Arquebusiers") score -= 10;
            }
            else if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.medium || GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard)
            {
                if (uType == "Coustilliers" || uType == "Stradioti") score += 10;
                if (tileType == "Hill") score -= 10;    // better because protected from Heavy cavalry
                if (tileType == "Town") score += 10;    // worse because easy target for artillery
                if (tileType == "Forest" && uType == "Arquebusiers") score -= 30;
                if (tileType == "Town" && (uType == "Coustilliers" || uType == "Stradioti" || uType == "Imperial Cavalery" || uType == "Gendarmes")) score -= 20;
                if (tileType == "Hill" && (uType == "Landsknechte" || uType == "Suisse")) score += 10;
                if (uType == "Coustilliers" || uType == "Stradioti")
                {
                    if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 2)    // check if tile in front and next to it is key field
                    {
                        tc = BattleManager.Instance.GetTile(tileId - 1 + BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                        tc = BattleManager.Instance.GetTile(tileId - 1 - BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                    }
                    if ((tileId - 1) % BattleManager.Instance.boardHeight == 1)
                    {
                        tc = BattleManager.Instance.GetTile(tileId + 1 + BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                        tc = BattleManager.Instance.GetTile(tileId + 1 - BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                    }
                }
                if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard) // Heavy cavalry shoudn't be placed on both end of deployment zone
                {
                    if (uType == "Imperial Cavalery" || uType == "Gendarmes")
                    {
                        if (tileId < 2 * BattleManager.Instance.boardHeight || tileId > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - 2 * BattleManager.Instance.boardHeight) score += 10;
                    }
                }
            }
        }
        else if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1 || (tileId - 1) % BattleManager.Instance.boardHeight == 0) // if tile in second line 
        {
            if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 1) tc = BattleManager.Instance.GetTile(tileId - 1);
            else tc = BattleManager.Instance.GetTile(tileId + 1);
            if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.easy)
            {
                if (uType == "Coustilliers" || uType == "Stradioti") score -= 10;
                if (tc.tileType == "Hill") score += 10;    // better because protected from Heavy cavalry
                if (tc.tileType == "Town") score -= 10;    // worse because easy target for artillery
                if (tc.tileType == "Forest" && uType == "Arquebusiers") score -= 10;    // better because arquebusiers has bonus to defense in woods
            }
            else if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.medium || GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard)
            {
                if (uType == "Coustilliers" || uType == "Stradioti") score -= 10;
                if (tc.tileType == "Hill") score -= 10;    // better because protected from Heavy cavalry
                if (tc.tileType == "Town") score += 10;    // worse because easy target for artillery
                if (tc.tileType == "Forest" && uType == "Arquebusiers") score -= 30;    // better because arquebusiers has bonus to defense in woods
                if (tc.tileType == "Town" && (uType == "Coustilliers" || uType == "Stradioti" || uType == "Imperial Cavalery" || uType == "Gendarmes")) score -= 20;
                if (tc.tileType == "Hill" && (uType == "Landsknechte" || uType == "Suisse")) score += 10;
                if (uType == "Coustilliers" || uType == "Stradioti")
                {
                    if ((tileId - 1) % BattleManager.Instance.boardHeight == BattleManager.Instance.boardHeight - 2)     // check if tile in front and next to it is key field
                    {
                        tc = BattleManager.Instance.GetTile(tileId - 2 + BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                        tc = BattleManager.Instance.GetTile(tileId - 2 - BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                    }
                    if ((tileId - 1) % BattleManager.Instance.boardHeight == 1)
                    {
                        tc = BattleManager.Instance.GetTile(tileId + 2 + BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                        tc = BattleManager.Instance.GetTile(tileId + 2 - BattleManager.Instance.boardHeight);
                        if (tc.keyFieldId != 0) score -= 10;
                    }
                }
                if(GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard)  // Heavy cavalry shoudn't be placed on both end of deployment zone
                {
                    if (uType == "Imperial Cavalery" || uType == "Gendarmes")
                    {
                        if (tileId < 2 * BattleManager.Instance.boardHeight || tileId > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - 2 * BattleManager.Instance.boardHeight) score += 10;
                    }
                }
            }
        }
        return score;
    }

    public int GetOpposingUnitValue(string uType)
    {
        int score = 0;
        if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.medium || GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard)
        {
            if (uType == "Imperial Cavalery" || uType == "Gendarmes")
            {
                if (tileType == "Hill") score += 20;
            }
            else if (uType == "Artillery")
            {
                if (tileType == "Town") score -=30;
            }
            if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard)
            {
                if (tileType == "Town")
                {
                    if (uType == "Artillery") score += 20;
                    if (uType == "Landsknechte" || uType == "Suisse") score -= 10;
                }
                if (uType == "Landsknechte" || uType == "Suisse")
                {
                    if (tileId < 2 * BattleManager.Instance.boardHeight || tileId > BattleManager.Instance.boardHeight * BattleManager.Instance.boardWidth - 2 * BattleManager.Instance.boardHeight) score += 10;
                }
            }
        }
        return score;
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

    public void ResetTile()
    {
        pawn.SetActive(false);
        isBlinking = false;
        ownerId = 0;
        lastClickedUnit = 0;
        forwardUnitId = 0;
    }

    public void ResetDeployedUnit()
    {
        myDeployedUnitId = 0;
    }

    public bool IsTileOccupied()
    {
        if (myDeployedUnitId == 0) return false;
        else return true;
    }
}
