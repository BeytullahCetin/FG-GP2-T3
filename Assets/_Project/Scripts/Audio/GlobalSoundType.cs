using UnityEngine;

namespace FG_GP2_T3
{
    public enum GlobalSoundType
    {
        //// UI: 0 - 999
        ButtonDefault = 0,
        
        
        //// Tiles: 1000 - 1999
        // Base Actions: 1000 - 1099
        SelectTileBlank = 1000,
        SelectRoadDefault = 1001,
        SelectTowerDefault = 1002,
        
        PlaceRoadDefault = 1010,
        PlaceTowerDefault = 1011,
        
        // Specific Towers: 1100 - ?
        // SelectTowerJaguar = xxxx
        // PlaceTowerJaguar = xxxx
        
        
        //// Narrative: 2000 - 2999
        
        
        //// Ambience: 3000 - 3999
        // different tower calls?
    }
}
