using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class StunProjectileAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            if(target == null) return;

            IDamageable enemy = target.GetComponentInParent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(Tower.Stats.Damage);
                ApplyEffects(enemy);
            }
            Debug.Log($"<color=red> {Tower.name}+{Tower.Stats.Damage} given to {target.name} - Exit()</color>");


        }

        
    }
}
