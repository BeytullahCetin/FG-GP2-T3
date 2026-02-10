using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// This script handles both melee and ranged projectiles.
///
///  Melee Projectiles:->Rigidbody = OFF and InstantHit = true
///
///  Ranged Projectiles: ->Gravity = OFF in Rigidbody and  InstantHit = false
/// </summary>
namespace FG_GP2_T3
{
    public class Projectile : MonoBehaviour
    {
        private Transform _Target;
        private float _Damage;
        private TowerAttack _OwnerAttack;

        [Header("Projectile Settings")]
        public float Speed;

        [Header("Melee Mode")]
        public bool InstantHit = false;

        public void Initialize(
            Transform target,
            float damage,
            TowerAttack ownerAttack
        )
        {
            _Target = target;
            _Damage = damage;
            _OwnerAttack = ownerAttack;

            if (InstantHit)
                DoInstantHit();
        }

        private void DoInstantHit()
        {
            IDamageable enemy = _Target.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(_Damage);
                _OwnerAttack.ApplyEffects(enemy);
            }

            Destroy(gameObject, 0.1f);
        }

        void Update()
        {
            if (InstantHit) return;

            if (_Target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = (_Target.position - transform.position).normalized;
            transform.position += dir * Speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;

            IDamageable enemy = other.GetComponent<IDamageable>();
            if (enemy != null)
            {
                enemy.TakeDamage(_Damage);
                _OwnerAttack.ApplyEffects(enemy);
            }

            Destroy(gameObject);
        }
    }
}
