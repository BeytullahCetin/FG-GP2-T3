using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class SettingsButton : MonoBehaviour
    {
        Button button;
        SettingsPanel settingsPanel;

        void Awake()
        {
            settingsPanel = FindAnyObjectByType<SettingsPanel>();
            button = GetComponent<Button>();
        }

        void Start()
        {
            button.onClick.AddListener(settingsPanel.Show);
        }
    }
}
