using System;
using System.Collections.Generic;
using System.Linq;
using EPOOutline;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexTile : MonoBehaviour
    {
        public HexTileData Data { get; private set; }
        public HexCell ParentCell { get; private set; }

        private int _lastChosenRoadIndex = 0;
        private List<HexDirection> _pathsNotLeadingToCore = new();
        private List<HexDirection> _availablePaths = new();

        [SerializeField] GameObject original;
        [SerializeField] GameObject preview;

        public void SetPreview(bool value)
        {
            preview.SetActive(value);
            original.SetActive(!value);
        }

        public void Initialize(HexTileData data, HexCell parentCell)
        {
            Data = data;
            ParentCell = parentCell;

            _pathsNotLeadingToCore = Enum.GetValues(typeof(HexDirection)).Cast<HexDirection>().Where(dir => Data.HasRoad(dir)).ToList();
            UpdateAvailablePaths();
        }

        public HexDirection GetNextDirection(HexDirection incomingDirection)
        {
            if (_availablePaths.Count == 1) return _availablePaths[0];

            HexDirection chosen = _availablePaths[_lastChosenRoadIndex];
            _lastChosenRoadIndex = (_lastChosenRoadIndex + 1) % _availablePaths.Count;

            return chosen == incomingDirection ? GetNextDirection(incomingDirection) : chosen;
        }

        public void UpdateAvailablePaths()
        {
            for (int i = _pathsNotLeadingToCore.Count - 1; i >= 0; i--)
            {
                HexDirection direction = _pathsNotLeadingToCore[i];

                if (LeadsToCore(ParentCell, direction))
                {
                    _availablePaths.Add(direction);
                    _pathsNotLeadingToCore.RemoveAt(i);
                }
            }
        }

        private bool LeadsToCore(HexCell startCell, HexDirection direction)
        {
            HashSet<HexCell> visitedCells = new HashSet<HexCell> { startCell };
            HexCell neighbor = startCell.GetNeighbor(direction);

            return CheckIfPathLeadsToCore(neighbor, direction.Opposite(), visitedCells);
        }

        private static bool CheckIfPathLeadsToCore(HexCell currentCell, HexDirection incomingDirection, HashSet<HexCell> visitedCells)
        {
            if (currentCell != null && currentCell.IsCore) return true;
            if (currentCell == null || currentCell.Tile == null || visitedCells.Contains(currentCell)) return false;

            visitedCells.Add(currentCell);

            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                if (direction == incomingDirection) continue;
                if (!currentCell.Tile.Data.HasRoad(direction)) continue;

                if (CheckIfPathLeadsToCore(currentCell.GetNeighbor(direction), direction.Opposite(), visitedCells))
                    return true;
            }

            return false;
        }
    }
}