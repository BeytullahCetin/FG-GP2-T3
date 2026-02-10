using UnityEngine;

namespace FG_GP2_T3
{
    public class WaveUI : MonoBehaviour
    {
        private HudElement hudElement;

        void Awake()
        {
            hudElement = GetComponent<HudElement>();
        }

        void Start()
        {
            UpdateWaveText();
            EventManager.Register<OnWaveEvent>(UpdateWaveText);
        }

        void UpdateWaveText()
        {
            hudElement.ContentText.FillText((EnemyManager.Instance.WaveIndex + 1).ToString());
        }

        void UpdateWaveText(OnWaveEvent args)
        {
            if (args.EventType == WaveEventType.End)
                UpdateWaveText();
        }
    }
}
