using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainToggleController : MonoBehaviour
{
    private Toggle myToggle = null;

    public enum terrainToggleEnum { historical, random };
    [SerializeField] private terrainToggleEnum terrainType;

    void Awake()
    {
        myToggle = GetComponent<Toggle>();
    }

    private void Start()
    {
        if (terrainType == terrainToggleEnum.historical)
        {
            if (GameManagerController.Instance.terrainType == GameManagerController.terrainTypeEnum.historical) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (terrainType == terrainToggleEnum.random)
        {
            if (GameManagerController.Instance.terrainType == GameManagerController.terrainTypeEnum.random) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
    }

    public void ToggleClicked()
    {
        if (myToggle != null && myToggle.isOn)
        {
            if (terrainType == terrainToggleEnum.historical)
            {
                GameManagerController.Instance.terrainType = GameManagerController.terrainTypeEnum.historical;
            }
            else if (terrainType == terrainToggleEnum.random)
            {
                GameManagerController.Instance.terrainType = GameManagerController.terrainTypeEnum.random;
            }
        }
    }
}
