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
            int waveNo = EnemyManager.Instance.WaveIndex + 1;
            int totalWaveCount = EnemyManager.Instance.Waves.Count;

            if (waveNo <= totalWaveCount)
            {
                hudElement.ContentText.FillText($"{waveNo}/{totalWaveCount}");
                return;
            }

            GameManager.Instance.SwitchToGameWinState();
        }

        void UpdateWaveText(OnWaveEvent args)
        {
            if (args.EventType == WaveEventType.End)
                UpdateWaveText();
        }
    }
}
