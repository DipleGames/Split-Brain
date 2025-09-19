using UnityEngine;
using UnityEngine.EventSystems;

public class MG02_RController : MonoBehaviour
{
    [SerializeField] Camera cam; 
    [SerializeField] Canvas canvas; // UI 캔버스
    [SerializeField] PinePie.SimpleJoystick.JoystickController joystick; // 연결

    [Header("캐릭터 스피드")]
    [SerializeField] float moveSpeed = 5f;

    Rigidbody2D rb;
    bool holding;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 dir = joystick.InputDirection;

        if (dir.sqrMagnitude > 0.01f) 
        {
            dir = dir.normalized; // 방향만 유지하고 크기는 1로 고정
            rb.velocity = dir * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // 입력 없으면 정지
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            MG02_GameManager.Instance.GameOver();
        }
    }


    public void OnTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            holding = true;
        }

        if (!holding) return;
    }

    public void OnTouchEnd(Touch touch)
    {
        holding = false;
    }
}
