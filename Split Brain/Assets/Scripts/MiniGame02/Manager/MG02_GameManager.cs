using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MG02_GameManager : SingleTon<MG02_GameManager>
{
    
    public enum MG02_GameState { Ready, Playing, Pause, GameOver } // 상태 패턴

    public MG02_GameState gameState = MG02_GameState.Ready;


    void Update()
    {
        // 화면에 손가락이 닿아 있는 경우
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0); // 첫 번째 터치 정보 가져오기

            // 터치가 시작된 순간(손가락이 닿는 순간)만 감지
            if (t.phase == TouchPhase.Began)
            {
                switch (gameState)
                {
                    case MG02_GameState.Ready:
                        StartCoroutine("GameStart");
                        break;
                    case MG02_GameState.GameOver:
                        Ready();
                        break;
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if(gameState == MG02_GameState.Ready)
                StartCoroutine("GameStart");
        }
    }

     IEnumerator GameStart()
    {
        Debug.Log("게임 시작");
        yield return new WaitForSeconds(0.1f); // 바로 터치 방지를 위한 인풋딜레이

        gameState = MG02_GameState.Playing;
        //MG02_AudioManager.Instance.PlayBGM();
        MG02_UIManager.Instance.OnGameUI(gameState);
    }

    void Ready()
    {
        Debug.Log("게임 준비");
        gameState = MG02_GameState.Ready;
        MG02_ScoreManager.Instance.Score = 0;
        MG02_UIManager.Instance.OnGameUI(gameState);
    }

    public void GameOver()
    {
        Debug.Log("게임 종료");
        gameState = MG02_GameState.GameOver;
        //MG02_AudioManager.Instance.StopBGM();
        MG02_UIManager.Instance.OnGameUI(gameState);
    }


    public void PauseGame()
    {
        Time.timeScale = 0f; // 게임 정지
        MG02_UIManager.Instance.OnPauseUI(gameState);
        gameState = MG02_GameState.Pause;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // 게임 재개
        MG02_UIManager.Instance.OnPauseUI(gameState);
        gameState = MG02_GameState.Playing;
    }

    public void GoToLobby()
    {
        Time.timeScale = 1f; 
        Ready();
        SceneManager.LoadScene("Lobby");
    }
}
