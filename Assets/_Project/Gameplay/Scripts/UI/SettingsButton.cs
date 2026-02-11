using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] SettingsPanel settingsPanel;
        Button button;

        void Awake()
        {
            button = GetComponent<Button>();
        }

        void Start()
        {
            button.onClick.AddListener(settingsPanel.Show);
        }
    }
}
