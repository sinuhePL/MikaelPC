using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManagerController : MonoBehaviour
{
    [SerializeField] private AudioClip throwDiceSound;

    private AudioSource myAudioSource;

    private void PlayThrowSound(int a)
    {
        myAudioSource.PlayOneShot(throwDiceSound);
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        EventManager.onAttackOrdered += PlayThrowSound;
    }

    private void OnDestroy()
    {
        EventManager.onAttackOrdered -= PlayThrowSound;
    }
}
