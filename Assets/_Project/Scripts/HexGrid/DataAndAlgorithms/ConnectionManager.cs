using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;    

namespace FG_GP2_T3
{
    public class ConnectionManager
    {
        public HashSet<(HexCell, HexDirection)> PathConnections { get; } = new();
        public HashSet<(HexCell, HexDirection)> PathConnectionsWithoutCore { get; } = new();
        public HashSet<HexCell> TowerTileConnections { get; } = new();

        int _currentConnectionIndex = 0;

        public void AddPathConnection((HexCell Cell, HexDirection Direction) connection)
        {
            Debug.Log($"Connection added {connection.Item1.Coordinates}, {connection.Item2}");

            PathConnections.Add(connection);

            if (!connection.Cell.IsCore)
            {
                HexCell neighbor = connection.Cell.GetNeighbor(connection.Direction);
                if(neighbor != null)
                    TowerTileConnections.Remove(neighbor);

                PathConnectionsWithoutCore.Add(connection);
            }            
        }

        public void AddTowerTileConnections(HexCell cell)
        {
            foreach (HexDirection direction in System.Enum.GetValues(typeof(HexDirection)))
            {
                HexCell neighbor = cell.GetNeighbor(direction);
                if (neighbor == null || neighbor.Tile != null || neighbor.IsCore) continue;

                bool isPathConnection = false;
                foreach ((HexCell, HexDirection) pathConnection in PathConnectionsWithoutCore)
                    if(pathConnection.Item1.GetNeighbor(pathConnection.Item2) == neighbor)
                    {
                        isPathConnection = true;
                        break;
                    }

                if(!isPathConnection) 
                    TowerTileConnections.Add(neighbor);
            }
        }

        public void AddTowerTileConnection(HexCell cell) => TowerTileConnections.Add(cell); 

        public void RemovePathConnection((HexCell Cell, HexDirection Direction) connection)
        {
            Debug.Log($"Connection removed {connection.Item1.Coordinates}, {connection.Item2}");
            PathConnections.Remove(connection);
            PathConnectionsWithoutCore.Remove(connection);
        }

        public void RemoveTowerTileConnection(HexCell cell) => TowerTileConnections.Remove(cell);

        public (HexCell, HexDirection) GetNextPathConnection()
        {
            if(PathConnectionsWithoutCore.Count == 0) throw new System.InvalidOperationException("No connections available.");

            _currentConnectionIndex = (_currentConnectionIndex + 1) % PathConnectionsWithoutCore.Count;
            (HexCell, HexDirection) connection = PathConnectionsWithoutCore.ElementAt(_currentConnectionIndex);
            
            return connection;
        }

        public Vector3 GetEntrancePoint(HexCell cell, HexDirection direction) => cell.transform.localPosition + HexMetrics.GetEdgeCenter(direction);
    }
}