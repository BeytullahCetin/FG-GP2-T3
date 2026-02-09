using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class RadiusAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            Collider[] Hits = Physics.OverlapSphere(transform.position, Tower.Data.Range, Tower.EnemyLayer);

            foreach (Collider hit in Hits)
            {
               IDamageable enemy = hit.GetComponent<IDamageable>();
                if (enemy != null)
                    enemy.TakeDamage(Tower.Stats.Damage);

            }
            Debug.Log($"<color=red> {Tower.name}+{Tower.Stats.Damage} given to {target.name} - Exit()</color>");
        }


    }
}
