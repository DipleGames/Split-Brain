using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MG03_ScoreManager : SingleTon<MG03_ScoreManager>
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


    public void AddScore()
    {
        Score += 1f;       
    }
}
