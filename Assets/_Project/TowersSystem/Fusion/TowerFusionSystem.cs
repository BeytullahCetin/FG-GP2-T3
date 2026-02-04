using System;
using UnityEngine;

namespace FG_GP2_T3
{
    public static class TowerFusionSystem
    {
        public static bool TryFuse(TowerBase basetower, TowerBase sacrificetower)
        {
            if (basetower == null || sacrificetower == null) return false;

            if ((basetower.HasFused || sacrificetower.HasFused))
            {
                Debug.Log("Fusion is bloced towers has already been fused.");
                return false;

            }
            string BaseID = basetower.Data.FusionID;
            string SacrificeID = sacrificetower.Data.FusionID;

            if (!FusionRegistry.CanFuse(BaseID, SacrificeID))
            {
                Debug.Log("Fusion not allowed between these towers.");
                return false;
            }

            ApplyFusionStat(basetower, sacrificetower);
            ApplyFusionMaterials(basetower, sacrificetower);

            FusionRegistry.MarkUsed(BaseID, SacrificeID);

            basetower.MarkFused();
            sacrificetower.MarkFused();

            GameObject.Destroy(sacrificetower.gameObject);

            Debug.Log($"Fusion: BASE={basetower.Data.FusionID} " +
                      $"SACRIFICE={sacrificetower.Data.FusionID}"
);
            return true;
        }

        private static void ApplyFusionStat(TowerBase tower, TowerBase sacrifice)
        {
            switch (sacrifice.Data.FusionStatType)
            {
                case FusionStatType.None:
                    break;
                case FusionStatType.RangeBoost:
                    tower.Stats.Range += sacrifice.Data.FusionStatValue;
                    break;
                case FusionStatType.DamageBoost:
                    tower.Stats.Damage += sacrifice.Data.FusionStatValue;
                    break;
                case FusionStatType.FireRateBoost:
                    tower.Stats.FireRate += sacrifice.Data.FusionStatValue;
                    break;
                case FusionStatType.SlowEffectBoost:
                    tower.Stats.Slowpercent += sacrifice.Data.FusionStatValue;
                    break;
                case FusionStatType.StunDurationBoost:
                    tower.Stats.CCDuraton += sacrifice.Data.FusionStatValue;
                    break;
                case FusionStatType.DotDurationBoost:
                    tower.Stats.DotDuration += sacrifice.Data.FusionStatValue;
                    break;
                case FusionStatType.SplashRadiusBoost:
                    tower.Stats.Explosionradius += sacrifice.Data.FusionStatValue;
                    break;

            }
            Debug.Log($"Applied fusion stat {sacrifice.Data.FusionStatType} with value {sacrifice.Data.FusionStatValue} to tower {tower.Data.TowerName}.");
        }

        private static void ApplyFusionMaterials(TowerBase tower, TowerBase sacrifice)
        {
            Material fusionMaterial = sacrifice.Data.towerMaterial;

            foreach (MeshRenderer part in tower.FusionParts)

            {
                part.material = fusionMaterial;

            }
            Debug.Log($"Applied fusion material from sacrifice tower {sacrifice.Data.TowerName} to base tower {tower.Data.TowerName}.");
        }
    }
}
