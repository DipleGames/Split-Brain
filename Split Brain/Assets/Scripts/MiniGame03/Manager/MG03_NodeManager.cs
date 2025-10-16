using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public enum Direction { Up, Down, Left, Right }

public class MG03_NodeManager : SingleTon<MG03_NodeManager>
{
    [Header("Node Settings")]
    public GameObject nodePrefab;
    public NodeSystem[] nodeSystems; // Left / Right 패널

    [Header("Pooling Settings")]
    public int poolSize = 20;
    [SerializeField] private float moveDuration = 0.25f;

    private List<Queue<Node>> nodePools = new List<Queue<Node>>();
    private List<List<Node>> activeNodes = new List<List<Node>>();

    protected override void Awake()
    {
        base.Awake();

        // 패널별 풀 초기화
        for (int i = 0; i < nodeSystems.Length; i++)
        {
            nodePools.Add(new Queue<Node>());
            activeNodes.Add(new List<Node>());

            for (int j = 0; j < poolSize; j++)
            {
                GameObject obj = Instantiate(nodePrefab, nodeSystems[i].transform);
                obj.SetActive(false);
                nodePools[i].Enqueue(obj.GetComponent<Node>());
            }
        }
    }

    private void Start()
    {
        // 처음에 각 패널마다 6개씩 노드 채우기
        for (int i = 0; i < nodeSystems.Length; i++)
        {
            var slots = nodeSystems[i].GetSlotPositions();

            for (int j = 0; j < slots.Count; j++)
            {
                Node node = SpawnNode(i);
                node.transform.localPosition = slots[j];
                activeNodes[i].Add(node);
            }
        }
    }

    /// <summary>
    /// 외부 입력 (예: 방향키 터치 등) 시 호출
    /// </summary>
    public void OnUpButtonPressed(int panelIndex)
    {
        OnNodeInput(panelIndex, Direction.Up);
    }
    public void OnDownButtonPressed(int panelIndex)
    {
        OnNodeInput(panelIndex, Direction.Down);
    }
    public void OnLeftButtonPressed(int panelIndex)
    {
        OnNodeInput(panelIndex, Direction.Left);
    }
    public void OnRightButtonPressed(int panelIndex)
    {
        OnNodeInput(panelIndex, Direction.Right);
    }

    public void OnNodeInput(int panelIndex, Direction dir)
    {
        SlideAndAddNode(panelIndex, dir);
    }

    /// <summary>
    /// 가장 왼쪽 노드를 제거하고 전체를 왼쪽으로 밀고 새 노드를 추가
    /// </summary>
    private void SlideAndAddNode(int panelIndex, Direction dir)
    {
        var slots = nodeSystems[panelIndex].GetSlotPositions();
        var list = activeNodes[panelIndex];

        if (list.Count == 0) return;

        // 1. 맨 앞 노드 "팡!" 제거 (간단한 스케일 애니메이션)
        Node removed = list[0];
        if (removed.direction != dir) return; // 방향다르면 안됨.

        list.RemoveAt(0);

        removed.transform.DOScale(0f, 0.15f).SetEase(Ease.InBack).OnComplete(() =>
        {
            removed.transform.localScale = Vector3.one;
            MG03_HealthManager.Instance.RecoverHealth(1, panelIndex);
            ReturnNode(removed, panelIndex);
        });

        // 2. 나머지 노드 왼쪽으로 한 칸 이동
        for (int i = 0; i < list.Count; i++)
        {
            list[i].transform.DOLocalMove(slots[i], moveDuration).SetEase(Ease.OutQuad);
        }

        // 3. 새 노드 생성 (맨 오른쪽에 등장)
        Node newNode = SpawnNode(panelIndex);
        Vector3 startPos = slots[slots.Count - 1] + new Vector3(200f, 0, 0);
        newNode.transform.localPosition = startPos;

        newNode.transform.DOLocalMove(slots[slots.Count - 1], moveDuration)
            .SetEase(Ease.OutQuad);

        list.Add(newNode);
    }

    // SpawnNode 안에 크기 자동 보정 추가
    private Node SpawnNode(int panelIndex)
    {
        Queue<Node> pool = nodePools[panelIndex];
        Node node = (pool.Count > 0)
            ? pool.Dequeue()
            : Instantiate(nodePrefab, nodeSystems[panelIndex].transform).GetComponent<Node>();

        node.gameObject.SetActive(true);
        node.Setup((Direction)Random.Range(0, 4), panelIndex);

        // 노드 크기 자동 조정 (6칸 균등)
        RectTransform panelRect = nodeSystems[panelIndex].GetComponent<RectTransform>();
        RectTransform nodeRect = node.GetComponent<RectTransform>();

        float panelWidth = panelRect.rect.width;
        float spacing = nodeSystems[panelIndex].spacing;
        float cellWidth = (panelWidth - spacing * (nodeSystems[panelIndex].visibleCount - 1)) / nodeSystems[panelIndex].visibleCount;
        nodeRect.sizeDelta = new Vector2(cellWidth, panelRect.rect.height);

        return node;
    }

    private void ReturnNode(Node node, int panelIndex)
    {
        node.gameObject.SetActive(false);
        nodePools[panelIndex].Enqueue(node);
    }
}
