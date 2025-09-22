using UnityEngine;

public class MG02_RController : MonoBehaviour
{
    Vector2 startTouchPos;
    Vector3 startPlayerPos;
    bool holding = false;

    Camera cam;
    Rigidbody2D rb;

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            holding = true;
            startTouchPos = touch.position;         // 터치 시작 위치 (스크린 좌표)
            startPlayerPos = transform.position;    // 캐릭터 시작 위치 (월드 좌표)
        }
        else if (holding && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
        {
            // 현재 터치 위치와 시작 위치 차이 (스크린 좌표 단위)
            Vector2 delta = touch.position - startTouchPos;

            // 화면 좌표 → 월드 좌표 보정 (픽셀 차이를 월드 거리로 변환)
            Vector3 worldDelta = cam.ScreenToWorldPoint(new Vector3(delta.x, delta.y, 0)) 
                               - cam.ScreenToWorldPoint(Vector3.zero);

            // 캐릭터 위치 갱신
            transform.position = startPlayerPos + worldDelta;
        }
    }

    public void OnTouchEnd(Touch touch)
    {
        holding = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            MG02_GameManager.Instance.GameOver();
        }
    }
}
