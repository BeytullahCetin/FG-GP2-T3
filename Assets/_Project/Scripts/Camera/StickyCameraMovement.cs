using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FG_GP2_T3
{
    public class StickyCameraMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionReference dragAction;
        [SerializeField] private InputActionReference screenPositionAction;

        [Header("Zoom Settings")]
        [SerializeField] private InputActionReference zoomInAction;
        [SerializeField] private InputActionReference zoomOutAction;
        [SerializeField] private float zoomOutSize = 10f;
        [SerializeField] private float zoomInSize = 5f;
        [SerializeField] private float zoomDuration = 0.5f;
        
        private Plane referencePlane = new Plane(Vector3.up, Vector3.zero);
        private Vector3 startWorldPosition;
        private Camera mainCamera;

        void Awake()
        {
            mainCamera = Camera.main;
        }

        void OnEnable()
        {
            dragAction.action.Enable();
            screenPositionAction.action.Enable();
            zoomInAction.action.Enable();
            zoomOutAction.action.Enable();
        }

        void OnDisable()
        {
            dragAction.action.Disable();
            screenPositionAction.action.Disable();
            zoomInAction.action.Disable();
            zoomOutAction.action.Disable();
        }

        Vector3 GetPressWorldPosition()
        {
            Vector2 screenPosition = screenPositionAction.action.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);

            if (referencePlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return Vector3.zero;
        }

        public void ZoomIn()
        {
            mainCamera.DOOrthoSize(zoomInSize, zoomDuration);
        }

        public void ZoomOut()
        {
            mainCamera.DOOrthoSize(zoomOutSize, zoomDuration);
        }

        void LateUpdate()
        {
            if (dragAction.action.WasPressedThisFrame())
            {
                startWorldPosition = GetPressWorldPosition();
            }

            if (dragAction.action.IsPressed())
            {
                Vector3 currentWorldPosition = GetPressWorldPosition();
                Vector3 difference = startWorldPosition - currentWorldPosition;
                transform.position += difference;
            }

            if (zoomInAction.action.WasPressedThisFrame())
            {
                ZoomIn();
            }
            if (zoomOutAction.action.WasPressedThisFrame())
            {
                ZoomOut();
            }
        }
    }
}