using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class ConeAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            Collider[] Hits = Physics.OverlapSphere(transform.position, Tower.Data.Range, Tower.EnemyLayer);


            foreach (Collider hit in Hits)
            {
                Vector3 dir = (hit.transform.position - Tower.transform.position).normalized;

                float angle = Vector3.Angle(Tower.transform.forward, dir);

                if (angle <= Tower.Data.ConeAngle / 2f)
                {

                    IDamageable enemy = hit.GetComponent<IDamageable>();
                    if (enemy != null)
                        enemy.TakeDamage(Tower.Stats.Damage);
                }
            }
        }
    }
}
