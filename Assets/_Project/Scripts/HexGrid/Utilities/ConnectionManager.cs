using System.Collections.Generic;

namespace FG_GP2_T3
{
    public class ConnectionManager
    {
        public HashSet<(HexCell, HexDirection)> Connections { get; } = new();
        public HashSet<(HexCell, HexDirection)> ConnectionsWithoutCore { get; } = new();

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
    }
}