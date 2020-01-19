using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    private Slider mySlider;

    private enum controlType {sound, music};
    [SerializeField] private controlType controlObject;

    private void Start()
    {
        mySlider = GetComponent<Slider>();
    }

    public void ValueChanged()
    {
        if (controlObject == controlType.sound) BattleManager.soundLevel = mySlider.value;
        else if (controlObject == controlType.music) BattleManager.musicLevel = mySlider.value;
    }
}
