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
            EventManager.Register<OnCellEvent>(OnCellEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnTowerEvent>(OnTowerEvent);
            EventManager.Unregister<OnCellEvent>(OnCellEvent);
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
                    Debug.LogWarning("Select tower");
                    OnPlaySound(tdata.SoundOnSelected);
                    break;
                case TowerEventType.Deselect:
                    Debug.LogWarning("Deselect tower");
                    OnStopSound(tdata.SoundOnSelected);
                    break;
            }
        }
        
        private void OnCellEvent(OnCellEvent args)
        {
            CellEventType eventType = args.EventType;
            
            switch (eventType)
            {
                case CellEventType.Click:
                    Debug.LogWarning("Click");
                    OnPlaySound(GlobalSoundType.SelectTileBlank);
                    break;
                case CellEventType.Rotate:
                    Debug.LogWarning("Rotate");
                    break;
                case CellEventType.Place:
                    OnPlaySound(GlobalSoundType.PlaceRoadDefault);
                    break;
                case CellEventType.Remove:
                    Debug.LogWarning("Remove");
                    break;
                case CellEventType.Cancel:
                    Debug.LogWarning("Cancel");
                    break;
            }
        }
        
    }
}
