using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManagerController : MonoBehaviour
{
    public enum viewTypeEnum {perspective, isometric };
    public enum diffLevelEnum {easy, medium, hard};
    public enum terrainTypeEnum {historical, random};
    private static GameManagerController _instance;
    public bool isSoundEnabled = true;
    public bool isMusicEnabled = true;
    [Range(0.0f, 1.0f)]
    public float soundLevel = 0.9f;
    [Range(0.0f, 1.0f)]
    public float musicLevel = 0.7f;
    public bool isPlayer1Human = true;
    public bool isPlayer2Human = true;
    public viewTypeEnum viewType = viewTypeEnum.perspective;
    public diffLevelEnum difficultyLevel = diffLevelEnum.medium;
    public terrainTypeEnum terrainType = terrainTypeEnum.random;

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
        DOTween.Init();
    }

    public static GameManagerController Instance { get { return _instance; } }

    public void LoadLevel(string level)
    {
        /*if (level == "BattleScene") Screen.orientation = ScreenOrientation.LandscapeRight;
        else Screen.orientation = ScreenOrientation.Portrait;*/
        if (level == "BattleScene" || level == "Tutorial") SoundManagerController.Instance.PlayMusicWithCrossFade(1);
        else
        {
            SoundManagerController.Instance.PlayMusicWithCrossFade(0);
            isPlayer1Human = true;
            isPlayer2Human = true;
            difficultyLevel = diffLevelEnum.medium;
            terrainType = terrainTypeEnum.random;
        }
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
