using UnityEngine;

namespace FG_GP2_T3
{
    public enum GlobalSoundType
    {
        //// Default for when a value is unset
        Default = 0,
        
        //// UI: 1 - 999
        ButtonDefault = 1,
        
        TapToPlay = 10,
        RestartGame = 11,
        StartRound = 12,
        
        ChangeSpeed = 20,
        
        TowerMenuOpen = 30,
        TowerMenuClose = 31,
        TowerInfoExpand = 35,
        TowerInfoCollapse = 36,
        
        
        //// Tiles: 1000 - 1999
        // Base Actions: 1000 - 1099
        //SelectTileBlank = 1000,
        SelectRoadDefault = 1001,
        SelectTowerDefault = 1002,
        RotateTile = 1003,
        CancelPlaceTile = 1004,
        PreviewPlaceTile = 1005,
        
        PlaceRoadDefault = 1010,
        PlaceTowerDefault = 1011,
        
        // Specific Towers: 1100 - ?
        SelectTowerJaguar = 1100,
        PlaceTowerJaguar = 1101,
        
        SelectTowerHowler = 1105,
        PlaceTowerHowler = 1106,
        
        SelectTowerBamboo = 1110,
        PlaceTowerBamboo = 1111,
        
        SelectTowerBrazil = 1115,
        PlaceTowerBrazil = 1116,
        
        SelectTowerTrumpet = 1120,
        PlaceTowerTrumpet = 1121,
        
        SelectTowerHarpy = 1125,
        PlaceTowerHarpy = 1126,
        
        SelectTowerMacaw = 1130,
        PlaceTowerMacaw = 1131,
        
        SelectTower8 = 1135,
        PlaceTower8 = 1136,
        
        
        //// Narrative: 2000 - 2999
        PoeLowHealth = 2000,
        
        WaveStart = 2010,
        WaveEnd = 2011,
        
        GameWin = 2020,
        GameLose = 2021,
        
        
        //// Ambience: 3000 - 3999
        // different tower calls?
    }
}
