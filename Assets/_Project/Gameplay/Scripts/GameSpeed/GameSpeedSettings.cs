using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    [CreateAssetMenu(fileName = "GameSpeedSettings", menuName = "Scriptable Objects/GameSpeedSettings")]
    public class GameSpeedSettings : ScriptableObject
    {
        [SerializeField] List<float> gameSpeeds = new List<float>();
        public List<float> GameSpeeds => gameSpeeds;
    }
}
