using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class TowerConfirmation : MonoBehaviour
    {
        [SerializeField] Button cancelButton;
        [SerializeField] Button confirmButton;

        public Button CancelButton => cancelButton;
        public Button ConfirmButton => confirmButton;
    }
}
