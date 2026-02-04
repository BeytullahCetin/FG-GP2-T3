
using System;

namespace FG_GP2_T3
{
    public enum WaveEventType
    {
        Start,
        End
    }

    public class OnWaveEvent : GameEventArgs
    {
        public readonly WaveEventType EventType;
        public readonly int WaveNumber;

        public OnWaveEvent(WaveEventType eventType, int waveNumber)
        {
            EventType = eventType;
            WaveNumber = waveNumber;
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
        public readonly int CurrentHealth01;
        public readonly int DamageTaken;

        public OnCoreDamageEvent(int currentHealth, int currentHealth01, int damageTaken)
        {
            CurrentHealth = currentHealth;
            CurrentHealth01 = currentHealth01;
            DamageTaken = damageTaken;
        }
    }
}