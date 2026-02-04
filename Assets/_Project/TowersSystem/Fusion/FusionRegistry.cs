using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public static class FusionRegistry
    {
        private static HashSet<string> UsedFusions = new HashSet<string>();

        private static string key(string id1, string id2)
        {
            return $"{id1}::{id2}";
        }
        public static bool CanFuse(string id1, string id2)
        {
            return !UsedFusions.Contains(key(id1, id2));
        }
        public static void MarkUsed(string id1, string id2)
        {
            UsedFusions.Add(key(id1, id2));
        }

        public static void Reset()
        {
            UsedFusions.Clear();
        }
    }
}