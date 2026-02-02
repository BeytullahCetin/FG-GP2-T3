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
            GameManager.Instance.SwitchToTilePlacementSubState();
        }

        public List<HexTile> GetHexTilesForPlacement()
        {
            // TODO: add select 3 random tile
            return hexTiles;
        }

        public void SetValidCellsForSelectedHexTile()
        {
            validCellsForSelectedHexTile = HexManager.Instance.GetValidCells(currentSelectedHexTile);
        }

        public void StartAnimateValidCells()
        {
            // TODO: add settings for colors
            foreach (HexCell cell in validCellsForSelectedHexTile)
            {
                cell.OuterColor = Color.white;
                cell.InnerColor = Color.white;
            }
        }

        public void StopAnimateValidCells()
        {
            // TODO: add settings for colors
            foreach (HexCell cell in validCellsForSelectedHexTile)
            {
                cell.OuterColor = Color.black;
                cell.InnerColor = Color.black;
            }
        }
    }
}
