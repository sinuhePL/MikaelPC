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
        transform.DOScale(0.305f, 120.0f);   
    }
}
