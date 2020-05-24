using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButtonController : MenuElementController
{
    protected override void onElementEnabled()
    {
        GameManagerController.Instance.isMusicEnabled = true;
    }

    protected override void onElementDisabled()
    {
        GameManagerController.Instance.isMusicEnabled = false;
    }
}
