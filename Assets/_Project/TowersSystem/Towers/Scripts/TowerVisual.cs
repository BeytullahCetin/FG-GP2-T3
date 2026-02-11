using UnityEngine;
using NaughtyAttributes;

namespace FG_GP2_T3
{
    public class TowerVisual : MonoBehaviour
    {
        [Expandable]
        [SerializeField] private TowerData towerData;

        [SerializeField] private Animator animator;

        public TowerData TowerData => towerData;

        private static readonly int IdleHash = Animator.StringToHash("IsIdle");
        private static readonly int AttackHash = Animator.StringToHash("Attack");

        private void Awake()
        {
            if (!animator)
                animator = GetComponentInChildren<Animator>();
        }

        public void PlayIdle()
        {
            if (!animator) return;
            animator.SetBool(IdleHash, true);
        }

        public void PlayAttack()
        {
            if (!animator) return;
            animator.SetBool(IdleHash, false);
            animator.SetTrigger(AttackHash);
        }
    }
}
