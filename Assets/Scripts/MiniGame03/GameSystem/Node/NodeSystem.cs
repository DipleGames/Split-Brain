using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class NodeSystem : MonoBehaviour
{
    [Header("Slot Settings")]
    public int visibleCount = 6;  // 한 화면에 보이는 노드 수
    public float spacing = 10f;   // 노드 간 간격

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    /// <summary>
    /// 현재 패널의 슬롯 위치 리스트 반환
    /// </summary>
    public List<Vector3> GetSlotPositions()
    {
        List<Vector3> slots = new List<Vector3>();
        if (rect == null) rect = GetComponent<RectTransform>();

        float panelWidth = rect.rect.width;
        float cellWidth = (panelWidth - spacing * (visibleCount - 1)) / visibleCount;

        float startX = -panelWidth / 2f + cellWidth / 2f;
        for (int i = 0; i < visibleCount; i++)
        {
            float x = startX + i * (cellWidth + spacing);
            slots.Add(new Vector3(x, 0f, 0f));
        }

        return slots;
    }
}
