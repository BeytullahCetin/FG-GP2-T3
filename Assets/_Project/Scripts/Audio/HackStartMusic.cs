using System;
using FMODUnity;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HackStartMusic : MonoBehaviour
    {
        public float volume = 0.5f;
        public StudioEventEmitter emitter;

        private void Start()
        {
            // RuntimeManager.StudioSystem.setParameterByName("Volume_Music", volume);
            
            if(!emitter.IsPlaying())
                emitter.Play();
        }
    }
}
