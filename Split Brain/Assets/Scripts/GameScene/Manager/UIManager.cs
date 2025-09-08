using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingleTon<UIManager>
{
    [Header("게임진행 관련 UI")]
    public GameObject gameStart_UI;
    public GameObject gameOver_UI;
    public GameObject PauseGame_UI;
    public GameObject Sound_UI;

    [Header("스코어 관련 UI")]
    public Text score_Text;
    public Text gameOverScore_Text;

    [Header("버튼")]
    public GameObject pause_Btn;
    public GameObject sound_Btn;

    void Start()
    {
        ScoreManager.Instance.OnChangedScore += OnChangedScoreUI;
    }

    void OnChangedScoreUI(float score)
    {
        score_Text.text = $"Score : {score:F0}";
    }

    void OnBtnUI(bool b)
    {
        pause_Btn.SetActive(b);
        sound_Btn.SetActive(b);
    }

    public void OnGameUI(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Playing:
                gameStart_UI.SetActive(false);
                OnBtnUI(true);
                break;
            case GameManager.GameState.Ready:
                gameOver_UI.SetActive(false);
                gameStart_UI.SetActive(true);
                OnBtnUI(false);
                break;
            case GameManager.GameState.GameOver:
                gameOver_UI.SetActive(true);
                gameOverScore_Text.text = $"Score : {ScoreManager.Instance.Score:F0}";
                break;
        }
    }

    public void OnPauseUI(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Playing:
                PauseGame_UI.SetActive(true);
                break;
            case GameManager.GameState.Pause:
                PauseGame_UI.SetActive(false);
                break;
        }
    }

    public void OnSoundUI(bool b)
    {
        Sound_UI.SetActive(b);
    }
}