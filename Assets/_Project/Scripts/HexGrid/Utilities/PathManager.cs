using System.Collections.Generic;
using System.Linq;
using UnityEditor.MemoryProfiler;
using Vector3 = UnityEngine.Vector3;

namespace FG_GP2_T3
{
    public class PathManager
    {
        public HashSet<(HexCell, HexDirection)> Connections { get; } = new();
        public HashSet<(HexCell, HexDirection)> ConnectionsWithoutCore { get; } = new();

        int _currentConnectionIndex = 0;

        public void Add((HexCell Cell, HexDirection Direction) connection)
        {
            Connections.Add(connection);

            if (!connection.Cell.IsCore)
                ConnectionsWithoutCore.Add(connection);
        }

        public void Remove((HexCell Cell, HexDirection Direction) connection)
        {
            Connections.Remove(connection);
            ConnectionsWithoutCore.Remove(connection);
        }

        public void Clear()
        {
            Connections.Clear();
            ConnectionsWithoutCore.Clear();
        }

        public (HexCell, HexDirection) GetNextConnection()
        {
            if(Connections.Count == 0) throw new System.InvalidOperationException("No connections available.");

            (HexCell, HexDirection) connection = Connections.ElementAt(_currentConnectionIndex);
            _currentConnectionIndex = (_currentConnectionIndex + 1) % Connections.Count;

            return connection;
        }

        public Vector3 GetEntrancePoint(HexCell cell, HexDirection direction) => cell.transform.localPosition + HexMetrics.GetEdgeCenter(direction);
    }
}