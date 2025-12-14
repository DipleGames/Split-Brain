using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : SingleTon<ObstacleSpawner>
{
    [Header("Refs")]
    [SerializeField] Camera cam;
    [SerializeField] GameObject obstaclePrefab;

    [Header("Spawn Area (world)")]
    [SerializeField] float spawnZ = 0f;          // 장애물이 놓일 Z
    [SerializeField] float yOffset = 0.5f;       // 화면 위로 얼마나 더 위에서 스폰할지
    [SerializeField] float centerGapWorld = 0.6f;// 가운데 금지 구역(월드 단위 폭)

    [Header("Timing")]
    [SerializeField] float minDelay = 0.8f;
    [SerializeField] float maxDelay = 1.5f;

    [Header("Speed")]
    [SerializeField] float baseSpeed = 100f;       // 시작 낙하 속도
    [SerializeField] float speedPer20Sec = 20f;  // 분당 추가 속도

    [Header("Pooling")]
    [SerializeField] int poolSize = 32;

    float leftX, rightX, midX, topY, depthToPlane;
    float leftMin, leftMax, rightMin, rightMax;
    readonly Queue<GameObject> pool = new Queue<GameObject>();

    enum Side { Left, Right }

    protected override void Awake()
    {
        base.Awake();

        if (!cam) cam = Camera.main;
        depthToPlane = spawnZ - cam.transform.position.z;

        // 화면 상단 월드 경계
        Vector3 topLeft = cam.ViewportToWorldPoint(new Vector3(0f, 1f, depthToPlane));
        Vector3 topRight = cam.ViewportToWorldPoint(new Vector3(1f, 1f, depthToPlane));
        leftX = Mathf.Min(topLeft.x, topRight.x);
        rightX = Mathf.Max(topLeft.x, topRight.x);
        midX = (leftX + rightX) * 0.5f;
        topY = Mathf.Max(topLeft.y, topRight.y) + yOffset;

        // 가운데 금지 구역을 고려한 좌/우 스폰 범위
        float halfGap = Mathf.Max(0f, centerGapWorld * 0.5f);
        leftMin = leftX;
        leftMax = Mathf.Min(midX - halfGap, rightX);
        rightMin = Mathf.Max(midX + halfGap, leftX);
        rightMax = rightX;

        // 풀 생성
        for (int i = 0; i < poolSize; i++)
        {
            var go = Instantiate(obstaclePrefab, gameObject.transform);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

    void OnEnable()
    {
        StartCoroutine(SpawnLoop(Side.Left));   // 왼쪽 독립 스폰
        StartCoroutine(SpawnLoop(Side.Right));  // 오른쪽 독립 스폰
    }

    void OnDisable() => StopAllCoroutines();


    IEnumerator SpawnLoop(Side side)    
    {
        float startTime = Time.time;

        while (true)
        {
            float t = (Time.time - startTime) / 20f; // minutes
            float speed = baseSpeed + speedPer20Sec * t;

            float x = RandomX(side);
            SpawnAt(x, speed);

            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
        }
    }

    float RandomX(Side side)
    {
        if (side == Side.Left)
        {
            if (leftMin > leftMax) return leftMax; // 안전장치
            return Random.Range(leftMin, leftMax);
        }
        else
        {
            if (rightMin > rightMax) return rightMin; // 안전장치
            return Random.Range(rightMin, rightMax);
        }
    }

    readonly HashSet<GameObject> active = new HashSet<GameObject>();
    void SpawnAt(float x, float fallSpeed)
    {
        var go = Get();
        var p = go.transform.position;
        p.x = x; p.y = topY; p.z = spawnZ;
        go.transform.position = p;
        Debug.Log(go.transform.position);
        go.SetActive(true);

        var rb = go.GetComponent<Rigidbody2D>();
        if (rb)
        {
            float w = Random.Range(3, 10);
            rb.velocity = Vector2.down * fallSpeed * w;
        }
        active.Add(go);
    }

    GameObject Get()
    {
        if (pool.Count > 0)
        {
            var g = pool.Dequeue();
            return g;
        }
        return Instantiate(obstaclePrefab);
    }

    public void Return(GameObject go)
    {
        if (!go) return;

        if (active.Remove(go)) // 활성에 있었던 것만 처리
        {
            var rb = go.GetComponent<Rigidbody2D>();
            if (rb) rb.velocity = Vector2.zero;

            go.SetActive(false);
            go.transform.SetParent(transform, false);
            pool.Enqueue(go);
        }
    }

    public void DespawnAll()
    {
        var snapshot = new List<GameObject>(active);
        foreach (var go in snapshot) Return(go);
        active.Clear();
    }
}
