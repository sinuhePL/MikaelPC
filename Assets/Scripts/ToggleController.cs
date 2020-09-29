using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    private Toggle myToggle = null;
    private Camera myCamera;

    public enum toggleEnum { easy, medium, hard, perspective, isometric};
    [SerializeField] private toggleEnum enumType;

    // Start is called before the first frame update
    void Awake()
    {
        myToggle = GetComponent<Toggle>();
        myCamera = Camera.main;
    }

    private void Start()
    {
        if (enumType == toggleEnum.easy)
        {
            if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.easy) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (enumType == toggleEnum.medium)
        {
            if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.medium) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (enumType == toggleEnum.hard)
        {
            if (GameManagerController.Instance.difficultyLevel == GameManagerController.diffLevelEnum.hard) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (enumType == toggleEnum.perspective)
        {
            if (GameManagerController.Instance.viewType == GameManagerController.viewTypeEnum.perspective) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
        else if (enumType == toggleEnum.isometric)
        {
            if (GameManagerController.Instance.viewType == GameManagerController.viewTypeEnum.isometric) myToggle.isOn = true;
            else myToggle.isOn = false;
        }
    }

    public void ToggleChanged()
    {
        SoundManagerController.Instance.PlayClick();
        if (myToggle != null && myToggle.isOn)
        {
            if (enumType == toggleEnum.easy) GameManagerController.Instance.difficultyLevel = GameManagerController.diffLevelEnum.easy;
            else if (enumType == toggleEnum.medium) GameManagerController.Instance.difficultyLevel = GameManagerController.diffLevelEnum.medium;
            else if (enumType == toggleEnum.hard) GameManagerController.Instance.difficultyLevel = GameManagerController.diffLevelEnum.hard;
            else if (enumType == toggleEnum.perspective)
            {
                GameManagerController.Instance.viewType = GameManagerController.viewTypeEnum.perspective;
                if (myCamera != null)
                {
                    myCamera.orthographic = false;
                    myCamera.transform.localRotation = Quaternion.Euler(new Vector3(45.0f, 30.0f, 0.0f));
                    myCamera.transform.position = new Vector3(8.0f, 6.0f, -25.0f);
                }
            }
            else if (enumType == toggleEnum.isometric)
            {
                GameManagerController.Instance.viewType = GameManagerController.viewTypeEnum.isometric;
                if (myCamera != null)
                {
                    myCamera.orthographic = true;
                    myCamera.transform.localRotation = Quaternion.Euler(new Vector3(30.0f, 45.0f, 0.0f));
                    myCamera.transform.position = new Vector3(8.0f, 12.0f, -25.0f);
                }
            }
        }
    }
}
