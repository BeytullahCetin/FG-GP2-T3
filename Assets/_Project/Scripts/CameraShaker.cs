using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] float duration = .5f;
        [SerializeField] float strenght = 1;
        [SerializeField] int vibrato = 10;

        void OnEnable()
        {
            TilePlacementController.OnTilePlaced += ShakeCameraOnTilePlacement;
        }

        void OnDisable()
        {
            TilePlacementController.OnTilePlaced -= ShakeCameraOnTilePlacement;
        }

        [Button]
        void ShakeCameraOnTilePlacement()
        {
            cam.DOShakeRotation(duration, strenght, vibrato);
        }
    }
}
