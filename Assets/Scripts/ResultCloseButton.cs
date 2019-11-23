using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCloseButton : MonoBehaviour
{
    public void ButtonPressed()
    {
        EventManager.RaiseEventResultMenuClosed();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
}
