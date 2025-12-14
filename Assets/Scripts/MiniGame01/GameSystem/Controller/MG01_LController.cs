using UnityEngine;

public class MG01_LController : MonoBehaviour
{
    [SerializeField] Camera cam; // 비워두면 자동 할당
    float startTouchScreenX;
    float startObjWorldX;
    float objScreenZ; // 월드<->스크린 변환용 Z(깊이)
    bool holding;

    [SerializeField] float z, minX, maxX, margin;

    void Awake()
    {
        if (!cam) cam = Camera.main;

        z = cam.WorldToScreenPoint(transform.position).z;
        Vector3 worldLeft = cam.ViewportToWorldPoint(new Vector3(0f, 0.5f, z));
        minX = worldLeft.x + margin;
    }

    public void LeftResetPos()
    {
        transform.position = cam.ViewportToWorldPoint(new Vector3(0.25f, 0.5f, z));
        transform.position = new Vector3(transform.position.x, -307, z);
    }

    public void OnTouch(Touch touch)
    {
        if (touch.phase == TouchPhase.Began)
        {
            // 기준값 캐싱: 터치 시작 화면 X, 오브젝트 시작 월드 X, 변환용 Z
            startTouchScreenX = touch.position.x;
            startObjWorldX = transform.position.x;
            objScreenZ = cam.WorldToScreenPoint(transform.position).z;
            holding = true;
            return;
        }

        if (!holding) return;

        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            // 드래그한 화면 X 변화량
            float deltaScreenX = touch.position.x - startTouchScreenX;

            // 화면 X 변화량을 월드 X 변화량으로 변환
            Vector3 worldAtStart = cam.ScreenToWorldPoint(new Vector3(startTouchScreenX, 0f, objScreenZ));
            Vector3 worldAtNow = cam.ScreenToWorldPoint(new Vector3(startTouchScreenX + deltaScreenX, 0f, objScreenZ));
            float deltaWorldX = worldAtNow.x - worldAtStart.x;

            // 상하(Y) 고정, X만 상대 이동
            Vector3 p = transform.position;
            p.x = Mathf.Clamp(startObjWorldX + deltaWorldX, minX, maxX);
            transform.position = p;
        }
    }

    public void OnTouchEnd(Touch touch)
    {
        holding = false;
    }
}
