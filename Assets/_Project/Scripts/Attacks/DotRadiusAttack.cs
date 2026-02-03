using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class DotRadiusAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            Collider[] Hits = Physics.OverlapSphere(transform.position, Tower.Data.Range, Tower.EnemyLayer);

            foreach (Collider hit in Hits)
            {
                IDamageable enemy = hit.GetComponent<IDamageable>();
                if (enemy != null)
                    enemy.ApplyDot(Tower.Data.DotDamagePerSecond, Tower.Data.DotDuration);
            }
        }

        
    }
}
