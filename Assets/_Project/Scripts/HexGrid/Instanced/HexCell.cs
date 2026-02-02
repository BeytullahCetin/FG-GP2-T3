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
        public GameObject Tile;
        public HexTileData TileData;
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

        public bool TrySetTile(HexTileData tile, float rotation = 0f) //Set to null to remove the tile
        {
            if(IsCore) return false;
                
            TileData = Instantiate(tile);

            if(tile == null)
            {
                if(Tile != null)
                {
                    Destroy(Tile);
                    EventManager.Invoke(new CellActionEventArgs(this, CellEventType.Remove));
                    return true;
                }
                return false;
            }
            else
            {
                if (Tile == null)
                {
                    Tile = Instantiate(tile.TilePrefab);
                    Tile.transform.SetParent(transform, false);
                    transform.Rotate(0f, rotation, 0f);
                    TileData.Roads.ShiftRight((int)(rotation / 60f));
                    EventManager.Invoke(new CellActionEventArgs(this, CellEventType.Place));
                    return true;
                }
                return false;
            }
        }

        #endregion
    }
}