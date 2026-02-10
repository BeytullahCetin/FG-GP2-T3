using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class AudioSettingsSlider : MonoBehaviour
    {
        public string audioSettingsKey;
        public Slider Slider => slider;
        public TMP_Text AudioSettingsText => audioSettingsText;

        Slider slider;
        TMP_Text audioSettingsText;

        void Awake()
        {
            slider = GetComponentInChildren<Slider>();
            audioSettingsText = GetComponentInChildren<TMP_Text>();
        }
    }
}
