using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class MortarAttack : TowerAttack
    {
        public override void Attack(Transform target)
        {
            if(target == null) return;

            GameObject Proj=Instantiate(Tower.Data.ProjectilePrefab, Tower.transform.position, Quaternion.identity);

          
            MotarProjectile motarProjectile = Proj.GetComponent<MotarProjectile>();

            if(motarProjectile !=null)
            {
                motarProjectile.Initialize(target, Tower.Data.Damage, Tower.Data.ExplosionRadius);
            }



        }

        
    }
}
