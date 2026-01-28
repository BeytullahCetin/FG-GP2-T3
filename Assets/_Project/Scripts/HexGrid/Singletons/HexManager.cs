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

        [SerializeField, Expandable] private List<HexTile> _hexTiles = new();

        private HashSet<(HexCell, HexDirection)> _availableConnections = new();
        private HashSet<(HexCell, HexDirection)> _availableConnectionsWithoutCore => new HashSet<(HexCell, HexDirection)>(
            _availableConnections.Where(connection => !HexCore.CoreCoordinates.Contains(connection.Item1.Coordinates))
        );

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
            EventManager.Register<CellActionEventArgs>(OnCellAction);
        }

        private void OnDisable()
        {
            EventManager.Unregister<CellActionEventArgs>(OnCellAction);
        }

        private void SetCoreTiles()
        {
            foreach(HexCoordinates coordinate in HexCore.CoreCoordinates)
                if(HexGrid.Instance.TryGetCell(coordinate, out HexCell cell))
                    cell.SetAsCore();

            foreach(HexCoordinates coordinate in HexCore.CoreCoordinates)
                if(HexGrid.Instance.TryGetCell(coordinate, out HexCell cell))
                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        HexCell neighbor = cell.GetNeighbor(direction);
                        if(neighbor != null && !neighbor.IsCore)
                            _availableConnections.Add((cell, direction));
                    }   
        }

        private bool IsTileValid(HexTile tile, HexCell cell)
        {
            //Check if all sides match with neighbors in terms of road connections
            foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                HexCell neighbor = cell.GetNeighbor(direction);
                if(neighbor != null && !neighbor.IsCore && neighbor.TileData != null && neighbor.TileData.HasRoad(direction.Opposite()) != tile.HasRoad(direction))
                    return false;
            }

            return true;
        }

        private void RemoveInvalidTiles(List<HexTile> tiles, bool isFirstTurn)
        {
            HashSet<(HexCell, HexDirection)> connections = (isFirstTurn || _availableConnectionsWithoutCore.Count == 0) ? _availableConnections : _availableConnectionsWithoutCore;

            tiles.RemoveAll(tile => 
            {
                foreach ((HexCell cell, HexDirection direction) in isFirstTurn ? _availableConnections : _availableConnectionsWithoutCore)
                {
                    HexCell neighbor = cell.GetNeighbor(direction);

                    if (GetValidTileRotations(tile, neighbor).Count > 0)
                        return false; 
                }

                return true;
            });
        }

        private void OnCellAction(CellActionEventArgs args)
        {
            switch(args.ActionType)
            {
                case CellEventType.Place:
                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        //Removing neighbor connection points
                        HexCell neighbor = args.Cell.GetNeighbor(direction);
                        if(neighbor != null)
                            _availableConnections.Remove((neighbor, direction.Opposite()));

                        if(!args.Cell.TileData.HasRoad(direction))
                            continue;

                        //Adding available connections if roads end with no adjacent tiles
                        if(neighbor != null && neighbor.Tile == null && !neighbor.IsCore)
                            _availableConnections.Add((args.Cell, direction));
                    }
                    return;
                case CellEventType.Remove:
                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        //Removing all existing connections for the cell
                        _availableConnections.Remove((args.Cell, direction));

                        //Adding available connections to neighbor cells if they have roads
                        HexCell neighbor = args.Cell.GetNeighbor(direction);
                        if(neighbor != null && neighbor.TileData != null && neighbor.TileData.HasRoad(direction.Opposite()))
                            _availableConnections.Add((neighbor, direction.Opposite()));
                    }
                    return;
                default: return;
            }
        }

        #region API

        public List<HexTile> GetRandomValidTiles(int amount, bool isFirstTurn = false)
        {
            List<HexTile> availableTiles = new List<HexTile>(_hexTiles);

            if(isFirstTurn) availableTiles = availableTiles.Where(t => t.RoadsCount == 2).ToList();
                
            RemoveInvalidTiles(availableTiles, isFirstTurn);

            List<HexTile> result = new List<HexTile>();
            for (int i = 0; i < Mathf.Min(amount, availableTiles.Count); i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, availableTiles.Count);
                result.Add(availableTiles[randomIndex]);
                availableTiles.RemoveAt(randomIndex);
            }

            return result;
        }

        public List<HexCell> GetValidCells(HexTile tile)
        {
            List<HexCell> validCells = new();

            foreach((HexCell cell, HexDirection direction) in _availableConnections)
            {
                HexCell neighbor = cell.GetNeighbor(direction);
                if(GetValidTileRotations(tile, neighbor).Count > 0)
                    validCells.Add(cell);
            }

            return validCells;
        }

        public List<float> GetValidTileRotations(HexTile tile, HexCell cell)
        {
            List<float> validRotations = new();

            HexTile tileCopy = Instantiate(tile);

            for(int i = 0; i < 6; i++)
            {
                if(IsTileValid(tileCopy, cell))
                    validRotations.Add(i * 60f);
                
                tileCopy.Roads.ShiftRight();
            }

            return validRotations;
        }

        //TODO
        public List<Vector3> GetEnemyPath()
        {
            return new List<Vector3>();
        }

        #endregion
    }
}