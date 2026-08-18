using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreTextMesh;
    [SerializeField] private Button restartButton;

    private Action restartButtonClickAction;


    private void Awake()
    {
        restartButton.onClick.AddListener(() =>
        {
            restartButtonClickAction();
        });
    }

    private void Start()
    {
        PlayerController.Instance.OnGameOver += PlayerController_OnGameOver;
        Hide();
    }

    private void Update()
    {
        UpdateScoreTextMesh();
    }

    private void UpdateScoreTextMesh()
    {
        scoreTextMesh.text = "" + GameManager.Instance.GetScore();
    }

    private void PlayerController_OnGameOver(object sender, EventArgs e)
    {
        Show();

        restartButtonClickAction = GameManager.Instance.RestartGame;
    }

    private void Show()
    {
        restartButton.gameObject.SetActive(true);
    }

    private void Hide()
    {
        restartButton.gameObject.SetActive(false);
    }


}
