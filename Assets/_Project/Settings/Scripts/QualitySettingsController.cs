using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class QualitySettingsController : MonoBehaviour
    {
        public List<string> QualitySettings => qualitySettings;
        public List<int> SupportedFrameRates => supportedFrameRates;

        List<string> qualitySettings = new List<string>();
        [SerializeField] List<int> supportedFrameRates = new List<int>();

        void Awake()
        {
            qualitySettings = UnityEngine.QualitySettings.names.ToList();
        }

        public void Load()
        {

        }

        public void Save()
        {

        }
    }
}
