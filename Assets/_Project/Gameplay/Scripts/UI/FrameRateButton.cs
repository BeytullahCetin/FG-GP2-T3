using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class FrameRateButton : MonoBehaviour
    {
        public Button Button => button;
        public int frameRate;
        public TMP_Text FrameRateText => frameRateText;

        Button button;
        TMP_Text frameRateText;

        void Awake()
        {
            button = GetComponent<Button>();
            frameRateText = GetComponentInChildren<TMP_Text>();
        }
    }
}
