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
        if (controlObject == controlType.sound) mySlider.value = GameManagerController.Instance.soundLevel;
        else if (controlObject == controlType.music) mySlider.value = GameManagerController.Instance.musicLevel;
    }

    public void ValueChanged()
    {
        if (controlObject == controlType.sound) GameManagerController.Instance.soundLevel = mySlider.value;
        else if (controlObject == controlType.music)
        {
            GameManagerController.Instance.musicLevel = mySlider.value;
            SoundManagerController.Instance.ChangeMusicVolume();
        }
    }
}
