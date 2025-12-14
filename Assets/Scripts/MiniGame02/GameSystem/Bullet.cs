using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    [SerializeField] RectTransform boundArea;

    public void Init(RectTransform area)
    {
        boundArea = area;
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

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
