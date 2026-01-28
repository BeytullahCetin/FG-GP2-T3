using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance { get; private set; }
        
        
        private List<MusicEmitter> _musicEmitters;
        private MusicEmitter _currEmitter;
        
        
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
            _musicEmitters = gameObject.GetComponentsInChildren<MusicEmitter>().ToList();
        }


        public void OnPlaySong(MusicType type)
        {
            _currEmitter?.Stop();
            _currEmitter = _musicEmitters.Find(x => x.musicType == type);
            _currEmitter.Play();
        }
    }
}
