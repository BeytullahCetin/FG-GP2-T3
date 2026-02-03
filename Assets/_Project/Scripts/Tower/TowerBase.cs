using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

        public LayerMask EnemyLayer;

        [SerializeField] private StudioEventEmitter attackSoundEmitter;
        
        private  List<Transform> _CurrentTargets= new List<Transform>();
        private Transform _CurrentTarget;
        private float _FireCooldown;
        [SerializeField] List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

        private TowerAttack _CurrentAttack;
        private void Start()
        {

            BuildTower();
        }

        private void BuildTower()
        {
            SetMaterials();
            SetupAttackBehaviour();
            PlayBuildSound();
        }

        public void SetMaterials()
        {
            foreach (var renderer in meshRenderers)
            {
                renderer.material = Data.towerMaterial;
            }
        }

        public void PlayBuildSound()
        {
            GlobalSoundManager.Instance.OnPlaySound(Data.SoundOnPlaced);
        }

        private void Update()
        {
            UpdateTarget();
            HandleTarget();
        }

        private void HandleTarget()
        {
            if (_CurrentTargets.Count==0 || _CurrentAttack ==null || Data==null) return;

            _FireCooldown -= Time.deltaTime;

            if (_FireCooldown <= 0f)
            {
                if (attackSoundEmitter)
                    attackSoundEmitter.Play();
                
                if(Data.EnemyTargetting==TargetType.Multiple)
                {
                    _CurrentAttack.Attack(_CurrentTargets);
                   
                }
                else
                {
                    _CurrentAttack.Attack(_CurrentTargets[0]);
                }
                _FireCooldown=1f/ Data.FireRate;
            }
        }

        

        private void UpdateTarget() => _CurrentTargets = SelectTargets();



        private bool TargetInRange(Transform currentTarget)
        {
            if(currentTarget==null) return false;

            return Vector3.Distance(transform.position, currentTarget.position) <= Data.Range;
        }

        private List<Transform> SelectTargets()
        {
            Collider[] Hits=Physics.OverlapSphere(transform.position,Data.Range,EnemyLayer);

            List<Transform> Targets= new List<Transform>();

            if (Hits.Length==0) return Targets;

            switch (Data.EnemyTargetting)
            {
                case TargetType.First:
                    Targets.Add(Hits[0].transform);
                    break;
                case TargetType.Nearest:
                    Targets.Add(GetnearestEnemy(Hits));
                    break;
                case TargetType.Multiple:
                    Targets=GetmultipleEnemies(Hits);
                    break;
            }
            return Targets;
        }

        private List<Transform> GetmultipleEnemies(Collider[] hits)
        {
           List<Transform> enemies= new List<Transform>();

            foreach(Collider enemy in hits)
            {
                enemies.Add(enemy.transform);
            }
            //sort by nearest
            enemies.Sort((a,b)=>Vector3.Distance(transform.position,a.position).CompareTo(Vector3.Distance(transform.position,b.position)));

            if(enemies.Count>Data.MaxTargets)
            {
                enemies=enemies.GetRange(0,Data.MaxTargets);
            }

            return enemies;
        }

        private Transform GetnearestEnemy(Collider[] hits)
        {
            Transform nearest = null;
            float minDistance = Mathf.Infinity;

            foreach(Collider enemy in hits)
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
           if(_CurrentAttack != null) 
            {
                Destroy(_CurrentAttack);
            }
            _CurrentAttack=TowerAttackFactory.CreateAttack(this);

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
                Gizmos.DrawSphere(transform.position, Data.Range);
            }
        }
        #endregion///
    }
}
