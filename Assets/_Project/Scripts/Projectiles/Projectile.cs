using System;
using UnityEngine;

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


        [Header("Projectile Settings")]
        public float Speed;

        [Header("Melee Mode")]
        public bool InstantHit = false;

        internal void Initiliaze(Transform currentTarget, float damage)
        {
            _Target = currentTarget;
            _Damage = damage;

            if (InstantHit)
            {
                DoInstantHit();
            }
        }

        private void DoInstantHit()
        {
            Health enemy = _Target.GetComponent<Health>();

            if (enemy != null)
            {
                enemy.TakeDamage(_Damage);
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

            Vector3 Dir = (_Target.position - transform.position).normalized;
            transform.position += Dir * Speed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;

            Health enemy = other.GetComponent<Health>();

            if (enemy != null)
            {
                enemy.TakeDamage(_Damage);
            }

            Destroy(gameObject);
        }
    }
}
