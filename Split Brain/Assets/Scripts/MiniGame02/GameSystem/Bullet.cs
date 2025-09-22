using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float invincibleTime = 1f;

    [SerializeField] RectTransform boundArea;
    float spawnTime;

    void OnEnable()
    {
        spawnTime = Time.time; // 활성화될 때마다 초기화
    }

    public void Init(RectTransform area)
    {
        boundArea = area;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // 무적 시간 동안은 검사 스킵
        if (Time.time - spawnTime < invincibleTime) return;

        if (boundArea != null && !IsInsideArea())
        {
            MG02_BulletSystem.MG02_BulletManager.Instance.DespawnBullet(gameObject);
        }
    }

    bool IsInsideArea()
    {
        Vector3 localPos = boundArea.InverseTransformPoint(transform.position);
        return boundArea.rect.Contains(localPos);
    }
}
