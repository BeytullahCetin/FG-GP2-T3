using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TilePlacementController : MonoBehaviour
    {
        [ReadOnly][SerializeField] private HexTile currentSelectedHexTile;
        [SerializeField] List<HexTile> hexTiles = new List<HexTile>();

        private List<HexCell> validCellsForSelectedHexTile = new List<HexCell>();

        public void SetSelectedHexTile(HexTile hexTile)
        {
            currentSelectedHexTile = hexTile;
            validCellsForSelectedHexTile = HexManager.Instance.GetValidCells(currentSelectedHexTile);
        }

        public List<HexTile> GetHexTilesForPlacement()
        {
            return hexTiles;
        }

        public void StartAnimateValidCellsForPlacement()
        {
            foreach (HexCell cell in validCellsForSelectedHexTile)
            {
                cell.OuterColor = Color.white;
                cell.InnerColor = Color.white;
            }
        }
    }
}
