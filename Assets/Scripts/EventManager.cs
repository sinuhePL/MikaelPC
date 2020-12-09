using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Action();
    public delegate void Action<A>(A arg1);
    public delegate void Action<A, B>(A arg1, B arg2);
    public delegate void Action<A, B, C>(A arg1, B arg2, C arg3);
    public delegate void Action<A, B, C, D, E>(A arg1, B arg2, C arg3, D arg4, E arg5);

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

    public static event Action<int> onMouseEnter;
    public static void RaiseEventOnMouseEnter(int idUnit)
    {
        if (onMouseEnter != null) onMouseEnter(idUnit);
    }

    public static event Action<int> onMouseExit;
    public static void RaiseEventOnMouseExit(int idUnit)
    {
        if (onMouseExit != null) onMouseExit(idUnit);
    }

    public static event Action<int, bool> onAttackClicked;
    public static void RaiseEventOnAttackClicked(int idArrow, bool isCounterAttack)
    {
        if (onAttackClicked != null) onAttackClicked(idArrow, isCounterAttack);
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

    public static event Action<string> onResultMenuClosed;
    public static void RaiseEventResultMenuClosed(string mode) // attack, routtest
    {
        if (onResultMenuClosed != null) onResultMenuClosed(mode);
    }

    /*public static event Action onAttackResultClosed;
    public static void RaiseEventAttackResultClosed()
    {
        if (onAttackResultClosed != null) onAttackResultClosed();
    }*/

    public static event Action<int> onUnitDestroyed;
    public static void RaiseEventOnUnitDestroyed(int unitId)
    {
        if (onUnitDestroyed != null) onUnitDestroyed(unitId);
    }

    public static event Action onTurnStart;
    public static void RaiseEventOnTurnStart()
    {
        if (onTurnStart != null) onTurnStart();
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

    public static event Action<string, int,int> onRouteTestOver;
    public static void RaiseEventRouteTestOver(string description, int result, int morale)
    {
        if (onRouteTestOver != null) onRouteTestOver(description, result, morale);
    }

    public static event Action<int, int, int, string, string> onUIDeployPressed;
    public static void RaiseEventOnUIDeployPressed(int armyId, int position, int uId, string uType, string commander)
    {
        if (onUIDeployPressed != null) onUIDeployPressed(armyId, position, uId, uType, commander);
    }

    public static event Action<int, int> onUnitDeployed;
    public static void RaiseEventOnUnitDeployed(int uId, int tId)
    {
        if (onUnitDeployed != null) onUnitDeployed(uId, tId);
    }

    public static event Action<int> onDeploymentStart;
    public static void RaiseEventOnDeploymentStart(int aId)
    {
        if (onDeploymentStart != null) onDeploymentStart(aId);
    }
}
