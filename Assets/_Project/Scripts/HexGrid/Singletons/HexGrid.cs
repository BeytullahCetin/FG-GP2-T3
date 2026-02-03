using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexGrid : MonoBehaviour
    {
        public static HexGrid Instance { get; private set; }

        [SerializeField] private HexCell _cellPrefab;
        [SerializeField] private int _gridRadius = 5;
        public int GridRadius => _gridRadius;

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

        private void InitializeDictionary()
        {
            _cells.Clear();
            HexCell[] foundCells = GetComponentsInChildren<HexCell>();
            
            foreach (HexCell cell in foundCells)
                if (cell != null)
                    _cells[cell.Coordinates] = cell;
        }

        [Button("Generate Grid")]
        public void CreateGrid()
        {
            if (Application.isPlaying)
                return;

            ClearGrid();
            InitializeDictionary();

            for (int q = -_gridRadius; q <= _gridRadius; q++)
            {
                int rMin = Mathf.Max(-_gridRadius, -q - _gridRadius);
                int rMax = Mathf.Min(_gridRadius, -q + _gridRadius);

                for (int r = rMin; r <= rMax; r++)
                    CreateCell(q, r);
            }

            ConnectCells();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
#endif
            Debug.Log("Grid has been generated and saved in the scene.");
        }

        [Button("Clear Grid")]
        private void ClearGrid()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);

            _cells.Clear();
        }

        private void CreateCell(int q, int r)
        {
            HexCoordinates coordinates = new HexCoordinates(q, r);
            HexCell cell;

#if UNITY_EDITOR
            cell = (HexCell)PrefabUtility.InstantiatePrefab(_cellPrefab, transform);
#else
            cell = Instantiate(_cellPrefab, transform);
#endif

            cell.transform.localPosition = HexCoordinates.ToWorldPosition(coordinates);
            cell.Coordinates = coordinates;
            cell.name = $"HexCell {coordinates}";

            _cells[coordinates] = cell;
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

#if UNITY_EDITOR
            EditorUtility.SetDirty(cell);
#endif
        }

        #region API
        
        public bool TryGetCell(HexCoordinates coordinates, out HexCell cell)
        {
            if (_cells.Count == 0 && transform.childCount > 0)
                InitializeDictionary();

            return _cells.TryGetValue(coordinates, out cell);
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