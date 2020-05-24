using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonController : MenuElementController
{
    protected override void onElementEnabled()
    {
        GameManagerController.Instance.isSoundEnabled = true;
    }

    protected override void onElementDisabled()
    {
        GameManagerController.Instance.isSoundEnabled = false;
    }
}
