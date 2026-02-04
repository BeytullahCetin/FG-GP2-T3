using System;
using UnityEngine;

namespace FG_GP2_T3
{
    [Serializable]
    public class TowerStats 
    {
        public float Range;
        public float FireRate;
        public float Damage;

        public float Slowpercent;
        public float CCDuraton;

        public float DotDuration;
        public float DotDamage;
        public float Explosionradius;

        public TowerStats(TowerData data)
        {
            Range = data.Range;
            FireRate = data.FireRate;
            Damage = data.Damage;
            Slowpercent = data.SlowPercent;
            CCDuraton = data.CcDuration;
            DotDuration = data.DotDuration;
            DotDamage = data.DotDamagePerSecond;
            Explosionradius = data.ExplosionRadius;

        }
    }
}
