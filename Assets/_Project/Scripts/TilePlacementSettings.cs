using DG.Tweening;
using UnityEngine;

namespace FG_GP2_T3
{
    [CreateAssetMenu(fileName = "TilePlacementSettings", menuName = "Scriptable Objects/TilePlacementSettings")]
    public class TilePlacementSettings : ScriptableObject
    {
        [Header("Tower Placement")]
        public float placementDuration = .2f;
        public Ease placementEase = Ease.OutQuint;

        [Header("Tower Shake")]
        public float shakeDuration = .2f;
        public float shakeStrenght = 1f;
        public int shakeVibrato = 10;
    }
}
