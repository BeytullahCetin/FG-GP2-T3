using System;
using FMODUnity;
using UnityEngine;

namespace FG_GP2_T3
{
    [Serializable]
    public class MusicEmitter: MonoBehaviour
    {
        public MusicType musicType;
        private StudioEventEmitter _emitter;


        private void Start()
        {
            _emitter = GetComponent<StudioEventEmitter>();
        }


        public void Play()
        {
            _emitter.Play();
        }

        public void Stop()
        {
            _emitter.SetParameter("Stop", 1);
        }
    }
}
