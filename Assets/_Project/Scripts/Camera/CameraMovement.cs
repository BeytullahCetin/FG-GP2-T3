using UnityEngine;
using UnityEngine.InputSystem;

namespace FG_GP2_T3
{
    public class CameraMovement : MonoBehaviour
    {
        [Header("1080x1920 = 1.75")]
        [Header("vivo NEX 3 5G = 0.89")]
        [Header("Speed Variables")]
        [SerializeField] private float moveSpeed = 1f;

        [Header("Input Reference")]
        [SerializeField] private InputActionReference dragAction;
        [SerializeField] private InputActionReference deltaAction;

        private float moveSpeedOffset;

        void OnEnable()
        {
            dragAction.action.Enable();
            deltaAction.action.Enable();
        }

        void OnDisable()
        {
            dragAction.action.Disable();
            deltaAction.action.Disable();
        }

        void OnValidate()
        {
            moveSpeedOffset = moveSpeed * 0.01f;
        }

        void LateUpdate()
        {
            if (dragAction.action.IsPressed())
            {
                Vector2 delta = deltaAction.action.ReadValue<Vector2>();
                if (delta != Vector2.zero)
                {
                    MoveCamera(delta);
                }
            }
        }

        void MoveCamera(Vector2 delta)
        {
            Vector3 forward = transform.up;
            Vector3 right = transform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();

            Vector3 move = (right * delta.x + (forward * delta.y * 2f)) * moveSpeedOffset;
            transform.position -= move;
        }
        
    }
}