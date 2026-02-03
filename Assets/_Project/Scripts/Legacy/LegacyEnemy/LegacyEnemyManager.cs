using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

namespace FG_GP2_T3
{
    public class LegacyEnemyManager : MonoBehaviour
    {
        public static LegacyEnemyManager Instance;
        public POE Poe => _poe;

        [SerializeField] private POE _poe;

        [SerializeField] private LegacyEnemy _enemyPrefab;
        [SerializeField] private Transform _enemiesParent;

        [Header("Settings")]
        [SerializeField] private float _baseEnemySpawnCount = 6;
        [SerializeField] private float _enemySpawnCountMultiplier = 1.2f;

        List<LegacyEnemy> enemies = new List<LegacyEnemy>();
        public event Action OnAllEnemiesDead = delegate { };

        private int _turn = 1;

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
            List<Vector3> movementPoints = HexManager.Instance.GetNextEnemyPath();
            
            //int enemySpawnCount = GetEnemyCount(wave);

            StartCoroutine(SpawnEnemiesRoutine(movementPoints));
        }

        private IEnumerator SpawnEnemiesRoutine(List<Vector3> spawnPoints)
        {
            int enemiesToSpawn = Math.Min(_turn * _turn, 64);
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                foreach (Vector3 spawnPoint in spawnPoints)
                    SpawnSingleEnemy(spawnPoint);

                yield return new WaitForSeconds(1f - (enemiesToSpawn / 100f));
            }

            _turn++;
        }

        void SpawnSingleEnemy(Vector3 position)
        {
            LegacyEnemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _enemiesParent);
            enemy.Health.OnDead += () => RemoveFromEnemiesList(enemy);
            enemies.Add(enemy);
        }

        public void RemoveFromEnemiesList(LegacyEnemy enemy)
        {
            enemies.Remove(enemy);

            if (enemies.Count <= 0)
                OnAllEnemiesDead();
        }

        public int GetEnemyCount(int wave)
        {
            if (wave <= 0)
                return 0;

            return Mathf.RoundToInt(_baseEnemySpawnCount * Mathf.Pow(_enemySpawnCountMultiplier, wave - 1));
        }
    }
}
