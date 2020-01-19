using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class MenuElementController : MonoBehaviour
{
    private Image myImage;
    private bool isEnabled;
    [SerializeField] private Sprite enabledPicture;
    [SerializeField] private Sprite disabledPicture;

    // Start is called before the first frame update
    void Start()
    {
        isEnabled = true;
        myImage = GetComponent<Image>();
    }

    public void ButtonPressed()
    {
        if (isEnabled)
        {
            myImage.sprite = disabledPicture;
            isEnabled = false;
            onElementDisabled();
        }
        else
        {
            myImage.sprite = enabledPicture;
            isEnabled = true;
            onElementEnabled();
        }
    }

    protected abstract void onElementEnabled();
    protected abstract void onElementDisabled();
}
