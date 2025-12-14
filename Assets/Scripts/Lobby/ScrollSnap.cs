using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ScrollSnap : MonoBehaviour
{
    [Header("Scroll Settings")]
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] RectTransform content;
    [SerializeField] float snapSpeed = 10f;

    [Header("Game Info")]
    [SerializeField] string[] gameSceneNames;   // 카드별 실행할 씬 이름
    [SerializeField] Button startButton;        // 게임 시작 버튼
    [SerializeField] Button leftArrow;          // 왼쪽 화살표 버튼
    [SerializeField] Button rightArrow;         // 오른쪽 화살표 버튼

    float[] pagePositions;
    int pageCount;
    int targetPage;
    bool dragging;

    void Start()
    {
        pageCount = content.childCount;
        pagePositions = new float[pageCount];

        if (pageCount == 1)
        {
            pagePositions[0] = 0f;
        }
        else
        {
            for (int i = 0; i < pageCount; i++)
                pagePositions[i] = (float)i / (pageCount - 1);
        }

        targetPage = 0;

        if (startButton != null)
            startButton.onClick.AddListener(StartSelectedGame);

        if (leftArrow != null)
            leftArrow.onClick.AddListener(() => MovePage(-1));
        if (rightArrow != null)
            rightArrow.onClick.AddListener(() => MovePage(1));

        UpdateArrows();
    }

    void Update()
    {
        if (!dragging && pageCount > 0)
        {
            float target = pagePositions[targetPage];
            float newX = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, target, Time.deltaTime * snapSpeed);
            scrollRect.horizontalNormalizedPosition = newX;
        }
    }

    public void OnBeginDrag() => dragging = true;

    public void OnEndDrag()
    {
        dragging = false;

        if (pageCount == 0) return;

        float pos = scrollRect.horizontalNormalizedPosition;
        float closestDist = float.MaxValue;
        for (int i = 0; i < pageCount; i++)
        {
            float dist = Mathf.Abs(pagePositions[i] - pos);
            if (dist < closestDist)
            {
                closestDist = dist;
                targetPage = i;
            }
        }

        targetPage = Mathf.Clamp(targetPage, 0, pageCount - 1);
        UpdateArrows();
    }

    void MovePage(int dir)
    {
        targetPage = Mathf.Clamp(targetPage + dir, 0, pageCount - 1);
        LobbyManager.Instance.ChangeMiniGame(targetPage);
        Debug.Log($"{targetPage}");
        UpdateArrows();
    }

    void UpdateArrows()
    {
        if (leftArrow != null) leftArrow.gameObject.SetActive(targetPage > 0);
        if (rightArrow != null) rightArrow.gameObject.SetActive(targetPage < pageCount - 1);
    }

    void StartSelectedGame()
    {
        if (targetPage >= 0 && targetPage < gameSceneNames.Length)
        {
            string sceneName = gameSceneNames[targetPage];
            Debug.Log($"게임 시작: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("선택된 게임에 해당하는 씬 이름이 없습니다!");
        }
    }
}

