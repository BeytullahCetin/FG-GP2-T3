using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TowerRangePreview : MonoBehaviour
    {
        [SerializeField] private GameObject _rangePreview;
        TowerBase towerBase;

        private MeshRenderer _innerMeshRenderer;
        private MaterialPropertyBlock _propertyBlock;

        private static readonly int _radiusProperty = Shader.PropertyToID("_Radius");
        private static readonly int _angleProperty = Shader.PropertyToID("_Angle");
        private static readonly int _rotationProperty = Shader.PropertyToID("_Rotation");

        void Awake() => Initialize();

        private void Initialize()
        {
            if (_propertyBlock != null) return;

            _innerMeshRenderer = _rangePreview.GetComponent<MeshRenderer>();
            _propertyBlock = new MaterialPropertyBlock();
            towerBase = GetComponent<TowerBase>();
        }

        private void ApplyProperties(float range, float angleDegrees, float directionDegrees)
        {
            if (_innerMeshRenderer == null) Initialize();

            _innerMeshRenderer.GetPropertyBlock(_propertyBlock);
            //_propertyBlock.SetFloat(_radiusProperty, range); Range controlled by scale
            _rangePreview.transform.localScale = Vector3.one * range;
            _propertyBlock.SetFloat(_angleProperty, angleDegrees);
            _propertyBlock.SetFloat(_rotationProperty, directionDegrees);
            _innerMeshRenderer.SetPropertyBlock(_propertyBlock);
        }

        #region API
        [SerializeField] float range;
        [SerializeField] float angleDegrees = 360;
        [SerializeField] float directionDegrees = 0;

        [Button]
        public void ShowRangeWithDI()
        {
            ApplyProperties(range, angleDegrees, directionDegrees);
            _rangePreview.SetActive(true);
        }

        [Button]
        public void ShowRange(TowerData towerToFuse = null)
        {
            float range = towerBase.Data.Range;
            if (towerToFuse != null && towerToFuse.FusionStatType == FusionStatType.RangeBoost)
                range += towerToFuse.FusionStatValue;

            ApplyProperties(range, 360, 0);
            _rangePreview.SetActive(true);
        }

        public void ShowRange(float range, float angleDegrees = 360, float directionDegrees = 0)
        {
            angleDegrees = Mathf.Clamp(angleDegrees, 0f, 360f);
            directionDegrees = (directionDegrees % 360f + 360f) % 360f;
            ApplyProperties(range, angleDegrees, directionDegrees);
            _rangePreview.SetActive(true);
        }

        [Button]
        public void HideRange()
        {
            _rangePreview.SetActive(false);
        }

        #endregion
    }
}
