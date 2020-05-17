using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonController : MenuElementController
{
    protected override void onElementEnabled()
    {
        GameManagerController.isSoundEnabled = true;
    }

    protected override void onElementDisabled()
    {
        GameManagerController.isSoundEnabled = false;
    }
}
