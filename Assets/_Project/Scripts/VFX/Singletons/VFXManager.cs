using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class VFXManager : MonoBehaviour
    {
        public static VFXManager Instance { get; private set; }

        private ComponentFactory _factory;

        [SerializeField] private GameObject _tilePlacedPrefab;
        [SerializeField] private GameObject _towerFusedPrefab;
        [SerializeField] private GameObject _enemySpawnPrefab;
        [SerializeField] private GameObject _enemyDamagedPrefab;
        [SerializeField] private GameObject _enemyDeathPrefab;
        [SerializeField] private GameObject _towerSlowAOEPrefab;
        [SerializeField] private GameObject _towerDOTAOEPrefab;

        HashSet<(TowerBase, ParticleSystem)> _slowTowerVFXs = new();
        HashSet<(TowerBase, ParticleSystem)> _dotTowerVFXs = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            GameObject _root = new GameObject("--- VFX_POOL_ROOT ---");
            _root.transform.SetParent(transform);
            _factory = new ComponentFactory(_root.transform);
        }

        private void Start()
        {
            EventManager.Register<OnCellEvent>(OnCellEvent);
            EventManager.Register<OnTowerEvent>(OnTowerEvent);
            EventManager.Register<OnEnemyActionEvent>(OnEnemyActionEvent);
            EventManager.Register<OnWaveEvent>(OnWaveEvent);
            EventManager.Register<OnBranchLostEvent>(OnBranchLostEvent);
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
            switch(args.EventType)
            {
                case TowerEventType.Build:
                    if(args.Tower.Data.CrowdControlType == CrowdControlType.Slow)
                    {
                        ParticleSystem vfx = SpawnVFX(_towerSlowAOEPrefab, args.Tower.transform.position + Vector3.up * 0.1f, true, args.Tower.transform);
                        vfx.gameObject.SetActive(false);
                        _slowTowerVFXs.Add((args.Tower, vfx));
                    }
                    else if(args.Tower.Data.DotDamagePerSecond > 0f)
                    {  
                        ParticleSystem vfx = SpawnVFX(_towerDOTAOEPrefab, args.Tower.transform.position + Vector3.up * 0.1f, true, args.Tower.transform);
                        vfx.gameObject.SetActive(false);
                        _dotTowerVFXs.Add((args.Tower, vfx));
                    }
                    return;
                case TowerEventType.Fuse:
                    SpawnVFX(_towerFusedPrefab, args.Tower.transform.position + Vector3.up * 0.1f, true, args.Tower.transform);
                    return;
            }
        }

        private void OnWaveEvent(OnWaveEvent args)
        {
            switch(args.EventType)
            {
                case WaveEventType.Start:
                    foreach(var slowTowerVFX in _slowTowerVFXs)
                        slowTowerVFX.Item2.gameObject.SetActive(true);
                    foreach(var dotTowerVFX in _dotTowerVFXs)
                        dotTowerVFX.Item2.gameObject.SetActive(true);
                    return;
                case WaveEventType.End:
                    foreach(var slowTowerVFX in _slowTowerVFXs)
                        slowTowerVFX.Item2.gameObject.SetActive(false);
                    foreach(var dotTowerVFX in _dotTowerVFXs)
                        dotTowerVFX.Item2.gameObject.SetActive(false);
                    return;
            }
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

        private void OnBranchLostEvent(OnBranchLostEvent args)
        {
            GameObject branch = args.Branch;
            Vector3 branchPos = branch.transform.position;
            //TODO: The actual vfx
            
            branch.SetActive(false); // Maybe this should be done elsewhere
        }

        private ParticleSystem SpawnVFX(GameObject prefab, Vector3 position, bool loop = false, Transform parent = null)
        {
            if (prefab == null) return null;

            Transform instance = _factory.Spawn(prefab.transform, position, prefab.transform.rotation, parent);
            
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

            return rootPS;
        }

        private System.Collections.IEnumerator WaitForVFXEnd(Transform prefab, Transform instance, ParticleSystem rootPS)
        {
            yield return new WaitForSeconds(0.5f);

            while (rootPS != null && rootPS.IsAlive(true))
                yield return new WaitForSeconds(0.5f);

            _factory.Despawn(prefab, instance);
        }
    }
}
