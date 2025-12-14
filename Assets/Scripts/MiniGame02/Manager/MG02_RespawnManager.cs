using UnityEngine;

public class MG02_RespawnManager : SingleTon<MG02_RespawnManager>
{
    [SerializeField] RectTransform[] uiArea;   // 기준이 되는 UI 영역
    [SerializeField] Transform[] player;       // 플레이어 오브젝트

    /// <summary>
    /// UI 영역의 비율 좌표(normalizedPos)로 플레이어 이동
    /// </summary>
    public void MovePlayerToUIPos(int index, Vector2 normalizedPos)
    {
        // UI 로컬 좌표 계산 (0,0=왼쪽아래 / 1,1=오른쪽위)
        Vector3 localPos = new Vector3(
            Mathf.Lerp(uiArea[index].rect.xMin, uiArea[index].rect.xMax, normalizedPos.x),
            Mathf.Lerp(uiArea[index].rect.yMin, uiArea[index].rect.yMax, normalizedPos.y),
            0f);

        // UI 로컬 좌표 → 월드 좌표 변환
        Vector3 worldPos = uiArea[index].TransformPoint(localPos);
        worldPos.z = 0f;

        // 플레이어 이동
        player[index].position = worldPos;
        Debug.Log($"player{index} 가 {player[index].position} 으로 이동했습니다.");
    }
}
