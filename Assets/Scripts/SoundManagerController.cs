using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManagerController : MonoBehaviour
{
    private static SoundManagerController _instance;
    private AudioSource musicAudioSource;
    private AudioSource musicAudioSource2;
    private AudioSource sfxAudioSource;
    private bool firstMusicSourceIsPlaying;
    [SerializeField] private AudioClip throwDiceSound;
    [SerializeField] private AudioClip turnStartSound;
    [SerializeField] private AudioClip arquebusiersAttackSound;
    [SerializeField] private AudioClip pikemanAttackSound;
    [SerializeField] private AudioClip artilleryAttackSound;
    [SerializeField] private AudioClip cavalryAttackSound;
    [SerializeField] private AudioClip lightCavalryAttackSound;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip lostSound;
    [SerializeField] private AudioClip stalemateSound;
    [SerializeField] private AudioClip wonSound;
    [SerializeField] private AudioClip[] unitReportSound;
    [SerializeField] private AudioClip[] unitPlaceSound;
    [SerializeField] private AudioClip[] gameMusic;
    [SerializeField] private AudioClip winGameSound;
    [SerializeField] private AudioClip looseGameSound;

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
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource2 = gameObject.AddComponent<AudioSource>();
        sfxAudioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayThrowSound(int a)
    {
        if(GameManagerController.Instance.isSoundEnabled) sfxAudioSource.PlayOneShot(throwDiceSound, GameManagerController.Instance.soundLevel);
    }

    private void PlayAttackSound(int a)
    {
        Attack at;
        Unit un;

        at = BattleManager.Instance.GetAttackById(a);
        if (GameManagerController.Instance.isSoundEnabled && at.GetName() != "Move")
        {
            un = at.GetOwner();
            if(un.GetUnitType() == "Landsknechte" || un.GetUnitType() == "Suisse") sfxAudioSource.PlayOneShot(pikemanAttackSound, GameManagerController.Instance.soundLevel*0.5f);
            if (un.GetUnitType() == "Arquebusiers") sfxAudioSource.PlayOneShot(arquebusiersAttackSound, GameManagerController.Instance.soundLevel*0.5f);
            if (un.GetUnitType() == "Artillery") sfxAudioSource.PlayOneShot(artilleryAttackSound, GameManagerController.Instance.soundLevel*0.5f);
            if (un.GetUnitType() == "Gendarmes" || un.GetUnitType() == "Imperial Cavalery") sfxAudioSource.PlayOneShot(cavalryAttackSound, GameManagerController.Instance.soundLevel * 0.3f);
            if (un.GetUnitType() == "Stradioti" || un.GetUnitType() == "Coustilliers") sfxAudioSource.PlayOneShot(lightCavalryAttackSound, GameManagerController.Instance.soundLevel * 0.5f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.onAttackOrdered += PlayAttackSound;
        musicAudioSource.loop = true;
        musicAudioSource2.loop = true;
        musicAudioSource.volume = GameManagerController.Instance.musicLevel;
        musicAudioSource2.volume = GameManagerController.Instance.musicLevel;
        sfxAudioSource.volume = GameManagerController.Instance.soundLevel;
        firstMusicSourceIsPlaying = false;
        PlayMusic(0, true);
    }

    private void OnDestroy()
    {
        EventManager.onAttackOrdered -= PlayAttackSound;
    }

    public void PlayStartTurn()
    {
        if (GameManagerController.Instance.isSoundEnabled) sfxAudioSource.PlayOneShot(turnStartSound, GameManagerController.Instance.soundLevel*0.4f);
    }

    public void PlayClick()
    {
        if (GameManagerController.Instance.isSoundEnabled) sfxAudioSource.PlayOneShot(clickSound, GameManagerController.Instance.soundLevel*0.6f);
    }

    public void PlayResult(string resultType)
    {
        if (GameManagerController.Instance.isSoundEnabled)
        {
            if(resultType == "won") sfxAudioSource.PlayOneShot(wonSound, GameManagerController.Instance.soundLevel);
            if (resultType == "stalemate") sfxAudioSource.PlayOneShot(stalemateSound, GameManagerController.Instance.soundLevel);
            if (resultType == "lost") sfxAudioSource.PlayOneShot(lostSound, GameManagerController.Instance.soundLevel);
        }
    }

    public void PlayUnitReport()
    {
        int ranNum;
        if (GameManagerController.Instance.isSoundEnabled)
        {
            ranNum = Random.Range(0, unitReportSound.Length);
            sfxAudioSource.PlayOneShot(unitReportSound[ranNum], GameManagerController.Instance.soundLevel);
        }
    }

    public void PlayUnitPlaced()
    {
        int ranNum;
        if (GameManagerController.Instance.isSoundEnabled)
        {
            ranNum = Random.Range(0, unitPlaceSound.Length);
            sfxAudioSource.PlayOneShot(unitPlaceSound[ranNum], GameManagerController.Instance.soundLevel);
        }
    }

    public void PlayMusic(int musicClip, bool isLoop)
    {
        if (GameManagerController.Instance.isMusicEnabled)
        {
            AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
            activeSource.clip = gameMusic[musicClip];
            activeSource.volume = GameManagerController.Instance.musicLevel;
            if (isLoop) activeSource.loop = true;
            else activeSource.loop = false;
            activeSource.Play();
        }
    }

    public void PlayMusicWithFade(int newClip, float transitionTime = 1.0f)
    {
        if (GameManagerController.Instance.isMusicEnabled)
        {
            AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
            activeSource.loop = false;
            StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
        }
    }

    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, int newClip, float transitionTime)
    {
        if (!activeSource.isPlaying) activeSource.Play();
        float t = 0.0f;
        for(t=0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = GameManagerController.Instance.musicLevel*(1 - (t / transitionTime));
            yield return null;
        }
        activeSource.Stop();
        activeSource.clip = gameMusic[newClip];
        activeSource.Play();
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = GameManagerController.Instance.musicLevel*(t / transitionTime);
            yield return null;
        }
    }

    public void PlayMusicWithCrossFade(int newClip, float transitionTime = 1.0f)
    {
        if (GameManagerController.Instance.isMusicEnabled)
        {
            AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
            AudioSource newSource = (firstMusicSourceIsPlaying) ? musicAudioSource2 : musicAudioSource;
            activeSource.loop = false;
            firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;
            if(newClip == 0) newSource.clip = gameMusic[newClip];
            else
            {
                int song = Random.Range(1, gameMusic.Length);
                newSource.clip = gameMusic[song];
            }
            newSource.Play();
            StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
            Invoke("PlayNextClip", newSource.clip.length);
        }
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            original.volume = GameManagerController.Instance.musicLevel * (1 - (t / transitionTime));
            newSource.volume = GameManagerController.Instance.musicLevel * (t / transitionTime);
            yield return null;
        }
        original.Stop();
    }

    private void PlayNextClip()
    {
        if (GameManagerController.Instance.isMusicEnabled)
        {
            int song = Random.Range(1, gameMusic.Length);
            if (SceneManager.GetActiveScene().name == "MenuScene") song = 0;
            PlayMusic(song, false);
            AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
            Invoke("PlayNextClip", activeSource.clip.length);
        }
    }

    public void StopMusic()
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
        activeSource.Stop();
    }

    public void ResumeMusic()
    {
        PlayNextClip();
    }

    public void ChangeMusicVolume()
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
        activeSource.volume = GameManagerController.Instance.musicLevel;
    }

    public void PlayEndGame(bool isWinner)
    {
        if (GameManagerController.Instance.isSoundEnabled)
        {
            if(isWinner) sfxAudioSource.PlayOneShot(winGameSound, GameManagerController.Instance.soundLevel);
            else sfxAudioSource.PlayOneShot(looseGameSound, GameManagerController.Instance.soundLevel);
        }
    }

    public void SilenceMusic(float transitionTime = 0.25f)
    {
        if (GameManagerController.Instance.isMusicEnabled)
        {
            AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
            StartCoroutine(SilenceMusicWithFade(activeSource, transitionTime));
        }
    }

    private IEnumerator SilenceMusicWithFade(AudioSource activeSource, float transitionTime)
    {
        if (!activeSource.isPlaying) activeSource.Play();
        float t = 0.0f;
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = GameManagerController.Instance.musicLevel * (1 - (t / transitionTime));
            yield return null;
        }
    }

    public void LouderMusic(float transitionTime = 0.25f)
    {
        if (GameManagerController.Instance.isMusicEnabled)
        {
            AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicAudioSource : musicAudioSource2;
            StartCoroutine(ResumeMusicWithFade(activeSource, transitionTime));
        }
    }

    private IEnumerator ResumeMusicWithFade(AudioSource activeSource, float transitionTime)
    {
        if (!activeSource.isPlaying) activeSource.Play();
        float t = 0.0f;
        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = GameManagerController.Instance.musicLevel * (t / transitionTime);
            yield return null;
        }
    }
}
