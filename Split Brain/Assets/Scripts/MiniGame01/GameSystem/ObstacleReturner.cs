using UnityEngine;

public class ObstacleReturner : MonoBehaviour
{
    [SerializeField] ObstacleSpawner spawner;

    void Awake()
    {
        spawner = ObstacleSpawner.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (spawner != null)
                spawner.Return(gameObject);
            else
                gameObject.SetActive(false);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (spawner != null)
            {
                spawner.Return(gameObject);
                MG01_GameManager.Instance.GameOver();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
