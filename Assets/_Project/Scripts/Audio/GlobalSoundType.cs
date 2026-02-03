using UnityEngine;

namespace FG_GP2_T3
{
    public enum GlobalSoundType
    {
        //// Default for when a value is unset
        Default = 0,
        
        //// UI: 1 - 999
        ButtonDefault = 1,
        
        
        //// Tiles: 1000 - 1999
        // Base Actions: 1000 - 1099
        SelectTileBlank = 1000,
        SelectRoadDefault = 1001,
        SelectTowerDefault = 1002,
        
        PlaceRoadDefault = 1010,
        PlaceTowerDefault = 1011,
        
        // Specific Towers: 1100 - ?
        SelectTowerJaguar = 1100,
        PlaceTowerJaguar = 1102,
        
        SelectTowerHowler = 1105,
        PlaceTowerHowler = 1106,
        
        SelectTowerBamboo = 1110,
        PlaceTowerBamboo = 1111,
        
        SelectTower4 = 1115,
        PlaceTower4 = 1116,
        
        SelectTower5 = 1120,
        PlaceTower5 = 1121,
        
        SelectTower6 = 1125,
        PlaceTower6 = 1126,
        
        SelectTower7 = 1130,
        PlaceTower7 = 1131,
        
        SelectTower8 = 1135,
        PlaceTower8 = 1136,
        
        
        //// Narrative: 2000 - 2999
        
        
        //// Ambience: 3000 - 3999
        // different tower calls?
    }
}
