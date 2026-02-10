using UnityEngine;

namespace FG_GP2_T3
{
    public class HealthUI : MonoBehaviour
    {
        private HudElement hudElement;

        // TODO: Register OnTakeDamageEvent

        void Awake()
        {
            hudElement = GetComponent<HudElement>();
        }

        void Start()
        {
            UpdateHealthText();
        }

        void UpdateHealthText()
        {
            hudElement.ContentText.FillText(EnemyManager.Instance.GetTarget().Health.ToString());
        }
    }
}
