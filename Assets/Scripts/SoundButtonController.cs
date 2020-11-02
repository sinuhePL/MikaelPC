using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonController : MenuElementController
{

    public override void Start()
    {
        base.Start();
        if (GameManagerController.Instance.isSoundEnabled)
        {
            isEnabled = true;
            myImage.sprite = enabledPicture;
        }
        else
        {
            isEnabled = false;
            myImage.sprite = disabledPicture;
        }
    }

    protected override void onElementEnabled()
    {
        GameManagerController.Instance.isSoundEnabled = true;
    }

    protected override void onElementDisabled()
    {
        GameManagerController.Instance.isSoundEnabled = false;
    }
}
