using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicButtonController : MenuElementController
{
    protected override void onElementEnabled()
    {
        BattleManager.isMusicEnabled = true;
    }

    protected override void onElementDisabled()
    {
        BattleManager.isMusicEnabled = false;
    }
}
