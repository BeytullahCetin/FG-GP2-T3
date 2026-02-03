using System;
using UnityEngine;

namespace FG_GP2_T3
{
    public class MotarProjectile : MonoBehaviour
    {
        Transform _Target;
        float _Damage;
        float _ExplosionRadius;

        public void Initialize(Transform target, float damage, float explosionRadius)
        {
            _Target = target;
            _Damage = damage;
            _ExplosionRadius = explosionRadius;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, _Target.position, 10f * Time.deltaTime);

            if (Vector3.Distance(transform.position, _Target.position) < 0.2f)
            {
                Explode();
            }
        }

        private void Explode()
        {
            Collider[] Hits=Physics.OverlapSphere(transform.position, _ExplosionRadius);

            foreach(Collider hit in Hits)
            {
                Test_Enemy enemy=hit.GetComponent<Test_Enemy>();
                if(enemy != null)
                {
                    enemy.TakeDamage(_Damage);
                }
                Destroy(gameObject);
            }
        }
    }
}
