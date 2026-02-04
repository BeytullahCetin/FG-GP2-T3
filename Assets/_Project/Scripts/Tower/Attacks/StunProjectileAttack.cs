using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class StunProjectileAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            if(target == null) return;

            IDamageable enemy = target.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(Tower.Stats.Damage);
                enemy.ApplyStun(Tower.Stats.CCDuraton);
            }



        }

        
    }
}
