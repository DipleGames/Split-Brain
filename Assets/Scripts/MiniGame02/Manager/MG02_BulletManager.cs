using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace MG02_BulletSystem
{
    [System.Serializable]
    public class BulletPooler
    {
        [SerializeField] GameObject prefab;
        [SerializeField] int poolSize = 30;
        private Queue<GameObject> pool = new Queue<GameObject>();
        private Transform parent;

        // 현재 활성화된 총알 리스트
        private List<GameObject> activeBullets = new List<GameObject>();

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

            // 활성 리스트에 추가
            activeBullets.Add(obj);

            return obj;
        }

        public void ReturnBullet(GameObject obj)
        {
            if (activeBullets.Contains(obj))
                activeBullets.Remove(obj);

            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        // 모든 총알 반환
        public void DespawnAll()
        {
            for (int i = activeBullets.Count - 1; i >= 0; i--)
            {
                GameObject obj = activeBullets[i];
                obj.SetActive(false);
                pool.Enqueue(obj);
            }
            activeBullets.Clear();
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
        public RectTransform leftArea;
        public RectTransform rightArea;

        [Header("Target")]
        [SerializeField] Transform left;
        [SerializeField] Transform right;


        protected override void Awake()
        {
            bulletPool.Init(transform);
        }

        float defaultDelay = 0.5f;  // 기본 패턴 간격
        float specialDelay = 3f;    // 특수 패턴 간격

        void Start()
        {
            StartCoroutine(DefaultPatternLoop());
            StartCoroutine(SpecialPatternLoop());
        }
        
        IEnumerator DefaultPatternLoop()
        {
            while (true)
            {
                if (MG02_GameManager.Instance.gameState == MG02_GameManager.MG02_GameState.Playing)
                {
                    if (Random.Range(0, 2) == 0)
                        Pattern_DefalutInLeft();
                    else
                        Pattern_DefalutInRight();
                }

                yield return new WaitForSeconds(defaultDelay);
            }
        }

        IEnumerator SpecialPatternLoop()
        {
            while (true)
            {
                if (MG02_GameManager.Instance.gameState == MG02_GameManager.MG02_GameState.Playing)
                {
                    int rand = Random.Range(0, 3);
                    switch (rand)
                    {
                        case 0:
                            if (Random.Range(0, 2) == 0)
                                Pattern_Circle(leftArea.position, leftArea, 16);
                            else
                                Pattern_Circle(rightArea.position, rightArea, 16);
                            break;
                        case 1:
                            if (Random.Range(0, 2) == 0)
                                Pattern_Burst(Vector3.zero, left.position, leftArea, 7, 60f);
                            else
                                Pattern_Burst(Vector3.zero, right.position, rightArea, 7, 60f);
                            break;
                        case 2:
                            if (Random.Range(0, 2) == 0)
                                Pattern_Line(leftArea, 8);
                            else
                                Pattern_Line(rightArea, 8);
                            break;
                    }
                }

                yield return new WaitForSeconds(specialDelay);
            }
        }

        // 총알 하나 가져오기
        public GameObject SpawnBullet(Vector3 pos, Quaternion rot, RectTransform area)
        {
            GameObject bullet = bulletPool.GetBullet(pos, rot);

            Bullet b = bullet.GetComponent<Bullet>();
            if (b != null)
            {
                b.Init(area); // 태생부터 귀속 확정
            }

            return bullet;
        }

        // 총알 반환하기
        public void DespawnBullet(GameObject bullet)
        {
            bulletPool.ReturnBullet(bullet);
        }

        public void DespawnAllBullets()
        {
            bulletPool.DespawnAll();
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

        public void Pattern_DefalutInLeft()
        {
            Vector3 pos = GetRandomPositionOnRectSide(leftArea);

            // 스폰된 위치 → 플레이어 방향
            Vector3 dir = (left.position - pos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            SpawnBullet(pos, rot, leftArea);
        }

        public void Pattern_DefalutInRight()
        {
            Vector3 pos = GetRandomPositionOnRectSide(rightArea);

            // 스폰된 위치 → 플레이어 방향
            Vector3 dir = (right.position - pos).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            Quaternion rot = Quaternion.Euler(0, 0, angle);

            SpawnBullet(pos, rot, rightArea);
        }
        
        public void Pattern_Circle(Vector3 center, RectTransform area, int bulletCount = 12, float radius = 0.1f)
        {
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = (360f / bulletCount) * i;
                Quaternion rot = Quaternion.Euler(0, 0, angle);

                // 각도 기반 방향
                Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f);

                // 총알 시작 위치
                Vector3 pos = center + dir * radius;

                // 해당 패널 area로 귀속 확정
                SpawnBullet(pos, rot, area);
            }
        }

        // 부채꼴 패턴 (Burst)
        public void Pattern_Burst(Vector3 pos, Vector3 target,  RectTransform area, int bulletCount = 5, float spreadAngle = 45f)
        {
            Vector3 dir = (target - pos).normalized;
            float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            for (int i = 0; i < bulletCount; i++)
            {
                float offset = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, (float)i / (bulletCount - 1));
                Quaternion rot = Quaternion.Euler(0, 0, baseAngle + offset - 90f);

                SpawnBullet(pos, rot, area);
            }
        }

        public void Pattern_Line(RectTransform area, int count = 8)
        {
            for (int i = 0; i < count; i++)
            {
                float t = (float)i / (count - 1);
                float x = Mathf.Lerp(area.rect.xMin, area.rect.xMax, t);
                float y = area.rect.yMax;

                Vector3 localPos = new Vector3(x, y, 0f);
                Vector3 pos = area.TransformPoint(localPos);

                Quaternion rot = Quaternion.Euler(0, 0, -180);

                SpawnBullet(pos, rot, area);
            }
        }
    }
}
