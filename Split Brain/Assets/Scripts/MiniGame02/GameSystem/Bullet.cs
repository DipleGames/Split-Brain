using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] float lifeTime = 5f;
    float timer;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        // 발사 당시 방향(transform.up)을 기준으로 직진
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            MG02_BulletSystem.MG02_BulletManager.Instance.DespawnBullet(gameObject);
        }
    }
}