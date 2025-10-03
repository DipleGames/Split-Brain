using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum Direction { Up, Down, Left, Right }

public class MG03_NodeManager : SingleTon<MG03_NodeManager>
{
    [Header("Node Settings")]
    public GameObject nodePrefab;       // 단일 Node 프리팹
    public Transform[] nodePanels;      // 부모 컨테이너 (0: Left, 1: Right)

    [Header("Pooling Settings")]
    public int poolSize = 20;

    // 패널별 풀
    private List<Queue<Node>> nodePools = new List<Queue<Node>>();

    protected override void Awake()
    {
        base.Awake();

        // 패널 개수만큼 풀 생성
        for (int i = 0; i < nodePanels.Length; i++)
        {
            Queue<Node> pool = new Queue<Node>();

            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(nodePrefab, nodePanels[i]);
                obj.SetActive(false);

                Node node = obj.GetComponent<Node>();
                pool.Enqueue(node);
            }

            nodePools.Add(pool);
        }
    }

      private void Start()
    {
        // 시작할 때 각 패널에 10개씩 채우기
        for (int i = 0; i < nodePanels.Length; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                SpawnNode(i);
            }
        }

        // 1초마다 노드 추가
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < nodePanels.Length; i++)
            {
                SpawnNode(i);
            }
        }
    }

    /// <summary>
    /// 특정 패널에 노드를 스폰
    /// </summary>
    public Node SpawnNode(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= nodePanels.Length)
        {
            Debug.LogError("잘못된 패널 인덱스: " + panelIndex);
            return null;
        }

        Queue<Node> pool = nodePools[panelIndex];
        Node node;

        if (pool.Count > 0)
        {
            node = pool.Dequeue();
        }
        else
        {
            GameObject obj = Instantiate(nodePrefab, nodePanels[panelIndex]);
            node = obj.GetComponent<Node>();
        }

        node.transform.SetParent(nodePanels[panelIndex], false);
        node.gameObject.SetActive(true);

        // 랜덤 방향 + 패널 인덱스 전달
        Direction dir = (Direction)Random.Range(0, 4);
        node.Setup(dir, panelIndex);

        return node;
    }

    /// <summary>
    /// 노드를 해당 패널 풀에 반환
    /// </summary>
    public void ReturnNode(Node node, int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= nodePools.Count) return;

        node.gameObject.SetActive(false);
        nodePools[panelIndex].Enqueue(node);
    }
}
