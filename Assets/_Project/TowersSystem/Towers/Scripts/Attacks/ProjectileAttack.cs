using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class ProjectileAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            if (target == null) return;

          

            GameObject proj=GameObject.Instantiate(Tower.Data.ProjectilePrefab, Tower.transform.position, Quaternion.identity);

            Projectile projectile = proj.GetComponent<Projectile>();

            if(projectile != null)
            {
                projectile.Initialize(target, Tower.Stats.Damage);
            }
           
        }

    }
}
