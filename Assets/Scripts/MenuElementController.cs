using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class MenuElementController : MonoBehaviour
{
    protected Image myImage;
    protected bool isEnabled;
    [SerializeField] protected Sprite enabledPicture;
    [SerializeField] protected Sprite disabledPicture;

    // Start is called before the first frame update
    public virtual void Start()
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
