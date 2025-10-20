using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MG03_AudioManager : SingleTon<MG03_AudioManager>
{
    private AudioSource audioSource;

    [Header("효과음")]
    [SerializeField] private AudioClip success_SFX;
    [SerializeField] private AudioClip fail_SFX;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(bool success)
    {
        audioSource.clip = success ? success_SFX : fail_SFX;
        audioSource.Play();
    }

    public void OnClickedSoundBtn()
    {
        Time.timeScale = 0f; // 게임 정지
        MG03_GameManager.Instance.gameState = MG03_GameManager.MG03_GameState.Pause;
        MG03_UIManager.Instance.OnSoundUI(true);
    }

    public void OnClickedOffBtn()
    {
        Time.timeScale = 1f; // 게임 재개
        MG03_GameManager.Instance.gameState = MG03_GameManager.MG03_GameState.Playing;
        MG03_UIManager.Instance.OnSoundUI(false);
        
    }
}
