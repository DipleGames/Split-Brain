using UnityEngine;

public class RectClamp : MonoBehaviour
{
    [SerializeField] RectTransform rectPanel; // 네모 패널 (UI)
    [SerializeField] Transform player;        // 플레이어 오브젝트

    void Update()
    {
        // 패널의 월드 좌표 기준 최소/최대 모서리 구하기
        Vector3[] corners = new Vector3[4];
        rectPanel.GetWorldCorners(corners);

        float minX = corners[0].x;
        float maxX = corners[2].x;
        float minY = corners[0].y;
        float maxY = corners[1].y;

        // 플레이어 위치 Clamp
        Vector3 pos = player.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        player.position = pos;
    }
}