using System;
using UnityEngine;

namespace FG_GP2_T3
{
    public class MotarProjectile : MonoBehaviour
    {
        Transform _Target;
        float _Damage;
        float _ExplosionRadius;
        TowerAttack _OwnerAttack;
        LayerMask _enemylayer;

        Vector3 _StartPoint;
        Vector3 _TargetPoint;

        float _FlightTime = 1.5f;
        float _Timer;
        float _ArcHeight = 5f;
        bool _Exploded;

        public void Initialize(
            Transform target,
            float damage,
            float explosionRadius,
            TowerAttack ownerAttack,
            LayerMask enemyLayer
        )
        {
            _Target = target;
            _Damage = damage;
            _ExplosionRadius = explosionRadius;
            _OwnerAttack = ownerAttack;
            _enemylayer = enemyLayer;

            _StartPoint = transform.position;
            _TargetPoint = target.position;
        }

        private void Update()
        {
            if (_Exploded) return;

            _Timer += Time.deltaTime;
            float t = _Timer / _FlightTime;

            if (t >= 1f || _Target == null)
            {
                Explode();
                return;
            }
            if (_Target != null)
                _TargetPoint = _Target.position;

            Vector3 flatPos = Vector3.Lerp(_StartPoint, _TargetPoint, t);

            float arc = Mathf.Sin(t * Mathf.PI) * _ArcHeight;
            transform.position = flatPos + Vector3.up * arc;
        }

        private void Explode()
        {
            if (_Exploded) return;
            _Exploded = true;

            Collider[] hits = Physics.OverlapSphere(transform.position, _ExplosionRadius, _enemylayer);

            foreach (Collider hit in hits)
            {
                IDamageable enemy = hit.GetComponentInParent<IDamageable>();
                if (enemy == null) continue;

                enemy.TakeDamage(_Damage);
                _OwnerAttack.ApplyEffects(enemy);
            }
            Debug.Log($"Explosion Radius: {_ExplosionRadius}");
            Debug.Log($"Hit Count: {hits.Length}");

            Destroy(gameObject);
        }
    
    }
}
    

