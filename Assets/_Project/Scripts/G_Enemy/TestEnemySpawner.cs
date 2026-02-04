using UnityEngine;

namespace FG_GP2_T3
{
    public class TestEnemySpawner : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public GameObject enemyTarget;
        public float spawnTime;
        private float _spawnTimer;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            _spawnTimer += Time.deltaTime;

            if (_spawnTimer < spawnTime)
            {
                return;
            }
            _spawnTimer = 0;
            
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            Test_Enemy castEnemy = enemy.GetComponent<Test_Enemy>();
            castEnemy.target = enemyTarget;
        }
    }
}
