using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class ZoomButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private StickyCameraMovement cameraMovement;

        void Awake()
        {
            if (button == null) button = GetComponent<Button>();
            if (cameraMovement == null) cameraMovement = Object.FindAnyObjectByType<StickyCameraMovement>();
        }

        void Start()
        {
            if (button != null)
            {
                button.onClick.AddListener(OnButtonClick);
            }
        }

        private void OnButtonClick()
        {
            if (cameraMovement == null) return;

            if (cameraMovement.IsZoomedIn)
            {
                cameraMovement.ZoomOut();
            }
            else
            {
                cameraMovement.ZoomIn();
            }
        }
    }
}