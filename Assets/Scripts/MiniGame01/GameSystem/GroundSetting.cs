using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSetting : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] RectTransform groundRect;
    public static float GroundTopY { get; private set; }

    void Awake()
    {
        if (!cam) cam = Camera.main;
    }

    void Start()
    {
        Vector3[] worldCorners = new Vector3[4];
        groundRect.GetWorldCorners(worldCorners);

        // worldCorners[1] = TopLeft
        GroundTopY = worldCorners[1].y;

        Debug.Log($"땅의 높이는 {GroundTopY}입니다.");
    }
}
