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
        [ShowAssetPreview] public Sprite TowerIcon;
        public string TowerName;
        public string Role;
        public int Cost;
        [TextArea] public string Description;
        [TextArea] public string ReferanceDescription;
        [TextArea] public string RealLifeEquivalentDescription;

        public AttackType AttackType;
        public DamageType DamageType;

        public GlobalSoundType SoundOnSelected;
        public GlobalSoundType SoundOnPlaced;

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
        public float DotDuration;

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
        public string RangeString;
        public string DamageString;
        public string FireRateString;
        public Vector2 RotationOffset;


        [EnableIf(nameof(UsesProjectiles))]
        public GameObject ProjectilePrefab;


        public Material towerMaterial;


        public int CompostCost;

        [Header("Fusion System")]
        public string FusionID;
        public FusionStatType FusionStatType;
        public string FusionStatString;
        public float FusionStatValue;






        /// <summary>
        /// CONDITIONS FOR EASY ACCESS TO TOWERDATA PROPERTIES
        /// </summary>


        private bool UsesProjectiles => AttackType == AttackType.Projectile || AttackType == AttackType.Mortar;

        private bool IsconeAttack => AttackType == AttackType.Cone;

        private bool UsesExplosion => AttackType == AttackType.Mortar || AttackType == AttackType.Radius;

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
        Melee,
        Radius,
        Mortar
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
