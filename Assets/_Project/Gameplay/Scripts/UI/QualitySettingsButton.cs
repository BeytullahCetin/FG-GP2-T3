using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class QualitySettingsButton : MonoBehaviour
    {
        public int qualityIndex;
        public string qualityName;
        public Button Button => button;
        public TMP_Text QualitySettingsText => qualitySettingsText;

        Button button;
        TMP_Text qualitySettingsText;

        void Awake()
        {
            button = GetComponent<Button>();
            qualitySettingsText = GetComponentInChildren<TMP_Text>();
        }
    }
}
