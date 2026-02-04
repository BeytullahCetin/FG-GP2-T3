using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexTowerTile : HexTile
    {
        private TowerData towerData;
        private TowerBase towerBase;

        public TowerData TowerData => towerData;
        public TowerBase TowerBase => towerBase;

        public void SetTowerData(TowerData towerData)
        {
            this.towerData = towerData;
        }
    }
}