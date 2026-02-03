using System;
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public static class HexTileValidator
    {
        public static bool IsTileValid(HexTileData tile, HexCell cell)
        {
            bool isCoreAdjacent = false;
            int roadsToTiles = 0;
            int roadsToCore = 0;

            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                HexCell neighbor = cell.GetNeighbor(direction);

                // Road going off the map
                if (neighbor == null && tile.HasRoad(direction)) 
                    return false;

                if (neighbor == null) continue;

                if (neighbor.IsCore)
                {
                    if (tile.HasRoad(direction)) roadsToCore++;
                    isCoreAdjacent = true;
                }
                else if (neighbor.Tile != null)
                {
                    //Side type doesn't match with neighbor
                    if (neighbor.Tile.Data.HasRoad(direction.Opposite()) != tile.HasRoad(direction)) 
                        return false;

                    if (tile.HasRoad(direction)) roadsToTiles++;
                }
            }

            if (tile.RoadsCount == 0) 
                return true;

            if (isCoreAdjacent)
            {
                // Road connecting with all ends to the core creating a closed circuit
                if (roadsToCore == tile.RoadsCount) 
                    return false;

                // Road not connected to anything
                if (roadsToCore == 0 && roadsToTiles == 0) 
                    return false;
            }

            return IsCreatingClosedCircuit(tile, cell);
        }

        private static bool IsCreatingClosedCircuit(HexTileData tile, HexCell cell)
        {
            HashSet<HexCell> visitedCells = new HashSet<HexCell> { cell };

            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                if (!tile.HasRoad(direction)) continue;

                HexCell neighbor = cell.GetNeighbor(direction);
                if (IsBranchOpen(neighbor, direction.Opposite(), visitedCells))
                    return false;
            }

            return true;
        }

        private static bool IsBranchOpen(HexCell currentCell, HexDirection incomingDirection, HashSet<HexCell> visitedCells)
        {
            if (currentCell == null || currentCell.IsCore || currentCell.Tile == null) return false;
            if (visitedCells.Contains(currentCell)) return false;

            visitedCells.Add(currentCell);

            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                if (direction == incomingDirection) continue;
                if (!currentCell.Tile.Data.HasRoad(direction)) continue;

                HexCell nextNeighbor = currentCell.GetNeighbor(direction);
                if (IsBranchOpen(nextNeighbor, direction.Opposite(), visitedCells))
                    return true;
            }

            return false;
        }
    }
}