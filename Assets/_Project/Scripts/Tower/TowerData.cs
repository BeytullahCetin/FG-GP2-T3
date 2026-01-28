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
        public GameObject TowerPrefab;

        [Header("Tower_Stats")]
        public float Range;
        public float Damage;
        public float FireRate;

       
        public DamageType DamageType;
        public GameObject ProjectilePrefab;


        public int CompostCost;

    }
    public enum TowerRole
    {
        Ranged,
        Meele,
        CrowdControl,
        DOT

    }
    public enum DamageType
    {
        Instant,
        SingleTarget,
        DamageOverTime
    }


}
