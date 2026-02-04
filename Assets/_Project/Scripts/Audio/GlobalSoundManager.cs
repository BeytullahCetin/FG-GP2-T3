using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GlobalSoundManager : MonoBehaviour
    {
        public static GlobalSoundManager Instance { get; private set; }
        
        
        private List<GlobalSoundEmitter> _globalSoundEmitters = new();
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            _globalSoundEmitters = gameObject.GetComponentsInChildren<GlobalSoundEmitter>().ToList();
        }

        private void OnEnable()
        {
            EventManager.Register<OnTowerEvent>(OnTowerEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnTowerEvent>(OnTowerEvent);
        }


        public void OnPlaySound(GlobalSoundType type)
        {
            // Don't play if sound is set to default
            if (type == GlobalSoundType.Default)// && !Application.isEditor)
                return;
            
            _globalSoundEmitters.Find(x => x.soundType == type).Play();
        }

        public void OnStopSound(GlobalSoundType type)
        {
            _globalSoundEmitters.Find(x => x.soundType == type).Stop();
        }


        private void OnTowerEvent(OnTowerEvent args)
        {
            TowerEventType eventType = args.EventType;
            TowerData tdata = args.Tower.Data;

            switch (eventType)
            {
                case TowerEventType.Build:
                    OnPlaySound(tdata.SoundOnPlaced);
                    break;
                case TowerEventType.Fuse:
                    OnPlaySound(tdata.SoundOnPlaced);
                    //TODO: Also needs to play fusion sound
                    break;
                case TowerEventType.Select:
                    OnPlaySound(tdata.SoundOnSelected);
                    break;
                case TowerEventType.Deselect:
                    OnStopSound(tdata.SoundOnSelected);
                    break;
            }
        }
    }
}
