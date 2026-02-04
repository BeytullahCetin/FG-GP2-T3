using UnityEngine;

namespace FG_GP2_T3
{
    /// <summary>
    /// Template script to preview fusion validity by changing materials.
    /// </summary>
    public class FusionPreview : MonoBehaviour
    {
        public Material validMaterial;
        public Material invalidMaterial;


        private TowerBase _DraggedTower;
        private MeshRenderer _meshRenderer;

        private void Start()
        {
            _DraggedTower = GetComponent<TowerBase>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            TowerBase target=FusionDetector.GetTowerUnderMouse();
            if(target==null)return;

           bool valid=FusionAPI.CanFuse(target,_DraggedTower);

            _meshRenderer.material=valid? validMaterial : invalidMaterial;
        }
    }
}
