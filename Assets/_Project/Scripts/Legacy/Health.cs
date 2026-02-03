using System;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100;
        [ReadOnly][SerializeField] private float _health;

        public event Action OnTakeDamage = delegate{};
        public event Action OnDead = delegate{};

        public float CurrentHealth => _health;
        public float MaxHealth => _maxHealth;

        public bool IsAlive => _health > 0;

        void Start() => ResetHealth();

        public void ResetHealth() => _health = _maxHealth;

        public void TakeDamage(float damage)
        {
            if (IsAlive == false)
                return;

            _health -= damage;
            OnTakeDamage.Invoke();
            CheckDeath();
        }

        public void CheckDeath()
        {
            if (_health <= 0)
                OnDead.Invoke();
        }

        [Button]
        private void TestDeath()
        {
            _health = 0;
            CheckDeath();

            Debug.Log(ToString());
        }

        [Button]
        private void TestDamage()
        {
            TakeDamage(5);
            
            Debug.Log(ToString());
        }

        public override string ToString() => $"Health: {_health}/{_maxHealth}, Alive: {IsAlive}";
    }
}
