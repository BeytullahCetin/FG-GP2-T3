using System;
using UnityEngine;

namespace FG_GP2_T3
{
    public class MotarProjectile : MonoBehaviour
    {
        Transform _Target;
        float _Damage;
        float _ExplosionRadius;

        Vector3 _StartPoint;
        Vector3 _TargetPoint;

        float _Flightime=1.5f;
        float _Timer;
        float _ArcHeight=5f;
        private bool _exploded;

        public void Initialize(Transform target, float damage, float explosionRadius)
        {
            _Target = target;
            _Damage = damage;
            _ExplosionRadius = explosionRadius;

            _StartPoint = transform.position;
            _TargetPoint = target.position;
        }

        private void Update()
        {
            if (_exploded) return;

            if (_Target == null)
            {
                Explode();
                return;
            }

            _Timer += Time.deltaTime;
            float t= _Timer / _Flightime;
            if (t>=1f)
            {
                Explode();
                return;
            }

            Vector3 flatpos=Vector3.Lerp(_StartPoint,_TargetPoint, t);

            float Arc=MathF.Sin(t* MathF.PI) * _ArcHeight;

            transform.position=flatpos + Vector3.up * Arc;

        }

        private void Explode()
        {
            if (_exploded) return;
            _exploded = true;

            Collider[] hits = Physics.OverlapSphere(transform.position, _ExplosionRadius);

            foreach (Collider hit in hits)
            {
                Enemy enemy = hit.GetComponentInParent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(_Damage);
                }
            }

            if (hits.Length > 0)
                Debug.Log($"<color=red>{_Damage} given By {hits[0].name} to {_Target.name}</color>");
            Destroy(gameObject);
        }
            
           
        }
        

    }

