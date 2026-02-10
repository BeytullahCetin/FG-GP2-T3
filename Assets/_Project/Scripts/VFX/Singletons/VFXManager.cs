using UnityEngine;

namespace FG_GP2_T3
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance { get; private set; }

        [SerializeField] private GameObject _tilePlacedPrefab;
        [SerializeField] private GameObject _towerFusedPrefab;
        [SerializeField] private GameObject _enemySpawnPrefab;
        [SerializeField] private GameObject _enemyDamagedPrefab;
        [SerializeField] private GameObject _enemyDeathPrefab;
        [SerializeField] private GameObject _towerSlowAOEPrefab;
        [SerializeField] private GameObject _towerDOTAOEPrefab;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            EventManager.Register<OnCellEvent>(OnCellEvent);
            EventManager.Register<OnTowerEvent>(OnTowerEvent);
            EventManager.Register<OnTowerActionEvent>(OnTowerActionEvent);
            EventManager.Register<OnEnemyActionEvent>(OnEnemyActionEvent);
        }

        private void Update()
        {
        
        }

        private void OnCellEvent(OnCellEvent args)
        {
            if (args.EventType == CellEventType.Place)
                SpawnVFX(_tilePlacedPrefab, args.Cell.transform.position + Vector3.up * 0.1f, args.Cell.transform);
        }

        private void OnTowerEvent(OnTowerEvent args)
        {
            if(args.EventType == TowerEventType.Fuse)
                SpawnVFX(_towerFusedPrefab, args.Tower.transform.position + Vector3.up * 0.1f, true, args.Tower.transform);
        }

        private void OnTowerActionEvent(OnTowerActionEvent args)
        {
            
        }

        private void OnEnemyActionEvent(OnEnemyActionEvent args)
        {
            switch(args.EventType)
            {
                case EnemyEventType.Spawn:
                    SpawnVFX(_enemySpawnPrefab, args.Enemy.transform.position + Vector3.up * 0.1f, args.Enemy.transform);
                    return;
                case EnemyEventType.Damaged:
                    SpawnVFX(_enemyDamagedPrefab, args.Enemy.transform.position + Vector3.up * 0.1f, args.Enemy.transform);
                    return;
                case EnemyEventType.Death:
                    SpawnVFX(_enemyDeathPrefab, args.Enemy.transform.position + Vector3.up * 0.1f);
                    return;
            }
        }

        private void SpawnVFX(GameObject prefab, Vector3 position, bool loop = false, Transform parent = null)
        {
            if (prefab == null) return;

            Transform instance = ComponentFactory.Spawn(prefab.transform, position, Quaternion.identity, parent);
            
            ParticleSystem rootPS = instance.GetComponent<ParticleSystem>();
            
            if (rootPS == null)
                rootPS = instance.GetComponentInChildren<ParticleSystem>();

            if (rootPS != null)
            {
                rootPS.Play(true); 

                bool _isLooping = rootPS.main.loop;
                
                if (!loop && !_isLooping)
                    StartCoroutine(WaitForVFXEnd(prefab.transform, instance, rootPS));
            }
        }

        private System.Collections.IEnumerator WaitForVFXEnd(Transform prefab, Transform instance, ParticleSystem rootPS)
        {
            yield return new WaitForSeconds(0.5f);

            while (rootPS != null && rootPS.IsAlive(true))
                yield return new WaitForSeconds(0.5f);

            ComponentFactory.Despawn(prefab, instance);
        }
    }
}
