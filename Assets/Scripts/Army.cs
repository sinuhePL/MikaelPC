using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Army
{
    private string armyName;
    private int idArmy;
    private int morale;
    private int routeTestModifier;
    private int testDiceSize;

    public Army(Army pattern)   // konstruktor kopiujący
    {
        idArmy = pattern.idArmy;
        morale = pattern.morale;
        routeTestModifier = pattern.routeTestModifier;
        testDiceSize = pattern.testDiceSize;
        armyName = pattern.armyName;
    }

    public Army(int i, int m, int rtm, int ts, string n)  // kontruktor
    {
        idArmy = i;
        morale = m;
        routeTestModifier = rtm;
        testDiceSize = ts;
        armyName = n;
    }

    public int GetMorale()
    {
        return morale;
    }

    public void ChangeMorale(int mc)
    {
        morale += mc;
    }

    public int GetRouteTestModifier()
    {
        return routeTestModifier;
    }

    public void ChangeRouteTestModifier(int rtm)
    {
        routeTestModifier += rtm;
    }

    public bool MoraleTestSuccessful()  // testuje morale całej armii
    {
        int i;

        i = Random.Range(0, testDiceSize);
        i += routeTestModifier;
        if (i > morale) return false;    // jeśli zmodyfikowany wynik testu jest większy niż morale armii, armia panikuje i armia przegrywa bitwę
        else return true;
    }

    public int GetArmyId()
    {
        return idArmy;
    }

    public string GetArmyName()
    {
        return armyName;
    }
}
