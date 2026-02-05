using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexTowerTile : HexTile
    {
        [ReadOnly][SerializeField] private TowerBase towerBase;
        public TowerBase TowerBase => towerBase;

        public void SetTowerBase(TowerBase towerBase)
        {
            this.towerBase = towerBase;
        }
    }
}