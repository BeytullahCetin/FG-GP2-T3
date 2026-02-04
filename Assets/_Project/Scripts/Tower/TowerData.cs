using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// This is the base TowerData class where it consists of Data related to tower like
    /// Range,Role,Cost,Damage types
    /// </summary>

    [CreateAssetMenu(menuName = "Towers/TowerData")]
    public class TowerData : ScriptableObject
    {
        [Header("TowerInfo")]
        public string TowerName;
        public TowerRole Role;

        
        public AttackType AttackType;
        public DamageType DamageType;

        [Header("Crowd Control Stats")]
        public CrowdControlType CrowdControlType;

        [EnableIf(nameof(HasCrowdControl))]
        public float CcDuration;

        [EnableIf(nameof(IsSlow))]
        public float SlowPercent;

        [Header("Targetting")]
        public TargetType EnemyTargetting;

        [EnableIf(nameof(IsMultipleTarget))]
        public int MaxTargets;


        [Header("Explosion/Splash stats")]

        [EnableIf(nameof(UsesExplosion))]
        public float ExplosionRadius;



        [Header("Damage Over Time")]

        [EnableIf(nameof(IsDotDamage))]
        public float  DotDuration;

        [EnableIf(nameof(IsDotDamage))]
        public float DotDamagePerSecond;



        [Header("Cone Stats")]

        [EnableIf(nameof(IsconeAttack))]
        public float ConeAngle;


        [Header("Tower_Stats")]
        public float Range;
        public float Damage;
        public float FireRate;
        public int pierce;



        [EnableIf(nameof(UsesProjectiles))]
        public GameObject ProjectilePrefab;


        public Material towerMaterial;


        public int CompostCost;

        [Header("Fusion System")]
        public string FusionID;
        public FusionStatType FusionStatType;
        public float FusionStatValue;






        /// <summary>
        /// CONDITIONS FOR EASY ACCESS TO TOWERDATA PROPERTIES
        /// </summary>


        private bool UsesProjectiles => AttackType == AttackType.Projectile || AttackType == AttackType.Mortor;

        private bool IsconeAttack => AttackType == AttackType.Cone;

        private bool UsesExplosion => AttackType == AttackType.Mortor || AttackType == AttackType.Radius;

        private bool IsDotDamage => DamageType == DamageType.DamageOverTime;

        private bool HasCrowdControl => CrowdControlType != CrowdControlType.None;

        private bool IsSlow => CrowdControlType == CrowdControlType.Slow;

        private bool IsMultipleTarget => EnemyTargetting == TargetType.Multiple;

    }
    public enum TowerRole
    {
        Ranged,
        Melee,
        CrowdControl,


    }
    public enum AttackType
    {
        Projectile,
        Cone,
        melee,
        Radius,
        Mortor
    }
    public enum CrowdControlType
    {
        None,
        Stun,
        Slow
    }
    public enum DamageType
    {
        Instant,
        DamageOverTime
    }
    public enum TargetType
    {
        First,
        Nearest,
        Multiple
    }

    public enum FusionStatType 
    {
        None,
        RangeBoost,
        DamageBoost,
        FireRateBoost,
        SlowEffectBoost,
        StunDurationBoost,
        DotDurationBoost,
        SplashRadiusBoost
    }
    






}
