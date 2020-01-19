using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundButtonController : MenuElementController
{
    protected override void onElementEnabled()
    {
        BattleManager.isSoundEnabled = true;
    }

    protected override void onElementDisabled()
    {
        BattleManager.isSoundEnabled = false;
    }
}
