using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManagerController : MonoBehaviour
{
    private static SoundManagerController _instance;
    private AudioSource myAudioSource;
    [SerializeField] private AudioClip throwDiceSound;


    public static SoundManagerController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void PlayThrowSound(int a)
    {
        if(BattleManager.isSoundEnabled) myAudioSource.PlayOneShot(throwDiceSound, BattleManager.soundLevel);
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
