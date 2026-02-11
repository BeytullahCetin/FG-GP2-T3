using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GlobalSoundManager : MonoBehaviour
    {
        public static GlobalSoundManager Instance { get; private set; }

        public bool printSoundLogs = false;
        public bool printEventLogs = false;
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
            EventManager.Register<OnWaveEvent>(OnWaveEvent);
            EventManager.Register<OnGameEndedEvent>(OnGameEndedEvent);
            EventManager.Register<OnCoreDamageEvent>(OnCoreDamageEvent);
            EventManager.Register<OnBranchLostEvent>(OnBranchLostEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnTowerEvent>(OnTowerEvent);
            EventManager.Unregister<OnUITowerEvent>(OnUITowerEvent);
            EventManager.Unregister<OnCellEvent>(OnCellEvent);
            EventManager.Unregister<OnWaveEvent>(OnWaveEvent);
            EventManager.Unregister<OnGameEndedEvent>(OnGameEndedEvent);
            EventManager.Unregister<OnCoreDamageEvent>(OnCoreDamageEvent);
            EventManager.Unregister<OnBranchLostEvent>(OnBranchLostEvent);
        }


        public void OnPlaySound(GlobalSoundType type)
        {
            // Don't play if sound is set to default
            if (type == GlobalSoundType.Default)// && !Application.isEditor)
                return;

            SoundLog(true, type);
            _globalSoundEmitters.Find(x => x.soundType == type).Play();
        }

        public void OnStopSound(GlobalSoundType type)
        {
            SoundLog(false, type);
            _globalSoundEmitters.Find(x => x.soundType == type).Stop();
        }


        private void OnTowerEvent(OnTowerEvent args)
        {
            TowerEventType eventType = args.EventType;
            TowerData tdata = args.Tower.Data;
            EventLog("OnTowerEvent: " + eventType);

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

        private void OnUITowerEvent(OnUITowerEvent args)
        {
            UIEventType eventType = args.EventType;
            EventLog("OnUITowerEvent: " + eventType);

            switch (eventType)
            {
                case UIEventType.Open:
                    OnPlaySound(GlobalSoundType.TowerMenuOpen);
                    break;
                case UIEventType.Close:
                    OnPlaySound(GlobalSoundType.TowerMenuClose);
                    break;
            }
        }
        
        private void OnCellEvent(OnCellEvent args)
        {
            CellEventType eventType = args.EventType;
            EventLog("OnCellEvent: " + eventType);
            
            switch (eventType)
            {
                case CellEventType.Click:
                    OnPlaySound(GlobalSoundType.PreviewPlaceTile);
                    break;
                case CellEventType.Rotate:
                    OnPlaySound(GlobalSoundType.RotateTile);
                    break;
                case CellEventType.Place:
                    OnPlaySound(GlobalSoundType.PlaceRoadDefault);
                    break;
                case CellEventType.Remove:
                    break;
                case CellEventType.Cancel:
                    OnPlaySound(GlobalSoundType.CancelPlaceTile);
                    break;
            }
        }

        private void OnWaveEvent(OnWaveEvent args)
        {
            WaveEventType eventType = args.EventType;
            EventLog("OnWaveEvent: " + eventType);

            switch (eventType)
            {
                case WaveEventType.Start:
                    OnPlaySound(GlobalSoundType.WaveStart);
                    break;
                case WaveEventType.EnemyCountChanged:
                    break;
                case WaveEventType.End:
                    OnPlaySound(GlobalSoundType.WaveEnd);
                    break;
            }
        }

        private void OnGameEndedEvent(OnGameEndedEvent args)
        {
            bool playerWon = args.PlayerWon;
            EventLog("OnGameEndedEvent: Player " + (playerWon ? "Won" : "Lost"));
            
            if (playerWon)
                OnPlaySound(GlobalSoundType.GameWin);
            else
                OnPlaySound(GlobalSoundType.GameLose);
        }
        
        private void OnCoreDamageEvent(OnCoreDamageEvent args)
        {
            EventLog("OnCoreDamageEvent");
            if (hasPlayedLowHealthSound)
                return;
            hasPlayedLowHealthSound = true;
            
            float health = args.CurrentHealth01;
            if (health < poeLowHealthWarning)
                OnPlaySound(GlobalSoundType.PoeLowHealth);
        }

        private void OnBranchLostEvent(OnBranchLostEvent args)
        {
            EventLog("OnBranchLostEvent");
        }


        private void SoundLog(bool started, GlobalSoundType type)
        {
            if (!printSoundLogs)
                return;
            if (!Application.isEditor)
                return;
            
            string toPrint = $"<color=green>Global Sound {(started ? "Played" : "Stopped")}: </color><color=white>{type}</color>";
            Debug.Log(toPrint);
        }
        
        private void EventLog(string log)
        {
            if (!printEventLogs)
                return;
            if (!Application.isEditor)
                return;
            
            string toPrint = $"<color=green>Global Sound Event: </color><color=white>{log}</color>";
            Debug.Log(toPrint);
        }
    }
}
