using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class SlowRadiusAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            Collider[] Hits= Physics.OverlapSphere(transform.position, Tower.Data.Range,Tower.EnemyLayer);

            foreach(Collider hit in Hits)
            {
                IDamegable enemy= hit.GetComponent<IDamegable>();
                if (enemy==null) continue;

                enemy.ApplySlow(Tower.Data.SlowPercent, Tower.Data.CcDuration);
            }
        }

    }
}
