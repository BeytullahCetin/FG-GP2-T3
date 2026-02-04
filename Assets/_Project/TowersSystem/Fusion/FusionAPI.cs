using System;
using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// uI/Drag&Drop accessible API for fusion system.
    /// </summary>
    public static class FusionAPI 
    {
        public static event Action<TowerBase, TowerBase> OnFusionSuccess;
        public static event Action<String> OnFusionFailed;
        public static bool CanFuse(TowerBase basetower, TowerBase sacrificetower)
        {
            if(basetower == null || sacrificetower == null) return false;
            if(basetower.HasFused|| sacrificetower.HasFused) return false;

            return FusionRegistry.CanFuse(basetower.Data.FusionID, sacrificetower.Data.FusionID);

        }

        public static bool TryFuse(TowerBase basetower, TowerBase sacrificetower)
        {
           bool Success = TowerFusionSystem.TryFuse(basetower, sacrificetower);

             if(Success)
              {
                 OnFusionSuccess?.Invoke(basetower, sacrificetower);
              }
              else
              {
                 OnFusionFailed?.Invoke("Fusion Failed: Check if towers are eligible for fusion.");
              }
             return Success;
        }
    }
}
