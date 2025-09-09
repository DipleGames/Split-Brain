using System;
using UnityEngine;

public class MG01_ScoreManager : SingleTon<MG01_ScoreManager>
{
    public event Action<float> OnChangedScore;

    private float _score;
    public float Score
    {
        get { return _score; }
        set
        {
            if (Mathf.Approximately(_score, value)) return; // 불필요한 호출 방지
            _score = value;                                  
            OnChangedScore?.Invoke(_score);                 
        }
    }

    private float _tick; // 1초 누적용

    void Update()
    {
        if (MG01_GameManager.Instance.gameState != MG01_GameManager.GameState.Playing) return;

        _tick += Time.deltaTime;
        if (_tick >= 1f)
        {
            Score += 1f;       
            _tick -= 1f;       
        }
    }
}
