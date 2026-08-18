using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private const int MAIN_GAME_SCENE = 0;
    public static GameManager Instance { get; private set; }

    private static int score = 0;
    private float time;
    private bool isTimerActive;

    public static void ResetStaticData()
    {
        score = 0;
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        PlayerController.Instance.OnPointScored += PlayerController_OnPointScored;
        PlayerController.Instance.OnStateChanged += PlayerController_OnStateChanged;
    }

    void Update()
    {
        if (isTimerActive)
        {
            time += Time.deltaTime;
        }
    }

    private void PlayerController_OnStateChanged(object sender, PlayerController.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == PlayerController.State.Normal;
    }

    private void PlayerController_OnPointScored(object sender, EventArgs e)
    {
        score++;
    }

    public float GetTime()
    {
        return time;
    }

    public bool GetTimerActive()
    {
        return isTimerActive;
    }

    public int GetScore()
    {
        return score;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(MAIN_GAME_SCENE);
        ResetStaticData();
    }
}
