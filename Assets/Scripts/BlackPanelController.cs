using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackPanelController : MonoBehaviour
{
    public float transitionTime = 1.0f;
    private Image myImage;
    
    private IEnumerator MakeTransparent()
    {
        for(float myA = 1.0f; myA > 0; myA -= 0.01f)
        {
            Color c = myImage.material.color;
            c.a = myA;
            myImage.material.color = c;
            yield return new WaitForSeconds(.01f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        StartCoroutine(MakeTransparent());
    }


}
