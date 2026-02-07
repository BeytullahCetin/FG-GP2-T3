using System;
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    [CreateAssetMenu(fileName = "TowerStatTranslator", menuName = "Scriptable Objects/TowerStatTranslator")]
    public class TowerStatTranslator : ScriptableObject
    {
        [SerializeField] List<TowerTranslateData> towerTranslateDatas = new List<TowerTranslateData>();

        public string GetTranslation(float value)
        {
            foreach (TowerTranslateData data in towerTranslateDatas)
            {
                if (value < data.valueUntil)
                {
                    return data.translation;
                }
            }

            return "-";
        }
    }

    [Serializable]
    public class TowerTranslateData
    {
        public float valueUntil;
        public string translation;
    }
}
