using UnityEngine;

namespace FG_GP2_T3
{
    public class WaveUI : MonoBehaviour
    {
        private HudElement hudElement;

        // TODO: Register OnWaveChangeEvent

        void Awake()
        {
            hudElement = GetComponent<HudElement>();
        }

        void Start()
        {
            UpdateWaveText();
        }

        void UpdateWaveText()
        {
            hudElement.ContentText.FillText((EnemyManager.Instance.WaveIndex + 1).ToString());
        }
    }
}
