using UnityEngine;

namespace FG_GP2_T3
{
    public enum FmodGlobalParamType
    {
        // Volume Controls: 0 - 99
        VolumeMain = 0,
        VolumeMusic = 10,
        VolumeAmbience = 20,
        VolumeSfxAuto = 30,
        VolumeSfxInteraction = 40,
        VolumeNarration = 50,
        VolumeUi = 60,
        
        // Player Stuff: 100 - 199
        PoeHealth = 100,
        
        // Enemy Stuff: 200 - 299
        EnemyCount = 200,
        
        // UI/Gameplay Stuff: 500 - ?
        IsPaused = 500,
    }
}
