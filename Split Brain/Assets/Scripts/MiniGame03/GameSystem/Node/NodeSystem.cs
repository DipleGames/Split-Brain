using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeSystem : MonoBehaviour
{
    void Start()
    {
        UpdateCellSize();
    }

    void UpdateCellSize()
    {
        var rect = GetComponent<RectTransform>().rect;
        int minVisibleNodes = 6;

        float cellWidth = rect.width / minVisibleNodes;
        float cellHeight = rect.height; // 높이는 패널 전체 차지

        var grid = GetComponent<GridLayoutGroup>();
        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }

}
