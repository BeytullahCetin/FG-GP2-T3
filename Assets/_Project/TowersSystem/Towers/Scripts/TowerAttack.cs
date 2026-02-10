using System.Collections.Generic;
using UnityEngine;

///<summary>
///this script is the abstract base class for all tower attack behaviors
///</summary>
namespace FG_GP2_T3
{
    public abstract class TowerAttack : MonoBehaviour
    {
        protected TowerBase Tower;

        public virtual void Initialize(TowerBase towerBase)
        {
            Tower = towerBase;
        }
        public abstract void Attack(Transform target);

        public virtual void Attack(List<Transform> targets)
        {
            foreach(Transform target in targets)
            {
                Attack(target);
            }
        }
        public void ApplyEffects(IDamageable enemy)
        {
            if (enemy == null) return;

            if (Tower.Stats.HasStun)
            {
                enemy.ApplyStun(Tower.Stats.CCDuraton);
            }

            if (Tower.Stats.HasDot)
            {
                enemy.ApplyDot(
                    Tower.Stats.DotDamage,
                    Tower.Stats.DotDuration
                );
            }

            if (Tower.Stats.HasSlow)
            {
                enemy.ApplySlow(Tower.Stats.Slowpercent,Tower.Stats.CCDuraton);
            }
        }
        protected IDamageable GetEnemy(Transform Target)
        {
            return Target.GetComponent<IDamageable>();
        }
    }
}
