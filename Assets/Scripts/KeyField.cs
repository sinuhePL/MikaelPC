using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyField
{
    private int occupantId;
    private int boardFieldId;
	
    public KeyField(int fieldId)    // konstruktor
    {
        occupantId = 0;
        boardFieldId = fieldId;
    }

    public KeyField(KeyField pattern)
    {
        occupantId = pattern.occupantId;
        boardFieldId = pattern.boardFieldId;
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
        return boardFieldId;
    }
}
