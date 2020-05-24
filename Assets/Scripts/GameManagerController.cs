using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManagerController : MonoBehaviour
{
    public enum viewTypeEnum {perspective, isometric };
    public enum diffLevelEnum {easy, medium, hard};
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
        SceneManager.LoadScene(level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
