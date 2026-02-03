using System;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class TileRotationConfirmation : MonoBehaviour
    {
        [SerializeField] Button cancelButton;
        [SerializeField] Button rotateButton;
        [SerializeField] Button confirmButton;

        public Button CancelButton => cancelButton;
        public Button RotateButton => rotateButton;
        public Button ConfirmButton => confirmButton;
    }
}
