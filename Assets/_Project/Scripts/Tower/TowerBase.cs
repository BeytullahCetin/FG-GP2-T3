using NaughtyAttributes;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

///<summary>
///This script controls theDamage by  finding the first enemy enters into tower range and destroys the enemy
///
/// </summary>

namespace FG_GP2_T3
{
    public class TowerBase : MonoBehaviour
    {
        [Expandable]public TowerData Data;

        public LayerMask EnemyLayer;

        private Transform _CurrentTarget;
        private float _FireCooldown;

        private void Update()
        {
            _FireCooldown-=Time.deltaTime;

            if (_CurrentTarget == null || !TargetInRange(_CurrentTarget))
            {
                _CurrentTarget = FindFirstEnemyInRange();
            }

            if (_CurrentTarget != null && _FireCooldown <= 0) 
            {
                Fire();
                _FireCooldown = 1f;
            }
        }

        private Transform FindFirstEnemyInRange()
        {
            Collider[] Hits = Physics.OverlapSphere(transform.position, Data.Range, EnemyLayer);

            if (Hits.Length == 0)
            {
                return null;
            }
            return Hits[0].transform;
        }

        private bool TargetInRange(Transform currentTarget)
        {
            return Vector3.Distance(transform.position, currentTarget.position) <= Data.Range;
        }
        void Fire()
        {
            if (Data.ProjectilePrefab == null) return;

            GameObject ProjObj=Instantiate(Data.ProjectilePrefab,transform.position,Quaternion.identity);
            Projectile Proj=ProjObj.GetComponent<Projectile>();
            Proj.Initiliaze(_CurrentTarget, Data.Damage);
        }

        private void OnDrawGizmosSelected()
        {
            if(Data==null) return;

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, Data.Range);
        }
    }
}
