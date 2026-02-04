using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// this interface defines the methods that any damageable entity must implement
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float damageAmount);
        void ApplySlow(float slowPercentage, float duration);
        void ApplyStun(float duration);
        void ApplyDot(float damagePerSecond, float duration);
    }
}
