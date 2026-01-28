using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexGrid : MonoBehaviour
    {
        public static HexGrid Instance { get; private set; }

        [SerializeField] private HexCell _cellPrefab;
        private Dictionary<HexCoordinates, HexCell> _cells = new Dictionary<HexCoordinates, HexCell>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            CreateGrid();
        }

        private void CreateGrid()
        {
            for (int q = -GameConstants.HexGrid.GRID_RADIUS; q <= GameConstants.HexGrid.GRID_RADIUS; q++)
            {
                int rMin = Mathf.Max(-GameConstants.HexGrid.GRID_RADIUS, -q - GameConstants.HexGrid.GRID_RADIUS);
                int rMax = Mathf.Min(GameConstants.HexGrid.GRID_RADIUS, -q + GameConstants.HexGrid.GRID_RADIUS);

                for (int r = rMin; r <= rMax; r++)
                    CreateCell(q, r);
            }

            ConnectCells();
        }

        private void CreateCell(int q, int r)
        {
            HexCoordinates coordinates = new HexCoordinates(q, r);

            HexCell cell = Instantiate(_cellPrefab);

            cell.transform.SetParent(transform, false);
            cell.transform.localPosition = HexCoordinates.ToWorldPosition(coordinates);
            cell.Coordinates = coordinates;
            cell.name = "HexCell " + coordinates.ToString();

            _cells.Add(coordinates, cell);
        }

        private void ConnectCells()
        {
            foreach (KeyValuePair<HexCoordinates, HexCell> entry in _cells)
            {
                HexCell cell = entry.Value;
                HexCoordinates coords = entry.Key;

                ConnectInDirection(cell, HexDirection.SW, new HexCoordinates(coords.Q - 1, coords.R + 1));
                ConnectInDirection(cell, HexDirection.S,  new HexCoordinates(coords.Q, coords.R + 1));
                ConnectInDirection(cell, HexDirection.SE, new HexCoordinates(coords.Q + 1, coords.R));
            }
        }

        private void ConnectInDirection(HexCell cell, HexDirection direction, HexCoordinates neighborCoords)
        {
            if (!_cells.TryGetValue(neighborCoords, out HexCell _neighbor))
                return;

            cell.SetNeighbor(direction, _neighbor);
        }

        #region API
        
        public bool TryGetCell(HexCoordinates coordinates, out HexCell cell)
        {
            if(_cells.TryGetValue(coordinates, out HexCell c))
            {
                cell = c;
                return true;
            }

            cell = null;
            return false;
        }

        public bool TryGetCell(Vector3 position, out HexCell cell) 
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromWorldPosition(position);
            return TryGetCell(coordinates, out cell);
        }

        #endregion
    }
}
