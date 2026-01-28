using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class EnemyManager : MonoBehaviour
    {
        public static EnemyManager Instance;
        public POE Poe => _poe;

        [SerializeField] private POE _poe;

        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemiesParent;

        [Header("Settings")]
        [SerializeField] private float _baseEnemySpawnCount = 6;
        [SerializeField] private float _enemySpawnCountMultiplier = 1.2f;

        private void Awake()
        {
            Instance = this;
            _enemiesParent = transform;
        }

        [Button]
        public void SpawnEnemiesTest()
        {
            SpawnEnemies(1);
        }

        public void SpawnEnemies(int wave)
        {
            List<Vector3> spawnPoints = HexManager.Instance.GetEnemyEntryPoints();

            int spawnPointsCount = spawnPoints.Count;
            int enemySpawnCount = GetEnemyCount(wave);

            // TODO: Update after alpha, 
            // split enemySpawnCount to all spawnPoints
            foreach (Vector3 spawnPoint in spawnPoints)
            {
                for (int i = 0; i < enemySpawnCount; i++)
                {
                    Enemy enemy = Instantiate(_enemyPrefab, spawnPoint, Quaternion.identity, _enemiesParent);
                }
            }
        }

        public int GetEnemyCount(int wave)
        {
            if (wave <= 0)
                return 0;

            return Mathf.RoundToInt(_baseEnemySpawnCount * Mathf.Pow(_enemySpawnCountMultiplier, wave));
        }
    }
}
