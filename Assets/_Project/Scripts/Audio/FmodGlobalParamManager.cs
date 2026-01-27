using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

namespace FG_GP2_T3
{
    public class FmodGlobalParamManager : MonoBehaviour
    {
        public static FmodGlobalParamManager Instance { get; private set; }
        
        
        public List<FmodGlobalParam> globalParams;
        
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
            DontDestroyOnLoad(this);
        }


        public void SetParameter(FmodGlobalParamType globalParamType, float value)
        {
            string paramName = globalParams.Find(x => x.globalParamType == globalParamType).paramName;
            RuntimeManager.StudioSystem.setParameterByName(paramName, value);
        }
    }
}
