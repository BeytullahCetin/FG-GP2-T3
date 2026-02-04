using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// this script is only for test purposes for checking fusions.
    /// </summary>
    public class TowerFusionTrigger_test : MonoBehaviour
    {
        public TowerBase towerBase;

        private void Awake()
        {
            towerBase = GetComponentInParent<TowerBase>();
        }
        private void OnTriggerEnter(Collider other)
        {
            TowerBase sacrificeTower= other.GetComponent<TowerBase>();

            if(sacrificeTower==null) return;
            if(sacrificeTower.HasFused|| towerBase.HasFused) return;

           GetComponent<Collider>().enabled = false;

            Debug.Log("Fusion Triggered between " + towerBase.name + " and " + sacrificeTower.name);

            TowerFusionSystem.TryFuse(towerBase, sacrificeTower);
        }
    }
}
