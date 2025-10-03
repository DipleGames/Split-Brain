using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [Header("Arrow Sprites")]
    [SerializeField] private Sprite[] arrowSprites; // 0:Up, 1:Right, 2:Left, 3:Down
    [SerializeField] private Image arrowImage;      // 실제 표시될 Image 컴포넌트

    private Direction direction;
    private int panelIndex; // 내가 속한 패널 인덱스

    public void Setup(Direction dir, int panelIdx)
    {
        direction = dir;
        panelIndex = panelIdx;

        // 해당 방향에 맞는 스프라이트 적용
        switch (dir)
        {
            case Direction.Up:
                arrowImage.sprite = arrowSprites[0];
                break;
            case Direction.Right:
                arrowImage.sprite = arrowSprites[1];
                break;
            case Direction.Left:
                arrowImage.sprite = arrowSprites[2];
                break;
            case Direction.Down:
                arrowImage.sprite = arrowSprites[3];
                break;
        }

        // 혹시 투명도 문제 방지 (비활성 상태였다면 켜줌)
        arrowImage.enabled = true;
    }

    public Direction GetDirection() => direction;

    public void ReturnToPool()
    {
        MG03_NodeManager.Instance.ReturnNode(this, panelIndex);
    }
}
