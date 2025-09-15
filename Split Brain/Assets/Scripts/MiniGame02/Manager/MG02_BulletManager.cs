using UnityEngine;
using System.Collections.Generic;

namespace MG02_BulletSystem
{
    [System.Serializable]
    public class BulletPooler
    {
        [SerializeField] GameObject prefab;
        [SerializeField] int poolSize = 30;
        private Queue<GameObject> pool = new Queue<GameObject>();
        private Transform parent;

        public void Init(Transform parent)
        {
            this.parent = parent;
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab, parent);
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
        }

        public GameObject GetBullet(Vector3 pos, Quaternion rot)
        {
            GameObject obj = pool.Count > 0 ? pool.Dequeue() :
                GameObject.Instantiate(prefab, parent);

            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);
            return obj;
        }

        public void ReturnBullet(GameObject obj)
        {
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    [System.Serializable]
    public class SpawnArea
    {
        public Transform top;     // 영역 위쪽
        public Transform bottom;  // 영역 아래쪽

        public Vector3 GetRandomPosition()
        {
            float x = Random.Range(bottom.position.x, top.position.x);
            float y = Random.Range(bottom.position.y, top.position.y);
            return new Vector3(x, y, 0f);
        }
    }

    // Bullet Manager
    public class MG02_BulletManager : SingleTon<MG02_BulletManager>
    {
        [Header("Pool Settings")]
        [SerializeField] BulletPooler bulletPool;

        [Header("Spawn Areas (UI)")]
        [SerializeField] RectTransform leftArea;
        [SerializeField] RectTransform rightArea;

        [Header("Target")]
        [SerializeField] Transform left;
        [SerializeField] Transform right;


        protected override void Awake()
        {
            bulletPool.Init(transform);
        }

        float delay = 3f;
        float patternTime = 0f;
        void Update()
        {
            patternTime += Time.deltaTime;
            if (patternTime >= delay)
            {
                Pattern_RandomInLeft();
                Pattern_RandomInRight();
                patternTime = 0f;
            }
        }

        // 총알 하나 가져오기
        public GameObject SpawnBullet(Vector3 pos, Quaternion rot)
        {
            return bulletPool.GetBullet(pos, rot);
        }

        // 총알 반환하기
        public void DespawnBullet(GameObject bullet)
        {
            bulletPool.ReturnBullet(bullet);
        }
        
        Vector3 GetRandomPositionOnRectSide(RectTransform rect)
        {
            // rect 기준 local 좌표 랜덤
            float x, y;
            int side = Random.Range(0, 4); // 0=Left, 1=Right, 2=Top, 3=Bottom

            switch (side)
            {
                case 0: // Left
                    x = rect.rect.xMin;
                    y = Random.Range(rect.rect.yMin, rect.rect.yMax);
                    break;
                case 1: // Right
                    x = rect.rect.xMax;
                    y = Random.Range(rect.rect.yMin, rect.rect.yMax);
                    break;
                case 2: // Top
                    x = Random.Range(rect.rect.xMin, rect.rect.xMax);
                    y = rect.rect.yMax;
                    break;
                default: // Bottom
                    x = Random.Range(rect.rect.xMin, rect.rect.xMax);
                    y = rect.rect.yMin;
                    break;
            }

            Vector3 localPos = new Vector3(x, y, 0f);
            return rect.TransformPoint(localPos); // UI 좌표 → 월드 좌표 변환
        }
        public void Pattern_RandomInLeft()
        {
            Vector3 pos = GetRandomPositionOnRectSide(leftArea);

            // 스폰된 위치 → 플레이어 방향
            Vector3 dir = (left.position - pos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; 
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            SpawnBullet(pos, rot);
        }
        public void Pattern_RandomInRight()
        {
            Vector3 pos = GetRandomPositionOnRectSide(rightArea);

            // 스폰된 위치 → 플레이어 방향
            Vector3 dir = (right.position - pos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f; 
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            SpawnBullet(pos, rot);
        }
    }
}
