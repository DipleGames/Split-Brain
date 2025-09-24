using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MG02_AudioManager : SingleTon<MG02_AudioManager>
{
    AudioSource audioSource;
    AudioClip bgm;

    [SerializeField] Slider volume_Slider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Inspector에 지정된 기본 클립 가져오기
        bgm = audioSource.clip;
        audioSource.volume = volume_Slider.value;
        volume_Slider.onValueChanged.AddListener(SetVolume);

        StopBGM();
    }

    void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    public void PlayBGM()
    {
        if (bgm != null)
        {
            audioSource.Play();
        }
    }

    // BGM 정지
    public void StopBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    // 일시정지
    public void PauseBGM()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    // 일시정지 후 다시 재생
    public void ResumeBGM()
    {
        audioSource.UnPause();
    }

    public void OnClickedSoundBtn()
    {
        Time.timeScale = 0f; // 게임 정지
        MG02_GameManager.Instance.gameState = MG02_GameManager.MG02_GameState.Pause;
        MG02_UIManager.Instance.OnSoundUI(true);
    }

    public void OnClickedOffBtn()
    {
        Time.timeScale = 1f; // 게임 재개
        MG02_GameManager.Instance.gameState = MG02_GameManager.MG02_GameState.Playing;
        MG02_UIManager.Instance.OnSoundUI(false);
        
    }
}
