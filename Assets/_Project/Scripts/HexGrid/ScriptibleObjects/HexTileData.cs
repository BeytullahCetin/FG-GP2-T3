using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    [CreateAssetMenu(fileName = "HexTileData", menuName = "Scriptable Objects/HexTileData")]
    public class HexTileData : ScriptableObject
    {
        public Sprite TileIcon;
        public HexTile TilePrefab;
        public HexRoadSet Roads;
        public bool HasRoad(HexDirection direction) => Roads.HasRoad(direction);
        public int RoadsCount => Roads.CountRoads();
    }

    [System.Serializable]
    public class HexRoadSet //Made mostly for the property drawer
    {
        [SerializeField] private bool[] _roads = new bool[6];

        public bool this[int i]
        {
            get => _roads[i];
            set => _roads[i] = value;
        }

        public bool HasRoad(HexDirection dir) => _roads[(int)dir];

        public int CountRoads()
        {
            int count = 0;
            for (int i = 0; i < 6; i++)
                if (_roads[i]) count++;

            return count;
        }

        public void ShiftRight(int repetitions = 1)
        {
            if(repetitions <= 0)
                return;

            bool[] newRoads = new bool[6];
            for(int j = 0; j < 6; j++)
                newRoads[(j + repetitions) % 6] = _roads[j];

            _roads = newRoads;
        }
    }
}