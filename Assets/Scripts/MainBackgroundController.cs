using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MainBackgroundController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(Screen.width/6100.0f, 120.0f);   
    }
}
