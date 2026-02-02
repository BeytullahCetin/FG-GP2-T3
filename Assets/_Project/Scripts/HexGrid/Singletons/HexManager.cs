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

        [SerializeField] private List<HexTile> _hexTiles = new();

        private ConnectionManager _connections = new ConnectionManager();

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
                            _connections.Add((cell, direction));
                    }   
        }

        private void RemoveInvalidTiles(List<HexTile> tiles, bool isFirstTurn)
        {
            HashSet<(HexCell, HexDirection)> connections = (isFirstTurn || _connections.ConnectionsWithoutCore.Count == 0) ? _connections.Connections : _connections.ConnectionsWithoutCore;

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
                            _connections.Remove((neighbor, direction.Opposite()));

                        if(!args.Cell.TileData.HasRoad(direction))
                            continue;

                        //Adding available connections if roads end with no adjacent tiles
                        if(neighbor != null && neighbor.Tile == null && !neighbor.IsCore)
                            _connections.Add((args.Cell, direction));
                    }
                    return;
                case CellEventType.Remove:
                    foreach(HexDirection direction in Enum.GetValues(typeof(HexDirection)))
                    {
                        //Removing all existing connections for the cell
                        _connections.Remove((args.Cell, direction));

                        //Adding available connections to neighbor cells if they have roads
                        HexCell neighbor = args.Cell.GetNeighbor(direction);
                        if(neighbor != null && neighbor.TileData != null && neighbor.TileData.HasRoad(direction.Opposite()))
                            _connections.Add((neighbor, direction.Opposite()));
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

            foreach((HexCell cell, HexDirection direction) in _connections.Connections)
            {
                HexCell neighbor = cell.GetNeighbor(direction);
                if(GetValidTileRotations(tile, neighbor).Count > 0)
                    validCells.Add(neighbor);
            }

            return validCells;
        }

        public List<float> GetValidTileRotations(HexTile tile, HexCell cell)
        {
            List<float> validRotations = new();

            HexTile tileCopy = Instantiate(tile);

            for(int i = 0; i < 6; i++)
            {
                if(HexTileValidator.IsTileValid(tileCopy, cell))
                    validRotations.Add(i * 60f);
                
                tileCopy.Roads.ShiftRight();
            }

            return validRotations;
        }

        //TODO
        /*
        public List<Vector3> GetEnemyPath()
        {
            return new List<Vector3>();
        }
        */

        //TEMP REPLACEMENT
        public List<Vector3> GetEnemyEntryPoints()
        {
            List<Vector3> entryPoints = new();

            foreach((HexCell cell, HexDirection direction) in _connections.ConnectionsWithoutCore)
                entryPoints.Add(cell.transform.localPosition + HexMetrics.GetEdgeCenter(direction) * 0.8f + Vector3.up * 0.25f); //0.9f to spawn them closer to center

            return entryPoints;
        }

        #endregion
    }
}