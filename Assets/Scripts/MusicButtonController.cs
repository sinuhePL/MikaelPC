using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButtonController : MenuElementController
{
    public override void Start()
    {
        base.Start();
        if (GameManagerController.Instance.isMusicEnabled)
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
        GameManagerController.Instance.isMusicEnabled = true;
        SoundManagerController.Instance.ResumeMusic();
    }

    protected override void onElementDisabled()
    {
        GameManagerController.Instance.isMusicEnabled = false;
        SoundManagerController.Instance.StopMusic();
    }
}
