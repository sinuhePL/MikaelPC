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

    public static event Action<StateChange> onDiceResult;
    public static void RaiseEventOnDiceResult(StateChange result)
    {
        if (onDiceResult != null) onDiceResult(result);
    }

    public static event Action onResultMenuClosed;
    public static void RaiseEventResultMenuClosed()
    {
        if (onResultMenuClosed != null) onResultMenuClosed();
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

    public static event Action onTurnEnd;
    public static void RaiseEventOnTurnEnd()
    {
        if (onTurnEnd != null) onTurnEnd();
    }

    public static event Action onGameStart;
    public static void RaiseEventGameStart()
    {
        if (onGameStart != null) onGameStart();
    }

    public static event Action<int> onGameOver;
    public static void RaiseEventGameOver(int winnerId)
    {
        if (onGameOver != null) onGameOver(winnerId);
    }
}
