using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// this factory class is responsible for creating appropriate TowerAttack instances based on the TowerData configuration.
    /// </summary>
    public static class TowerAttackFactory

    {
        public static TowerAttack CreateAttack(TowerBase tower)
        {
            TowerAttack attack = tower.Data.AttackType switch
            {
                AttackType.Projectile => tower.gameObject.AddComponent<ProjectileAttack>(),
                AttackType.Cone => tower.gameObject.AddComponent<ConeAttack>(),
                AttackType.Melee => tower.gameObject.AddComponent<MeleeAttack>(),
                AttackType.Radius => tower.gameObject.AddComponent<RadiusAttack>(),
                AttackType.Mortar => tower.gameObject.AddComponent<MortarAttack>(),
                _ => null
            };

            attack?.Initialize(tower);
            return attack;
        }

    }
}
