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

    public void ToggleChanged()
    {
        if(myToggle != null && myToggle.isOn)
        {
            if (enumType == toggleEnum.easy) BattleManager.minimaxLimit = 10.0f;
            else if (enumType == toggleEnum.medium) BattleManager.minimaxLimit = 20.0f;
            else if (enumType == toggleEnum.hard) BattleManager.minimaxLimit = 30.0f;
            else if (enumType == toggleEnum.perspective)
            {
                GameManagerController.viewType = "perspective";
                if (myCamera != null)
                {
                    myCamera.orthographic = false;
                    myCamera.transform.localRotation = Quaternion.Euler(new Vector3(45.0f, 30.0f, 0.0f));
                    myCamera.transform.position = new Vector3(8.0f, 6.0f, -25.0f);
                }
            }
            else if (enumType == toggleEnum.isometric)
            {
                GameManagerController.viewType = "isometric";
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
