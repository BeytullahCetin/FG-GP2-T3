using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexManager : MonoBehaviour
    {
        public static HexManager Instance { get; private set; }

        [SerializeField] private List<HexTileData> _hexTiles = new();
        
        private ConnectionManager _connections = new ConnectionManager();
        private List<HexCell> _towerCells = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start() => SetCoreTiles();

        private void OnEnable()
        {
            EventManager.Register<OnCellEvent>(OnCellEvent);
        }

        private void OnDisable()
        {
            EventManager.Unregister<OnCellEvent>(OnCellEvent);
        }

        private void SetCoreTiles()
        {
            foreach(HexCoordinates coordinate in HexCore.CoreCoordinates)
                if(HexGrid.Instance.TryGetCell(coordinate, out HexCell cell))
                    cell.SetAsCore();

            foreach(HexCoordinates coordinate in HexCore.CoreCoordinates)
                if(HexGrid.Instance.TryGetCell(coordinate, out HexCell cell))
                {
                    _connections.AddTowerTileConnections(cell);
                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        HexCell neighbor = cell.GetNeighbor(direction);
                        if(neighbor != null && !neighbor.IsCore)
                        {
                            _connections.AddPathConnection((cell, direction));
                        }                 
                    }  
                } 
        }

        private void RemoveInvalidTiles(List<HexTileData> tiles, bool isFirstTurn)
        {
            HashSet<(HexCell, HexDirection)> connections = (isFirstTurn || _connections.PathConnectionsWithoutCore.Count == 0) ? _connections.PathConnections : _connections.PathConnectionsWithoutCore;

            tiles.RemoveAll(tile => 
            {
                foreach ((HexCell cell, HexDirection direction) in connections)
                {
                    HexCell neighbor = cell.GetNeighbor(direction);

                    if (GetValidTileRotations(tile, neighbor).Count > 0)
                        return false; 
                }

                return true;
            });
        }

        private void OnCellEvent(OnCellEvent args)
        {
            switch(args.EventType)
            {
                case CellEventType.Place:
                    if(args.Cell.Tile.Data.RoadsCount == 0)
                    {
                        _towerCells.Add(args.Cell);
                        _connections.RemoveTowerTileConnection(args.Cell);
                        return;
                    }

                    _connections.AddTowerTileConnections(args.Cell);

                    int tileNeighbours = 0;
                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        //Removing neighbor connection points
                        HexCell neighbor = args.Cell.GetNeighbor(direction);
                        if(neighbor != null)
                            _connections.RemovePathConnection((neighbor, direction.Opposite()));

                        if(!args.Cell.Tile.Data.HasRoad(direction))
                            continue;

                        //Adding available connections if roads end with no adjacent tiles
                        if(neighbor != null)
                        {
                            tileNeighbours++;
                            if(neighbor.Tile == null && !neighbor.IsCore)
                                _connections.AddPathConnection((args.Cell, direction));
                        }
                    }
                    if(tileNeighbours > 1)
                        UpdateTilesAvailablePathsInBranch(args.Cell);
                    return;
                case CellEventType.Remove:
                    if(args.Cell.Tile.Data.RoadsCount == 0)
                    {
                        _towerCells.Remove(args.Cell);
                        _connections.AddTowerTileConnection(args.Cell);
                        return;
                    }

                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        //Removing all existing connections for the cell
                        _connections.RemovePathConnection((args.Cell, direction));

                        //Adding available connections to neighbor cells if they have roads
                        HexCell neighbor = args.Cell.GetNeighbor(direction);
                        if(neighbor != null && neighbor.Tile != null && neighbor.Tile.Data.HasRoad(direction.Opposite()))
                            _connections.AddPathConnection((neighbor, direction.Opposite()));
                    }
                    return;
                default: return;
            }
        }

        private void UpdateTilesAvailablePathsInBranch(HexCell startCell)
        {
            if (startCell == null || startCell.Tile == null) return;

            Stack<HexCell> cellsToProcess = new Stack<HexCell>();
            HashSet<HexCell> visitedCells = new HashSet<HexCell>();

            cellsToProcess.Push(startCell);

            while (cellsToProcess.Count > 0)
            {
                HexCell current = cellsToProcess.Pop();

                if (visitedCells.Contains(current)) continue;
                visitedCells.Add(current);

                if (current.Tile != null)
                    current.Tile.UpdateAvailablePaths();

                foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                {
                    if (current.Tile != null && !current.Tile.Data.HasRoad(direction)) continue;
                    if (current.IsCore) continue;

                    HexCell neighbor = current.GetNeighbor(direction);

                    if (neighbor != null && neighbor.Tile != null)
                        cellsToProcess.Push(neighbor);
                }
            }
        }

        #region API

        public List<HexTileData> GetRandomValidTiles(int amount, bool isFirstTurn = false)
        {
            List<HexTileData> availableTiles = new List<HexTileData>(_hexTiles);

            if(isFirstTurn) availableTiles = availableTiles.Where(t => t.RoadsCount == 2).ToList();
                
            RemoveInvalidTiles(availableTiles, isFirstTurn);

            List<HexTileData> result = new List<HexTileData>();
            for (int i = 0; i < Mathf.Min(amount, availableTiles.Count); i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableTiles.Count);
                result.Add(availableTiles[randomIndex]);
                availableTiles.RemoveAt(randomIndex);
            }

            return result;
        }

        public List<HexCell> GetValidCells(HexTileData tile)
        {
            if(tile.RoadsCount == 0) return _connections.TowerTileConnections.ToList();

            List<HexCell> validCells = new();

            foreach((HexCell cell, HexDirection direction) in _connections.PathConnections)
            {
                HexCell neighbor = cell.GetNeighbor(direction);
                if(GetValidTileRotations(tile, neighbor).Count > 0)
                    validCells.Add(neighbor);
            }

            return validCells;
        }

        public List<float> GetValidTileRotations(HexTileData tile, HexCell cell)
        {
            List<float> validRotations = new();

            HexTileData tileCopy = Instantiate(tile);

            for(int i = 0; i < 6; i++)
            {
                if(HexTileValidator.IsTileValid(tileCopy, cell))
                    validRotations.Add(i * 60f);
                
                tileCopy.Roads.ShiftRight();
            }

            return validRotations;
        }

        public List<HexCell> GetTowerCells() => _towerCells;

        public List<Vector3> GetNextEnemyPath()
        {
            (HexCell, HexDirection) connection = _connections.GetNextPathConnection();
            Vector3 start = _connections.GetEntrancePoint(connection.Item1, connection.Item2);
            List<Vector3> path = new List<Vector3> { start };

            int safetyCounter = 0;

            HexCell currentCell = connection.Item1;
            HexDirection movementDirection = currentCell.Tile.GetNextDirection(connection.Item2);

            while(true)
            {
                if (currentCell.IsCore) break;

                path.Add(currentCell.transform.position);

                movementDirection = currentCell.Tile.GetNextDirection(movementDirection.Opposite());
                currentCell = currentCell.GetNeighbor(movementDirection);

                safetyCounter++;
                if(safetyCounter > 1000)
                    throw new Exception("Infinite loop detected in enemy pathfinding. Could not find a path to the core.");
            }

            return path;
        }

        //LEGACY
        public List<Vector3> GetEnemyEntryPoints()
        {
            List<Vector3> entryPoints = new();

            foreach((HexCell cell, HexDirection direction) in _connections.PathConnectionsWithoutCore)
                entryPoints.Add(_connections.GetEntrancePoint(cell, direction));

            return entryPoints;
        }

        #endregion
    }
}