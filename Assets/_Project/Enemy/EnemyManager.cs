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
        public static EnemyManager Instance;
        public POE Poe => _poe;

        [SerializeField] private POE _poe;

        [SerializeField] private Enemy _enemyPrefab;
        [SerializeField] private Transform _enemiesParent;

        [Header("Settings")]
        [SerializeField] private float _baseEnemySpawnCount = 6;
        [SerializeField] private float _enemySpawnCountMultiplier = 1.2f;

        List<Enemy> enemies = new List<Enemy>();
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
            List<Vector3> spawnPoints = HexManager.Instance.GetEnemyEntryPoints();
            
            //int enemySpawnCount = GetEnemyCount(wave);

            StartCoroutine(SpawnEnemiesRoutine(spawnPoints));
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
            Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, _enemiesParent);
            enemy.Health.OnDead += () => RemoveFromEnemiesList(enemy);
            enemies.Add(enemy);
        }

        public void RemoveFromEnemiesList(Enemy enemy)
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
