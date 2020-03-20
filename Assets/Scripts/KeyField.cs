using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyField
{
    private int occupantId;
    private int keyFieldId;
	
    public KeyField(int fieldId, int ownerId)    // konstruktor
    {
        occupantId = ownerId;
        keyFieldId = fieldId;
    }

    public KeyField(KeyField pattern)
    {
        occupantId = pattern.occupantId;
        keyFieldId = pattern.keyFieldId;
    }

    public void SetOccupant(int id)
    {
        occupantId = id;
    }

    public int GetOccupant()
    {
        return occupantId;
    }

    public int GetFieldId()
    {
        return keyFieldId;
    }
}
