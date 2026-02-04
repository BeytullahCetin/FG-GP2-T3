using System;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace FG_GP2_T3
{
    public class FmodGlobalParamManager : MonoBehaviour
    {
        public static FmodGlobalParamManager Instance { get; private set; }
        
        
        public List<FmodGlobalParam> globalParams;
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void OnEnable()
        {
            EventManager.Register<OnVolumeEvent>(OnVolumeEvent);
            EventManager.Register<OnPauseMenuEvent>(OnPauseMenuEvent);
            EventManager.Register<OnCoreDamageEvent>(OnCoreDamageEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnVolumeEvent>(OnVolumeEvent);
            EventManager.Unregister<OnPauseMenuEvent>(OnPauseMenuEvent);
            EventManager.Unregister<OnCoreDamageEvent>(OnCoreDamageEvent);
        }


        public void SetParameter(FmodGlobalParamType globalParamType, float value)
        {
            string paramName = globalParams.Find(x => x.globalParamType == globalParamType).paramName;
            RuntimeManager.StudioSystem.setParameterByName(paramName, value);
        }

        
        private void OnVolumeEvent(OnVolumeEvent args)
        {
            VolumeType volumeType = args.VolumeType;
            float volume = args.Volume01;
            
            switch (volumeType)
            {
                case VolumeType.VolumeMain:
                    SetParameter(FmodGlobalParamType.VolumeMain, volume);
                    break;
                case VolumeType.VolumeMusic:
                    SetParameter(FmodGlobalParamType.VolumeMusic, volume);
                    break;
                case VolumeType.VolumeAmbience:
                    SetParameter(FmodGlobalParamType.VolumeAmbience, volume);
                    break;
                case VolumeType.VolumeSfxAuto:
                    SetParameter(FmodGlobalParamType.VolumeSfxAuto, volume);
                    break;
                case VolumeType.VolumeSfxInteraction:
                    SetParameter(FmodGlobalParamType.VolumeSfxInteraction, volume);
                    break;
                case VolumeType.VolumeNarration:
                    SetParameter(FmodGlobalParamType.VolumeNarration, volume);
                    break;
                case VolumeType.VolumeUi:
                    SetParameter(FmodGlobalParamType.VolumeUi, volume);
                    break;
            }
        }

        private void OnPauseMenuEvent(OnPauseMenuEvent args)
        {
            UIEventType uiEventType = args.EventType;

            switch (uiEventType)
            {
                case UIEventType.Open:
                    SetParameter(FmodGlobalParamType.IsPaused, 1);
                    break;
                case UIEventType.Close:
                    SetParameter(FmodGlobalParamType.IsPaused, 0);
                    break;
            }
        }
        
        private void OnCoreDamageEvent(OnCoreDamageEvent args)
        {
            float health = args.CurrentHealth01;
            
            SetParameter(FmodGlobalParamType.PoeHealth, health);
        }
    }
}
