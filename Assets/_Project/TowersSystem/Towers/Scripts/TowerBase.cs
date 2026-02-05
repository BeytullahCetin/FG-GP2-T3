using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;


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

        [ReadOnly]
        [SerializeField] private TowerStats stats;

        public TowerStats Stats => stats;

        public LayerMask EnemyLayer;

        [SerializeField] private StudioEventEmitter attackSoundEmitter;

        private List<Transform> _CurrentTargets = new List<Transform>();
        private Transform _CurrentTarget;
        private float _FireCooldown;

        private float _TargetTimer;



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

        private void Awake()
        {
            GetVisualReferances();
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
                visual.gameObject.SetActive(Data == visual.TowerData);
            }
        }

        [Button]
        public void BuildTower()
        {
            stats = new TowerStats(Data);
            SetMaterials();
            SetupAttackBehaviour();
            PlayBuildSound();
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

        private void Update()
        {
            _TargetTimer -= Time.deltaTime;
            if (_TargetTimer <= 0f)
            {
                UpdateTarget();
                _TargetTimer = 0.2f;
            }
            HandleTarget();
        }


        private void HandleTarget()
        {
            if (_CurrentTargets.Count == 0 || _CurrentAttack == null || Data == null) return;

            _FireCooldown -= Time.deltaTime;

            if (_FireCooldown <= 0f)
            {
                if (attackSoundEmitter)
                    attackSoundEmitter.Play();

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

            return Vector3.Distance(transform.position, currentTarget.position) <= Data.Range;
        }

        private List<Transform> SelectTargets()
        {
            Collider[] Hits = Physics.OverlapSphere(transform.position, Data.Range, EnemyLayer);

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
                Vector3 forward = transform.forward * Data.Range;
                Quaternion leftRayRotation = Quaternion.AngleAxis(-Data.ConeAngle / 2f, Vector3.up);
                Quaternion rightRayRotation = Quaternion.AngleAxis(Data.ConeAngle / 2f, Vector3.up);
                Vector3 leftRayDirection = leftRayRotation * forward;
                Vector3 rightRayDirection = rightRayRotation * forward;
                // draw cone arc and edges
                Handles.color = Color.yellow;
                Handles.DrawSolidArc(transform.position, Vector3.up, leftRayDirection.normalized, Data.ConeAngle, Data.Range);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, transform.position + leftRayDirection);
                Gizmos.DrawLine(transform.position, transform.position + rightRayDirection);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, Data.Range);
            }
        }
#endif
        #endregion///
    }
}
