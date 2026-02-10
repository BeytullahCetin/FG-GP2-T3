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
            EventManager.Register<OnCoreDamageEvent>(UpdateHealthText);
        }

        void UpdateHealthText()
        {
            int health = Mathf.RoundToInt(Mathf.Clamp(EnemyManager.Instance.GetTarget().Health, 0, 100));
            hudElement.ContentText.FillText(health.ToString());
        }

        void UpdateHealthText(OnCoreDamageEvent args)
        {
            UpdateHealthText();
        }
    }
}
