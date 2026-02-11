using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FG_GP2_T3
{
    public class PoeDamageFeedback : MonoBehaviour
    {
        public List<GameObject> branches = new();

        
        private float _dmgThresh;
        private float _nextDmgFeedback;


        private void Start()
        {
            _dmgThresh = 1.0f / branches.Count;
            _nextDmgFeedback = 1.0f - _dmgThresh;
        }

        private void OnEnable()
        {
            EventManager.Register<OnCoreDamageEvent>(OnCoreDamageEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnCoreDamageEvent>(OnCoreDamageEvent);
        }
        
        
        private void OnCoreDamageEvent(OnCoreDamageEvent args)
        {
            float health = args.CurrentHealth01;
            //Debug.Log(health);
            if (health > _nextDmgFeedback)
                return;

            _nextDmgFeedback -= _dmgThresh;
            
            int branchIndex = Random.Range(0, branches.Count);
            GameObject branch = branches[branchIndex];
            branches.RemoveAt(branchIndex);
            
            EventManager.Invoke(new OnBranchLostEvent(branch));
        }
        
    }
}
