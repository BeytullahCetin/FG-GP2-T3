using System;
using FMODUnity;
using UnityEngine;

namespace FG_GP2_T3
{
    [Serializable]
    public class GlobalSoundEmitter: MonoBehaviour
    {
        public GlobalSoundType soundType;
        private StudioEventEmitter _emitter;


        private void Start()
        {
            _emitter = GetComponent<StudioEventEmitter>();
        }


        public void Play()
        {
            _emitter.Play();
        }
    }
}
