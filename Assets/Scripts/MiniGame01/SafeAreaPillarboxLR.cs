using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SafeAreaPillarboxLR : MonoBehaviour
{
    Camera cam;

    void OnEnable() { cam = GetComponent<Camera>(); Apply(); }
    void OnRectTransformDimensionsChange() { Apply(); } // 회전/해상도 변동 대응

    void Apply()
    {
        Rect sa = Screen.safeArea;     // 픽셀 단위 안전영역
        float w = Screen.width;
        float h = Screen.height;

        // 좌우만 반영(필러박스): x, width는 Safe Area 사용 / y는 전체(0~1) 유지
        float x = sa.xMin / w;
        float width = sa.width / w;

        cam.rect = new Rect(x, 0f, width, 1f);
    }
}
