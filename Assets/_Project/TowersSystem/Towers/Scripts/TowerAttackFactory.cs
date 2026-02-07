using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// this factory class is responsible for creating appropriate TowerAttack instances based on the TowerData configuration.
    /// </summary>
    public static class TowerAttackFactory

    {
        public static TowerAttack CreateAttack(TowerBase towerBase)
        {
            switch (towerBase.Data.AttackType)
            {
                case AttackType.Projectile:
                    if (towerBase.Data.CrowdControlType == CrowdControlType.Stun)
                    {
                        return towerBase.gameObject.AddComponent<StunProjectileAttack>();
                    }
                    return towerBase.gameObject.AddComponent<ProjectileAttack>();
                case AttackType.Cone:
                    return towerBase.gameObject.AddComponent<ConeAttack>();
                case AttackType.Melee:
                    return towerBase.gameObject.AddComponent<MeleeAttack>();
                case AttackType.Radius:
                    if (towerBase.Data.DamageType == DamageType.DamageOverTime)
                    {
                        return towerBase.gameObject.AddComponent<DotRadiusAttack>();
                    }
                    if (towerBase.Data.CrowdControlType == CrowdControlType.Slow)
                    {
                        return towerBase.gameObject.AddComponent<SlowRadiusAttack>();
                    }
                    return towerBase.gameObject.AddComponent<RadiusAttack>();
                case AttackType.Mortar:
                    return towerBase.gameObject.AddComponent<MortarAttack>();

            }
            return null;
        }

    }
}
