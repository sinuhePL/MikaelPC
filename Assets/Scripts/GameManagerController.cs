using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManagerController : MonoBehaviour
{
    private static GameManagerController _instance;
    public static bool isSoundEnabled = true;
    public static bool isMusicEnabled = true;
    public static float soundLevel = 0.9f;
    public static float musicLevel = 0.7f;
    public static bool isPlayer1Human = true;
    public static bool isPlayer2Human = true;
    public static string viewType = "perspective";

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
