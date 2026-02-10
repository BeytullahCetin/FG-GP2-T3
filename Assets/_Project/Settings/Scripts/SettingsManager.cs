using UnityEngine;

namespace FG_GP2_T3
{
    public class SettingsManager : MonoBehaviour
    {
        public static string PP_FrameRate = "PP_FrameRate";
        public static string PP_QualityIndex = "PP_QualityIndex";

        public static SettingsManager Instance;
        public QualitySettingsController QualitySettingsController => qualitySettingsController;
        public SoundSettingsController SoundSettingsController => soundSettingsController;

        QualitySettingsController qualitySettingsController;
        SoundSettingsController soundSettingsController;

        void Awake()
        {
            Instance = this;
            qualitySettingsController = GetComponent<QualitySettingsController>();
            soundSettingsController = GetComponent<SoundSettingsController>();

            qualitySettingsController.Load();
            soundSettingsController.Load();
        }

        void Start()
        {
        }
    }
}
