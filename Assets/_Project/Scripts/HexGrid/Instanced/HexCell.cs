using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FG_GP2_T3
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField] private GameObject _outerBorder;
        [SerializeField] private GameObject _innerBorder;

        public bool IsCore { get; private set; }
        public void SetAsCore(bool isCore = true) => IsCore = isCore;

        [SerializeField] HexCell[] _neighbors = new HexCell[6];
        private MeshRenderer _outerMeshRenderer;
        private MeshRenderer _innerMeshRenderer;
        private MaterialPropertyBlock _propertyBlock;
        private static readonly int _colorProperty = Shader.PropertyToID("_Color");

        private void Awake() => Initialize();

        private void Initialize()
        {
            if (_propertyBlock != null) return;

            _outerMeshRenderer = _outerBorder.GetComponent<MeshRenderer>();
            _innerMeshRenderer = _innerBorder.GetComponent<MeshRenderer>();
            _propertyBlock = new MaterialPropertyBlock();
        }

        public void SetNeighbor(HexDirection direction, HexCell cell) 
        {
            _neighbors[(int)direction] = cell;
            cell._neighbors[(int)direction.Opposite()] = this;
        }

        public HexCell GetNeighbor(HexDirection direction) => _neighbors[(int)direction];

        #region API

        public HexCoordinates Coordinates;
        public HexTile Tile;
        public Color OuterColor
        {
            set
            {
                if (_outerMeshRenderer == null) Initialize();

                _outerMeshRenderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(_colorProperty, value);
                _outerMeshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }
        public Color InnerColor
        {
            set
            {
                if (_innerMeshRenderer == null) Initialize();

                _innerMeshRenderer.GetPropertyBlock(_propertyBlock);
                _propertyBlock.SetColor(_colorProperty, value);
                _innerMeshRenderer.SetPropertyBlock(_propertyBlock);
            }
        }

        public bool TrySetTile(HexTileData tileData, float rotation = 0f) //Set to null to remove the tile
        {
            if(IsCore) return false;

            if(tileData == null)
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

            GameObject visual = Instantiate(dataInstance.TilePrefab);
            Tile = visual.AddComponent<HexTile>();
            Tile.Initialize(dataInstance, this);
            Tile.transform.SetParent(transform, false);
            Tile.transform.localRotation = Quaternion.Euler(0f, rotation, 0f);

            if(dataInstance.RoadsCount > 0) EventManager.Invoke(new OnCellEvent(this, CellEventType.Place));
            //For tower tiles, an event will be invoked from gameplay managing script
            //else EventManager.Invoke(new OnTowerEvent(null, TowerEventType.Build));
                
            return true;
        }

        #endregion
    }
}