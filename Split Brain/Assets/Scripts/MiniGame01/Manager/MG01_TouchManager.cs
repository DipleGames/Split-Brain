using UnityEngine;

public class MG01_TouchManager : SingleTon<MG01_TouchManager>
{
    public MG01_LController leftController;
    public MG01_RController rightController;

    void Update()
    {
        if (MG01_GameManager.Instance.gameState != MG01_GameManager.MG01_GameState.Playing) return;

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);

            // 터치 시작, 이동, 유지 상태 모두 감지
            if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
            {
                if (t.position.x < Screen.width * 0.5f)
                {
                    leftController.OnTouch(t);
                }
                else
                {
                    rightController.OnTouch(t);
                }
            }

            // 터치 종료 전달
            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
            {
                if (t.position.x < Screen.width * 0.5f)
                {
                    leftController.OnTouchEnd(t);
                }
                else
                {
                    rightController.OnTouchEnd(t);
                }
            }
        }
    }
}
