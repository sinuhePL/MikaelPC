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

    public static event Action<int> onAttackClicked;
    public static void RaiseEventOnAttackClicked(int idAttack)
    {
        if (onAttackClicked != null) onAttackClicked(idAttack);
    }

    public static event Action<int> onTileClicked;
    public static void RaiseEventOnTileClicked(int idTile)
    {
        if (onTileClicked != null) onTileClicked(idTile);
    }

    public static event Action<int> onAttackOrdered;
    public static void RaiseEventOnAttackOrdered(int idAttack)
    {
        if (onAttackOrdered != null) onAttackOrdered(idAttack);
    }

    public static event Action<ThrowResult> onDiceResult;
    public static void RaiseEventOnDiceResult(ThrowResult result)
    {
        if (onDiceResult != null) onDiceResult(result);
    }

    public static event Action onUpdateBoard;
    public static void RaiseEventUpdateBoard()
    {
        if (onUpdateBoard != null) onUpdateBoard();
    }

    public static event Action onAttackResultClosed;
    public static void RaiseEventAttackResultClosed()
    {
        if (onAttackResultClosed != null) onAttackResultClosed();
    }

    public static event Action<int> onUnitDestroyed;
    public static void RaiseEventOnUnitDestroyed(int unitId)
    {
        if (onUnitDestroyed != null) onUnitDestroyed(unitId);
    }
}
