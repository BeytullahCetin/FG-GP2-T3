using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GlobalSoundManager : MonoBehaviour
    {
        public static GlobalSoundManager Instance { get; private set; }

        public float poeLowHealthWarning = 0.2f;
        
        
        private List<GlobalSoundEmitter> _globalSoundEmitters = new();
        
        private bool hasPlayedLowHealthSound = false;
        
        
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
            EventManager.Register<OnUITowerEvent>(OnUITowerEvent);
            EventManager.Register<OnCellEvent>(OnCellEvent);
            EventManager.Register<OnCoreDamageEvent>(OnCoreDamageEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnTowerEvent>(OnTowerEvent);
            EventManager.Unregister<OnUITowerEvent>(OnUITowerEvent);
            EventManager.Unregister<OnCellEvent>(OnCellEvent);
            EventManager.Unregister<OnCoreDamageEvent>(OnCoreDamageEvent);
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

        private void OnUITowerEvent(OnUITowerEvent args)
        {
            UIEventType eventType = args.EventType;

            switch (eventType)
            {
                case UIEventType.Open:
                    Debug.LogWarning("Open tower");
                    break;
                case UIEventType.Close:
                    Debug.LogWarning("Close tower");
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
                    OnPlaySound(GlobalSoundType.PreviewPlaceTile);
                    break;
                case CellEventType.Rotate:
                    Debug.LogWarning("Rotate");
                    OnPlaySound(GlobalSoundType.RotateTile);
                    break;
                case CellEventType.Place:
                    OnPlaySound(GlobalSoundType.PlaceRoadDefault);
                    break;
                case CellEventType.Remove:
                    Debug.LogWarning("Remove");
                    break;
                case CellEventType.Cancel:
                    Debug.LogWarning("Cancel");
                    OnPlaySound(GlobalSoundType.CancelPlaceTile);
                    break;
            }
        }

        private void OnCoreDamageEvent(OnCoreDamageEvent args)
        {
            if (hasPlayedLowHealthSound)
                return;
            hasPlayedLowHealthSound = true;
            
            float health = args.CurrentHealth01;
            if (health < poeLowHealthWarning)
                OnPlaySound(GlobalSoundType.PoeLowHealth);
        }
        
    }
}
