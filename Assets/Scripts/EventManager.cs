using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Action();
    public delegate void Action<A>(A arg1);

    /* Example use:
    public static event Action newEvent;
    public static void RaiseEventnewEvent()
    {
      if(newEvent != null) newEvent();
    }
    
    public static event Action<int, bool> newEvent2;
    public static void RaiseEventnewEvent2(int howMany, bool isFinal)
    {
       if(newEvent2 != null) newEvent2(howMany, isFinal);
    }
    */
    public static event Action<int> onUnitClicked;
    public static void RaiseEventOnUnitClicked(int idUnit)
    {
        if (onUnitClicked != null) onUnitClicked(idUnit);
    }
}
