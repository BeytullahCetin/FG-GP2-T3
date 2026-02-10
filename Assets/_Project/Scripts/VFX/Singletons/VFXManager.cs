using UnityEngine;

namespace FG_GP2_T3
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance { get; private set; }

        [SerializeField] private ParticleSystem _tilePlacedPrefab;
        [SerializeField] private ParticleSystem _towerFusedPrefab;
        [SerializeField] private ParticleSystem _enemySpawnPrefab;
        [SerializeField] private ParticleSystem _enemyDamagedPrefab;
        [SerializeField] private ParticleSystem _enemyDeathPrefab;
        [SerializeField] private ParticleSystem _towerSlowAOEPrefab;
        [SerializeField] private ParticleSystem _towerDOTAOEPrefab;

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
                SpawnVFX(_towerFusedPrefab, args.Tower.transform.position + Vector3.up * 0.1f, args.Tower.transform);
        }

        private void OnTowerActionEvent(OnTowerActionEvent args)
        {
            
        }

        private void OnEnemyActionEvent(OnEnemyActionEvent args)
        {
            
        }

        private void SpawnVFX(ParticleSystem prefab, Vector3 position, bool loop = false, Transform parent = null)
        {
            ParticleSystem instance = ComponentFactory.Spawn(prefab, position, Quaternion.identity, parent);

            instance.Play();
            
            if (!loop)
                StartCoroutine(WaitForVFXEnd(prefab, instance));
        }

        private System.Collections.IEnumerator WaitForVFXEnd(ParticleSystem prefab, ParticleSystem instance)
        {
            yield return new WaitForSeconds(1f);

            while (instance.IsAlive(true))
                yield return new WaitForSeconds(0.5f);

            ComponentFactory.Despawn(prefab, instance);
        }
    }
}
