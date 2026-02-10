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
            float value = sacrifice.Data.FusionStatValue;
            switch (sacrifice.Data.FusionStatType)
            {
                case FusionStatType.None:
                    break;
                case FusionStatType.RangeBoost:
                    tower.Stats.Range += value;
                    break;
                case FusionStatType.DamageBoost:
                    tower.Stats.Damage += value;
                    break;
                case FusionStatType.FireRateBoost:
                    tower.Stats.FireRate += value;
                    break;
                case FusionStatType.splashradiusBoost:
                    if(!tower.Stats.HasSplash)
                        tower.Stats.HasSplash = true;

                    tower.Stats.Explosionradius += value;

                    break;
                case FusionStatType.AddSlow:
                    if(!tower.Stats.HasSlow)
                    { 
                        tower.Stats.HasSlow = true;
                       tower.Stats.Slowpercent = value;
                    }
                    else
                    {
                       tower.Stats.Slowpercent += value;
                    }
                        break;
                case FusionStatType.AddStun:
                    if (!tower.Stats.HasStun)
                    {
                        tower.Stats.HasStun = true;
                        tower.Stats.CCDuraton = value;
                    }
                    else
                    {
                        tower.Stats.CCDuraton += value;
                    }
                    break;
                case FusionStatType.AddDot:
                    if (!tower.Stats.HasDot)
                    {
                        tower.Stats.HasDot = true;
                        tower.Stats.DotDamage = value;
                        tower.Stats.DotDuration = value;
                    }
                    else
                    {
                        tower.Stats.DotDamage += value;
                    }
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
