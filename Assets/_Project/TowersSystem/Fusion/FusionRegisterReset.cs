using UnityEngine;

namespace FG_GP2_T3
{
    public class FusionRegisterReset : MonoBehaviour
    {
        private void Awake()
        {
            
            FusionRegistry.Reset();
            Debug.Log("Fusion registry has been reset For new play session.");
        }
    }
}
