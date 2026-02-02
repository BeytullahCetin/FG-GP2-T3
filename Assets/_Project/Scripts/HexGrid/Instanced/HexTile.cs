using System;
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexTile : MonoBehaviour
    {
        public HexTileData Data { get; private set; }
        
        private int _lastChosenRoadIndex = 0;

        public void Initialize(HexTileData data)
        {
            Data = data;
        }

        public HexDirection GetNextDirection(HexDirection incomingDirection)
        {
            List<HexDirection> availableOutlets = GetAvailableOutlets(incomingDirection);

            if (availableOutlets.Count == 0) throw new Exception("No available outlets found for the given incoming direction.");
            if (availableOutlets.Count == 1) return availableOutlets[0];

            HexDirection chosen = availableOutlets[_lastChosenRoadIndex];
            _lastChosenRoadIndex = (_lastChosenRoadIndex + 1) % availableOutlets.Count;
            
            return chosen;
        }

        //TODO filter roads that don't lead to the core
        private List<HexDirection> GetAvailableOutlets(HexDirection incomingDirection)
        {
            List<HexDirection> outlets = new();
            
            foreach (HexDirection direction in Enum.GetValues(typeof(HexDirection)))
            {
                if (direction == incomingDirection) continue;
                
                if (Data.HasRoad(direction))
                    outlets.Add(direction);
            }

            return outlets;
        }
    }
}