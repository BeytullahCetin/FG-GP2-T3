using NaughtyAttributes.Test;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FG_GP2_T3
{
    public class StickyCameraMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InputActionReference dragAction;
        [SerializeField] private InputActionReference screenPositionAction;
        
        private Plane referencePlane = new Plane(Vector3.up, Vector3.zero);
        private Vector3 startWorldPosition;
        private Vector3 startOffsetPosition;

        void OnEnable()
        {
            dragAction.action.Enable();
            screenPositionAction.action.Enable();
        }

        void OnDisable()
        {
            dragAction.action.Disable();
            screenPositionAction.action.Disable();
        }

        Vector3 GetPressWorldPosition()
        {
            Vector2 screenPosition = screenPositionAction.action.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);

            if (referencePlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return Vector3.zero;
        }

        void LateUpdate()
        {
            if (dragAction.action.WasPressedThisFrame())
            {
                startWorldPosition = GetPressWorldPosition();
                startOffsetPosition = transform.position;
            }

            if (dragAction.action.IsPressed())
            {
                Vector3 currentWorldPosition = GetPressWorldPosition();
                Vector3 difference = currentWorldPosition - startWorldPosition;
                transform.position = startOffsetPosition - difference;
            }
        }
    }
}