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

        public bool HasSlow;
        public float Slowpercent;
        public float CCDuraton;

        public bool HasSplash;
        public float Explosionradius;
        
        public bool HasDot;
        public bool HasStun;
        public float DotDuration;
        public float DotDamage;

        public TowerStats(TowerData data)
        {
            Range = data.Range;
            FireRate = data.FireRate;
            Damage = data.Damage;

            HasSlow=data.CrowdControlType == CrowdControlType.Slow;
            HasStun=data.CrowdControlType == CrowdControlType.Stun;

            Slowpercent = data.SlowPercent;
            CCDuraton = data.CcDuration;
            
            HasSplash=data.AttackType == AttackType.Mortar ;
            Explosionradius = data.ExplosionRadius;


            HasDot = data.DamageType == DamageType.DamageOverTime;

            DotDuration = data.DotDuration;
            DotDamage = data.DotDamagePerSecond;

        }
    }
}
