using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyField
{
    private int occupantId;
    private int keyFieldId;
    private string keyFieldName;
	
    public KeyField(int fieldId, int ownerId, string name)    // konstruktor
    {
        occupantId = ownerId;
        keyFieldId = fieldId;
        keyFieldName = name;
    }

    public KeyField(KeyField pattern)
    {
        occupantId = pattern.occupantId;
        keyFieldId = pattern.keyFieldId;
        keyFieldName = pattern.keyFieldName;
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

    public string GetFieldName()
    {
        return keyFieldName;
    }
}
