using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManagerController : MonoBehaviour
{
    private static SoundManagerController _instance;
    private AudioSource myAudioSource;
    [SerializeField] private AudioClip throwDiceSound;
    [SerializeField] private AudioClip turnStartSound;
    [SerializeField] private AudioClip arquebusiersAttackSound;
    [SerializeField] private AudioClip pikemanAttackSound;
    [SerializeField] private AudioClip artilleryAttackSound;
    [SerializeField] private AudioClip cavalryAttackSound;
    [SerializeField] private AudioClip lightCavalryAttackSound;

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
        DontDestroyOnLoad(gameObject);
    }

    public void PlayThrowSound(int a)
    {
        if(GameManagerController.Instance.isSoundEnabled) myAudioSource.PlayOneShot(throwDiceSound, GameManagerController.Instance.soundLevel);
    }

    private void PlayAttackSound(int a)
    {
        Attack at;
        Unit un;
        if (GameManagerController.Instance.isSoundEnabled)
        {
            at = BattleManager.Instance.GetAttackById(a);
            un = at.GetOwner();
            if(un.GetUnitType() == "Landsknechte" || un.GetUnitType() == "Suisse") myAudioSource.PlayOneShot(pikemanAttackSound, GameManagerController.Instance.soundLevel*0.5f);
            if (un.GetUnitType() == "Arquebusiers") myAudioSource.PlayOneShot(arquebusiersAttackSound, GameManagerController.Instance.soundLevel*0.5f);
            if (un.GetUnitType() == "Artillery") myAudioSource.PlayOneShot(artilleryAttackSound, GameManagerController.Instance.soundLevel*0.5f);
            if (un.GetUnitType() == "Gendarmes" || un.GetUnitType() == "Imperial Cavalery") myAudioSource.PlayOneShot(cavalryAttackSound, GameManagerController.Instance.soundLevel * 0.3f);
            if (un.GetUnitType() == "Stradioti" || un.GetUnitType() == "Coustilliers") myAudioSource.PlayOneShot(lightCavalryAttackSound, GameManagerController.Instance.soundLevel * 0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        EventManager.onAttackOrdered += PlayAttackSound;
    }

    private void OnDestroy()
    {
        EventManager.onAttackOrdered -= PlayAttackSound;
    }

    public void PlayStartTurn()
    {
        if (GameManagerController.Instance.isSoundEnabled) myAudioSource.PlayOneShot(turnStartSound, GameManagerController.Instance.soundLevel-0.4f);
    }


}
