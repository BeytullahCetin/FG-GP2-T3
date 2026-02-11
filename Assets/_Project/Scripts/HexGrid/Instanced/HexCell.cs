using System;
using System.Collections;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG_GP2_T3
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField] private GameObject _outerBorder;
        [SerializeField] private GameObject _innerBorder;

        [Header("Animation Settings")]
        [SerializeField] private AnimationCurve _placementHighlightSizeCurve;
        [SerializeField] private Gradient _placementHighlightGradient;
        [SerializeField] private AnimationCurve _fusionHighlightSizeCurve;
        [SerializeField] private Gradient _fusionHighlightGradient;

        private Coroutine _activeHighlightCoroutine;
        private MeshRenderer _outerMeshRenderer;
        private MeshRenderer _innerMeshRenderer;
        private MaterialPropertyBlock _propertyBlock;

        private static readonly int _colorProperty = Shader.PropertyToID("_Color");
        private static readonly int _sizeProperty = Shader.PropertyToID("_Size");
        private static readonly int _alphaProperty = Shader.PropertyToID("_Alpha");

        private Color _startingInnerColor;
        private float _startingInnerSize;

        public bool IsCore { get; private set; }
        public void SetAsCore(bool isCore = true) => IsCore = isCore;

        [ReadOnly][SerializeField] HexCell[] _neighbors = new HexCell[6];
        
        private void Awake() => Initialize();

        private void Initialize()
        {
            if (_propertyBlock != null) return;

            _outerMeshRenderer = _outerBorder.GetComponent<MeshRenderer>();
            _innerMeshRenderer = _innerBorder.GetComponent<MeshRenderer>();
            _propertyBlock = new MaterialPropertyBlock();

            _startingInnerColor = _innerMeshRenderer.sharedMaterial.GetColor(_colorProperty);
            _startingInnerColor.a = _innerMeshRenderer.sharedMaterial.GetFloat(_alphaProperty);
            _startingInnerSize = _innerMeshRenderer.sharedMaterial.GetFloat(_sizeProperty);
        }

        #region Highligh animations

        private void StopActiveHighlight()
        {
            if (_activeHighlightCoroutine != null)
            {
                StopCoroutine(_activeHighlightCoroutine);
                _activeHighlightCoroutine = null;
            }

            ApplyInnerProperties(_startingInnerColor, _startingInnerSize);
        }

        private IEnumerator AnimateHighlightCoroutine(AnimationCurve curve, Gradient gradient)
        {
            if (curve.length == 0) yield break;

            float duration = curve.keys[curve.length - 1].time;
            float timer = 0f;

            while (true)
            {
                timer += Time.deltaTime;
                if (timer > duration) timer = 0f;

                float normalizedTime = timer / duration;
                float sizeValue = curve.Evaluate(timer);
                Color colorValue = gradient.Evaluate(normalizedTime);

                ApplyInnerProperties(colorValue, sizeValue);

                yield return null;
            }
        }

        private void ApplyInnerProperties(Color color, float size)
        {
            if (_innerMeshRenderer == null) Initialize();

            _innerMeshRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor(_colorProperty, color);
            _propertyBlock.SetFloat(_sizeProperty, size);
            _propertyBlock.SetFloat(_alphaProperty, color.a);
            _innerMeshRenderer.SetPropertyBlock(_propertyBlock);
        }

        #endregion

        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            _neighbors[(int)direction] = cell;
            cell._neighbors[(int)direction.Opposite()] = this;
        }

        public HexCell GetNeighbor(HexDirection direction) => _neighbors[(int)direction];

        #region API

        public HexCoordinates Coordinates;
        public HexTile Tile;

        public void TogglePlacementHighlight(bool enable)
        {
            StopActiveHighlight();

            if (enable)
                _activeHighlightCoroutine = StartCoroutine(AnimateHighlightCoroutine(_placementHighlightSizeCurve, _placementHighlightGradient));
        }

        public void ToggleFusionHighlight(bool enable)
        {
            _outerBorder.transform.localPosition = _innerBorder.transform.localPosition = Vector3.zero;
            StopActiveHighlight();

            if (enable)
            {
                _outerBorder.transform.localPosition = _innerBorder.transform.localPosition = new Vector3(0f, 0f, -1.1f);
                _activeHighlightCoroutine = StartCoroutine(AnimateHighlightCoroutine(_fusionHighlightSizeCurve, _fusionHighlightGradient));
            } 
        }

        public bool TrySetTile(HexTileData tileData, float rotation = 0f) //Set to null to remove the tile
        {
            if (IsCore) return false;

            if (tileData == null)
            {
                if (Tile == null) return false;

                Destroy(Tile.gameObject);
                Tile = null;

                EventManager.Invoke(new OnCellEvent(this, CellEventType.Remove));
                return true;
            }

            if (Tile != null) return false;

            HexTileData dataInstance = Instantiate(tileData);
            dataInstance.Roads.ShiftRight(Mathf.RoundToInt(rotation / 60f));

            HexTile visual = Instantiate(dataInstance.TilePrefab);
            // Tile = visual.AddComponent<HexTile>();
            Tile = visual.GetComponent<HexTile>();
            Tile.Initialize(dataInstance, this);
            Tile.transform.SetParent(transform, false);
            Tile.transform.localRotation = Quaternion.Euler(0f, rotation, 0f);

            if (dataInstance.RoadsCount > 0) EventManager.Invoke(new OnCellEvent(this, CellEventType.Place));
            //For tower tiles, an event will be invoked from gameplay managing script
            //else EventManager.Invoke(new OnTowerEvent(null, TowerEventType.Build));

            return true;
        }

        #endregion
    }
}