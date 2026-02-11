using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class MeleeAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            if (target == null) return;

            IDamageable enemy = GetEnemy(target);
            if (enemy == null) return;

            enemy.TakeDamage(Tower.Stats.Damage);
            // Debug.Log($"<color=red> {Tower.name}+{Tower.Stats.Damage} given to {target.name} - Exit()</color>");
        }

       
    }
}
