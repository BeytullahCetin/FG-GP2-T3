using System;
using System.Collections.Generic;
using System.Linq;
using FMODUnity;
using UnityEngine;

namespace FG_GP2_T3
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance;
        public static string PP_FrameRate = "PP_FrameRate";
        public static string PP_QualityIndex = "PP_QualityIndex";

        public List<string> QualitySettings => qualitySettings;
        public List<int> SupportedFrameRates => supportedFrameRates;
        public List<AudioSettings> AudioSettingsList => audioSettingsList;

        SettingsPanel settingsPanel;

        [Header("Quality Settings")]
        List<string> qualitySettings = new List<string>();
        [SerializeField] List<int> supportedFrameRates = new List<int>();

        [Header("Sound Settings")]
        [SerializeField] List<AudioSettings> audioSettingsList = new List<AudioSettings>();

        void Awake()
        {
            Instance = this;
            qualitySettings = UnityEngine.QualitySettings.names.ToList();
            settingsPanel = FindAnyObjectByType<SettingsPanel>();
        }

        void Start()
        {
            LoadSettings();
            settingsPanel.Initialize();
        }

        public void LoadSettings()
        {
            SetTargetFrameRate(PlayerPrefs.GetInt(PP_FrameRate, 60));
            SetQualityLevel(PlayerPrefs.GetInt(PP_QualityIndex, 0));

            foreach (AudioSettings audioSettings in audioSettingsList)
            {
                SetParameterByName(audioSettings.settingsKey, PlayerPrefs.GetFloat(audioSettings.settingsKey, .5f));
            }
        }

        public void SetParameterByName(string key, float value)
        {
            RuntimeManager.StudioSystem.setParameterByName(key, value);
        }

        public void SetQualityLevel(int qualityIndex)
        {
            UnityEngine.QualitySettings.SetQualityLevel(qualityIndex, true);
        }

        public void SetTargetFrameRate(int value)
        {
            Application.targetFrameRate = value;
        }
    }

    [Serializable]
    public class AudioSettings
    {
        public string settingsName;
        public string settingsKey;
    }
}
