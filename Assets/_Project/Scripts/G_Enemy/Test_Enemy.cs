using System.Collections;
using UnityEngine;
///<summary>
///This is just a test enemy script to implement IDamegable interface for testing purposes
///</summary>
namespace FG_GP2_T3
{
    public class Test_Enemy : MonoBehaviour, IDamegable
    {
        public float maxHP = 50f;
        private float currentHP;

        public float speed = 5f;
        private float baseSpeed;

        public GameObject target;

        private void Start()
        {
            currentHP = maxHP;
            baseSpeed = speed;
        }

        public void Update()
        {
            if (target == null)
            {
                return;
            }
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        }

        public void TakeDamage(float amount)
        {
            currentHP -= amount;

            Debug.Log($"{name} took {amount} damage. HP: {currentHP}");

            if (currentHP <= 0)
                Destroy(gameObject);
        }

        public void ApplySlow(float percent, float duration)
        {
            StopCoroutine(nameof(SlowRoutine));
            StartCoroutine(SlowRoutine(percent, duration));
        }

        IEnumerator SlowRoutine(float percent, float duration)
        {
            speed = baseSpeed * (1f - percent);

            yield return new WaitForSeconds(duration);

            speed = baseSpeed;
        }

        public void ApplyStun(float duration)
        {
            StopCoroutine(nameof(StunRoutine));
            StartCoroutine(StunRoutine(duration));
        }

        IEnumerator StunRoutine(float duration)
        {
            Debug.Log($"{name} is stunned for {duration} seconds.");
            float savedSpeed = speed;
            speed = 0f;

            yield return new WaitForSeconds(duration);

            speed = savedSpeed;
        }

        public void ApplyDot(float dps, float duration)
        {
            StartCoroutine(DotRoutine(dps, duration));
        }

        IEnumerator DotRoutine(float dps, float duration)
        {
            Debug.Log($"{name} is taking {dps} DPS for {duration} seconds.");
            float timer = 0f;

            while (timer < duration)
            {
                TakeDamage(dps * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
}
