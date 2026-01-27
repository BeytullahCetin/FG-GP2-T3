using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HexManager : MonoBehaviour
    {
        public static HexManager Instance { get; private set; }

        [SerializeField] private List<HexTile> _hexTiles = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public List<HexTile> GetRandomValidTiles(int amount, int withRoads = -1)
        {
            List<HexTile> availableTiles = new List<HexTile>(_hexTiles);

            if(withRoads >= 0)
                availableTiles = availableTiles.Where(t => t.RoadsCount == withRoads).ToList();

            //TODO Add availableTiles validation filtering here

            List<HexTile> result = new List<HexTile>();
            for (int i = 0; i < Mathf.Min(amount, availableTiles.Count); i++)
            {
                int randomIndex = Random.Range(0, availableTiles.Count);
                result.Add(availableTiles[randomIndex]);
                availableTiles.RemoveAt(randomIndex);
            }

            return result;
        }

        public List<HexCell> GetValidCells(HexTile tile)
        {
            return new List<HexCell>();
        }

        //TODO
        public List<float> GetValidTileRotations(HexTile tile, HexCell cell)
        {
            return new List<float>();
        }
    }
}
