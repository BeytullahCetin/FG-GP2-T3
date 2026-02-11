
using System;
using UnityEngine;

namespace FG_GP2_T3
{
    public enum WaveEventType
    {
        Start,
        EnemyCountChanged,
        End
    }

    public class OnWaveEvent : GameEventArgs
    {
        public readonly WaveEventType EventType;
        public readonly int WaveNumber;
        public readonly int EnemyCount;

        public OnWaveEvent(WaveEventType eventType, int waveNumber, int enemyCount = 0)
        {
            EventType = eventType;
            WaveNumber = waveNumber;
            EnemyCount = enemyCount;
        }
    }

    public class OnGameEndedEvent : GameEventArgs
    {
        public readonly bool PlayerWon;

        public OnGameEndedEvent(bool playerWon)
        {
            PlayerWon = playerWon;
        }
    }

    public class OnCoreDamageEvent : GameEventArgs
    {
        public readonly int CurrentHealth;
        public readonly float CurrentHealth01;
        public readonly int DamageTaken;

        public OnCoreDamageEvent(int currentHealth, float currentHealth01, int damageTaken)
        {
            CurrentHealth = currentHealth;
            CurrentHealth01 = currentHealth01;
            DamageTaken = damageTaken;
        }
    }
    
    public class OnBranchLostEvent : GameEventArgs
    {
        public GameObject Branch;

        public OnBranchLostEvent(GameObject branch)
        {
            Branch = branch;
        }
    }
}