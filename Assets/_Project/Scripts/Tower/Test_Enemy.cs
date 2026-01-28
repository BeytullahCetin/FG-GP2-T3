using UnityEngine;
///<summary>
///This enemy class is to just for checking tower attacks
///</summary>

namespace FG_GP2_T3
{
    public class Test_Enemy : MonoBehaviour
    {
        public float maxHP = 50f;
        private float currentHP;

        void Start()
        {
            currentHP = maxHP;
        }

        public void TakeDamage(float amount)
        {
            currentHP -= amount;

            Debug.Log($"{gameObject.name} took {amount} damage. HP: {currentHP}");

            if (currentHP <= 0)
            {
                Die();
            }
        }

        void Die()
        {
            Destroy(gameObject);
        }
    }
}
