using UnityEngine;
using NaughtyAttributes;

namespace FG_GP2_T3
{
    public class TowerVisual : MonoBehaviour
    {
        [Expandable]
        [SerializeField] TowerData towerData;

        
        public TowerData TowerData => towerData;
    }
}
