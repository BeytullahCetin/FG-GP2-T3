using FormatableTextNS;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class TowerSelectionButton : SelectionButton
    {
        [SerializeField] FormatableText costText;
        public FormatableText CostText => costText;

        private TowerData towerData;
        public TowerData TowerData => towerData;
        public void SetTowerData(TowerData towerData)
        {
            this.towerData = towerData;
        }
    }
}
