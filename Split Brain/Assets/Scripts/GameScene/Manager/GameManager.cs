using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public enum GameState { Ready, Playing, GameOver } // 상태 패턴

    public GameState gameState = GameState.Ready;
    public GameObject obstacleSpawner;

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
                    case GameState.Ready:
                        StartCoroutine("GameStart");
                        break;
                    case GameState.GameOver:
                        Ready();
                        break;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine("GameStart");
        }
    }

    IEnumerator GameStart()
    {
        Debug.Log("게임 시작");
        yield return new WaitForSeconds(0.1f); // 바로 터치 방지를 위한 인풋딜레이

        gameState = GameState.Playing;
        obstacleSpawner.SetActive(true);
        AudioManager.Instance.PlayBGM();
        UIManager.Instance.OnGameUI(gameState);
    }

    void Ready()
    {
        Debug.Log("게임 준비");
        gameState = GameState.Ready;
        TouchManager.Instance.leftController.LeftResetPos();
        TouchManager.Instance.rightController.RightResetPos();
        ScoreManager.Instance.Score = 0;
        UIManager.Instance.OnGameUI(gameState);
    }

    public void GameOver()
    {
        Debug.Log("게임 종료");
        gameState = GameState.GameOver;
        ObstacleSpawner.Instance.DespawnAll();
        obstacleSpawner.SetActive(false);
        AudioManager.Instance.StopBGM();
        UIManager.Instance.OnGameUI(gameState);
    }
}
