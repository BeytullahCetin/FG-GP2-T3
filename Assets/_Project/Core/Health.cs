using System;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float maxHealthMultiplier = 1;
        [SerializeField] private float maxHealth = 100;
        [ReadOnly][SerializeField] private float currentHealth;

        public event Action OnTakeDamage = delegate { };
        public event Action OnDead = delegate { };

        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        public bool IsAlive => currentHealth > 0;

        public void SetMaxHealthMultiplier(float multiplier)
        {
            maxHealthMultiplier = multiplier;
        }

        void Start()
        {
            ResetHealth();
        }

        public void TakeDamage(float damage)
        {
            if (IsAlive == false)
                return;

            currentHealth -= damage;
            OnTakeDamage.Invoke();
            CheckDeath();
        }

        [Button]
        public void InstaDeath()
        {
            currentHealth = 0;
            CheckDeath();
        }

        public void CheckDeath()
        {
            if (currentHealth <= 0)
            {
                OnDead.Invoke();
            }
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth * maxHealthMultiplier;
        }

        [Button]
        void TakeDamageTest()
        {
            TakeDamage(75);
            Debug.Log($"Remaining health: {currentHealth}");
        }
    }
}
