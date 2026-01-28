using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GlobalSoundManager : MonoBehaviour
    {
        public static GlobalSoundManager Instance { get; private set; }
        
        
        private List<GlobalSoundEmitter> _globalSoundEmitters = new();
        
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        private void Start()
        {
            _globalSoundEmitters = gameObject.GetComponentsInChildren<GlobalSoundEmitter>().ToList();
        }


        public void OnPlaySound(GlobalSoundType type)
        {
            _globalSoundEmitters.Find(x => x.soundType == type).Play();
        }
    }
}
