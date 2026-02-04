using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace FG_GP2_T3
{
    public class EnemyManager : MonoBehaviour
    {
        [Serializable]
        public class EnemyGroup
        {
            public GameObject EnemyPrefab;
            public int Count;
            public float SpawnRate;
            public float StartDelay;
        }

        [Serializable]
        public class EnemyWave
        {
            public List<EnemyGroup> Groups;
        }

        public static EnemyManager Instance;

        [SerializeField] private List<EnemyWave> _waves;

        private POE _target;
        public POE GetTarget() => _target;

        private void Awake()
        { 
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start() => _target = FindFirstObjectByType<POE>();

        public void StartWave(int index)
        {
            if (index >= _waves.Count) return;

            foreach (EnemyGroup group in _waves[index].Groups)
                StartCoroutine(SpawnGroupCoroutine(group));
        }

        private IEnumerator SpawnGroupCoroutine(EnemyGroup group)
        {
            yield return new WaitForSeconds(group.StartDelay);

            float interval = 1f / group.SpawnRate;

            for (int i = 0; i < group.Count; i++)
            {
                SpawnEnemy(group.EnemyPrefab);
                yield return new WaitForSeconds(interval);
            }
        }

        private void SpawnEnemy(GameObject prefab)
        {
            List<Vector3> path = HexManager.Instance.GetNextEnemyPath();
            
            GameObject enemyObject = Instantiate(prefab, path[0], Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.Initialize(path);
        }
    }
}