using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FG_GP2_T3
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public InputSystem Controls { get; private set; }

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Controls = new InputSystem();
        }

        void OnEnable()
        {
            if (Controls == null) return;
            
            Controls.Enable();
        }

        void OnDisable()
        {
            if (Controls == null) return;

            Controls.Disable();
        }
    }
}
