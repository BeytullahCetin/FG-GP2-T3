using UnityEngine;

public class HexCell : MonoBehaviour
{
    [SerializeField] GameObject _outerBorder;
    [SerializeField] GameObject _innerBorder;

    public HexCoordinates Coordinates;
    public HexTile Tile = null;
    public HexCell[] _neighbors = new HexCell[6];
    public Color OuterColor
    {
        set
        {
            _outerMeshRenderer.GetPropertyBlock(_outerPropertyBlock);
            _outerPropertyBlock.SetColor(_colorProperty, value);
            _outerMeshRenderer.SetPropertyBlock(_outerPropertyBlock);
        }
    }
    public Color InnerColor
    {
        set
        {
            _innerMeshRenderer.GetPropertyBlock(_innerPropertyBlock);
            _innerPropertyBlock.SetColor(_colorProperty, value);
            _innerMeshRenderer.SetPropertyBlock(_innerPropertyBlock);
        }
    }

    private MeshRenderer _outerMeshRenderer;
    private MeshRenderer _innerMeshRenderer;
    private MaterialPropertyBlock _outerPropertyBlock;
    private MaterialPropertyBlock _innerPropertyBlock;
    private static readonly int _colorProperty = Shader.PropertyToID("_Color");

    private void Awake()
    {
        _outerMeshRenderer = _outerBorder.GetComponent<MeshRenderer>();
        _innerMeshRenderer = _innerBorder.GetComponent<MeshRenderer>();
        _outerPropertyBlock = new MaterialPropertyBlock();
        _innerPropertyBlock = new MaterialPropertyBlock();
    }

    public void SetNeighbor(HexDirection direction, HexCell cell) 
    {
        _neighbors[(int)direction] = cell;
        cell._neighbors[(int)direction.Opposite()] = this;
    }
    public HexCell GetNeighbor(HexDirection direction) => _neighbors[(int)direction];
}