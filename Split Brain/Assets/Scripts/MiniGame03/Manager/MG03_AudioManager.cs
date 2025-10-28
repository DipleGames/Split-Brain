using UnityEngine;
using UnityEngine.UI;

public class MG03_AudioManager : SingleTon<MG03_AudioManager>
{
    private AudioSource audioSource;


    [Header("효과음")]
    [SerializeField] private AudioClip success_SFX;
    [SerializeField] private AudioClip fail_SFX;

    [Header("볼륨 슬라이더")]
    [SerializeField] Slider volume_Slider;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Inspector에 지정된 기본 클립 가져오기
        audioSource.volume = volume_Slider.value;
        volume_Slider.onValueChanged.AddListener(SetVolume);
    }

    void SetVolume(float value)
    {
        audioSource.volume = value;
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
