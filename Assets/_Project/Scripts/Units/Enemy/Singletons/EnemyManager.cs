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
        private int _waveIndex = 0;

        private POE _target;
        public POE GetTarget() => _target;

        private int _enemiesRemaining = 0;
        private int _activeSpawningCoroutines = 0;
        private int _enemyCount = 0;
        private bool _isWaveActive = false;

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

        private void Update()
        {
            if (!_isWaveActive) return;
            if (_activeSpawningCoroutines > 0) return;
            if (_enemiesRemaining > 0) return;

            FinishWave();
        }

        private void FinishWave()
        {           
            _isWaveActive = false;
            _enemiesRemaining = 0;

            GameManager.Instance.SwitchToTileSelectionSubState();
            EventManager.Invoke(new OnWaveEvent(WaveEventType.End, _waveIndex));
            
            _waveIndex++;
        }

        public void StartWave()
        {
            if (_waveIndex >= _waves.Count) return;
            if (_isWaveActive) return;

            _isWaveActive = true;
            EventManager.Invoke(new OnWaveEvent(WaveEventType.Start, _waveIndex));

            foreach (EnemyGroup group in _waves[_waveIndex].Groups)
            {
                _activeSpawningCoroutines++;
                _enemiesRemaining += group.Count;
                StartCoroutine(SpawnGroupCoroutine(group));
            }
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

            _activeSpawningCoroutines--;
        }

        private void SpawnEnemy(GameObject prefab)
        {
            List<Vector3> path = HexManager.Instance.GetNextEnemyPath();

            GameObject enemyObject = Instantiate(prefab, path[0], Quaternion.identity);
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            enemy.Initialize(path);
        }

        public void RegisterEnemy()
        {
            EventManager.Invoke(new OnWaveEvent(WaveEventType.EnemyCountChanged, _waveIndex, _enemyCount));
            _enemyCount++;
        }

        public void UnregisterEnemy()
        {
            EventManager.Invoke(new OnWaveEvent(WaveEventType.EnemyCountChanged, _waveIndex, _enemyCount));
            _enemyCount--;
            _enemiesRemaining--;
        }
    }
}