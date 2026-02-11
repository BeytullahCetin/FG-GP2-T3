using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EPOOutline;
using FMODUnity;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using Handles = UnityEditor.Handles;
#endif

///<summary>
///This script handles the base functionality of a tower in the game, including targeting enemies,sending the info to attack behavior classes.
///
///
/// </summary>

namespace FG_GP2_T3
{
    public class TowerBase : MonoBehaviour
    {
        [Expandable] public TowerData Data;
        [SerializeField] Outlinable outlinable;
        public Outlinable Outlinable => outlinable;

        [ReadOnly]
        [SerializeField] private TowerStats stats;

        public TowerStats Stats => stats;
        public TowerRangePreview TowerRangePreview => towerRangePreview;
        public LayerMask EnemyLayer;

        private StudioEventEmitter _attackSoundEmitter;
        private TowerRangePreview towerRangePreview;

        private List<Transform> _CurrentTargets = new List<Transform>();
        private Transform _CurrentTarget;
        private float _FireCooldown;
        [SerializeField] private float RotationSpeed;

        private float _TargetTimer;
        private TowerVisual _CurrentVisual;


        [SerializeField] Transform towerVisualsParent;
        [ReadOnly][SerializeField] private List<TowerVisual> towerVisuals = new List<TowerVisual>();
        [SerializeField] List<MeshRenderer> meshRenderers = new List<MeshRenderer>();




        private TowerAttack _CurrentAttack;
        public bool HasFused { get; private set; }


        [Header("Fusion Visual Parts")]
        public List<MeshRenderer> Baseparts;
        public List<MeshRenderer> FusionParts;
        [InfoBox("Fusion parts = materials of leaves,Head etc..")]
        [InfoBox("Base parts = materials of body and weapon")]
        public bool ImNothing;

        public TowerData FusedTower;

        private void Awake()
        {
            GetVisualReferances();
            towerRangePreview = GetComponent<TowerRangePreview>();
        }

        void GetVisualReferances()
        {
            towerVisuals = towerVisualsParent.GetComponentsInChildren<TowerVisual>(true).ToList();
        }

        [Button]
        public void UpdateTowerVisual()
        {
            foreach (TowerVisual visual in towerVisuals)
            {
                bool setActive = Data == visual.TowerData;
                visual.gameObject.SetActive(setActive);

                if (setActive)
                {
                    _CurrentVisual = visual;
                    _attackSoundEmitter = visual.GetComponent<StudioEventEmitter>();
                }
            }
        }

        [Button]
        public void BuildTower()
        {
            stats = new TowerStats(Data);
            Debug.Log($"{Data.TowerName} -> Slow:{stats.HasSlow} Stun:{stats.HasStun} Dot:{stats.HasDot}");

            SetMaterials();
            SetupAttackBehaviour();
            PlayBuildSound();

            _CurrentVisual?.PlayIdle();
        }

        public void SetMaterials()
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.sharedMaterial = Data.towerMaterial;
            }
        }

        public void PlayBuildSound()
        {
            // GlobalSoundManager.Instance.OnPlaySound(Data.SoundOnPlaced);
        }

        public void RotateTowardsTarget()
        {
            if (_CurrentTargets == null || _CurrentTargets.Count == 0 || _CurrentVisual == null) return;

            Transform target = _CurrentTargets[0];
            if (target == null) return;

            Vector3 Direction = target.position - towerVisualsParent.position;
            Direction.y = 0f;

            if (Direction.sqrMagnitude < 0.001f) return;

            Quaternion LookRot = Quaternion.LookRotation(Direction);

            Quaternion OffsetRot = Quaternion.Euler(_CurrentVisual.TowerData.RotationOffset);

            towerVisualsParent.localRotation = Quaternion.Slerp(towerVisualsParent.rotation, LookRot * OffsetRot, RotationSpeed * Time.deltaTime);

        }
        private void Update()
        {
            _TargetTimer -= Time.deltaTime;
            if (_TargetTimer <= 0f)
            {
                UpdateTarget();
                _TargetTimer = 0.2f;
            }
            HandleTarget();
            RotateTowardsTarget();

        }


        private void HandleTarget()
        {
            if (_CurrentTargets.Count == 0 || _CurrentAttack == null || Data == null || TargetInRange(_CurrentTarget)) return;

            _FireCooldown -= Time.deltaTime;

            if (_FireCooldown <= 0f)
            {
                _CurrentVisual?.PlayAttack();
                if (_attackSoundEmitter)
                    _attackSoundEmitter.Play();

                if (Data.EnemyTargetting == TargetType.Multiple)
                {
                    _CurrentAttack.Attack(_CurrentTargets);

                }
                else
                {
                    _CurrentAttack.Attack(_CurrentTargets[0]);
                }
                _FireCooldown = 1f / Mathf.Max(0.01f, Stats.FireRate);
            }
        }


        public void MarkFused() => HasFused = true;
        private void UpdateTarget() => _CurrentTargets = SelectTargets();



        private bool TargetInRange(Transform currentTarget)
        {
            if (currentTarget == null) return false;

            return Vector3.Distance(transform.position, currentTarget.position) <= stats.Range;
        }

        private List<Transform> SelectTargets()
        {
            Collider[] Hits = Physics.OverlapSphere(transform.position, stats.Range, EnemyLayer);

            List<Transform> Targets = new List<Transform>();

            if (Hits.Length == 0) return Targets;

            switch (Data.EnemyTargetting)
            {
                case TargetType.First:
                    Targets.Add(Hits[0].transform);
                    break;
                case TargetType.Nearest:
                    Targets.Add(GetnearestEnemy(Hits));
                    break;
                case TargetType.Multiple:
                    Targets = GetmultipleEnemies(Hits);
                    break;
            }
            return Targets;
        }

        private List<Transform> GetmultipleEnemies(Collider[] hits)
        {
            List<Transform> enemies = new List<Transform>();

            foreach (Collider enemy in hits)
            {
                enemies.Add(enemy.transform);
            }
            //sort by nearest
            enemies.Sort((a, b) => Vector3.Distance(transform.position, a.position).CompareTo(Vector3.Distance(transform.position, b.position)));

            if (enemies.Count > Data.MaxTargets)
            {
                enemies = enemies.GetRange(0, Data.MaxTargets);
            }

            return enemies;
        }

        private Transform GetnearestEnemy(Collider[] hits)
        {
            Transform nearest = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider enemy in hits)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy.transform;
                }
            }
            return nearest;
        }



        private void SetupAttackBehaviour()
        {
            if (_CurrentAttack != null)
            {
                Destroy(_CurrentAttack);
            }
            _CurrentAttack = TowerAttackFactory.CreateAttack(this);

            if (_CurrentAttack != null)
                _CurrentAttack.Initialize(this);
        }
        public void UpdateTowerData(TowerData newdata)
        {
            Data = newdata;
            BuildTower();
        }

        /// <summary>
        /// Just for visualizing the tower's attack range in the editor
        /// </summary>
        #region
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (Data == null) return;

            if (Data.AttackType == AttackType.Cone)
            {
                Vector3 forward = transform.forward * stats.Range;
                Quaternion leftRayRotation = Quaternion.AngleAxis(-Data.ConeAngle / 2f, Vector3.up);
                Quaternion rightRayRotation = Quaternion.AngleAxis(Data.ConeAngle / 2f, Vector3.up);
                Vector3 leftRayDirection = leftRayRotation * forward;
                Vector3 rightRayDirection = rightRayRotation * forward;
                // draw cone arc and edges
                Handles.color = Color.yellow;
                Handles.DrawSolidArc(transform.position, Vector3.up, leftRayDirection.normalized, Data.ConeAngle, stats.Range);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + leftRayDirection);
                Gizmos.DrawLine(transform.position, transform.position + rightRayDirection);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, stats.Range);
            }
        }
#endif
        #endregion///
    }
}
