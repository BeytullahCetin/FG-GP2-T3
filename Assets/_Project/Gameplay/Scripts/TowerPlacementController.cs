using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TowerPlacementController : MonoBehaviour
    {
        // TODO: create a function for
        // if (previewTile != null)
        //         Destroy(previewTile);

        public TowerData SelectedTower => selectedTower;
        public List<HexCell> ValidCellsForSelectedTile => validCellsForSelectedTower;

        [SerializeField] StickyCameraMovement cam;
        [SerializeField] HexTileData towerTileBase;
        [SerializeField] List<TowerData> towerDatas = new List<TowerData>();

        [ReadOnly][SerializeField] private TowerData selectedTower;
        [ReadOnly][SerializeField] private HexCell selectedCell;
        [ReadOnly][SerializeField] private GameObject previewTower;
        private List<HexCell> validCellsForSelectedTower = new List<HexCell>();

        public void SetSelectedTower(TowerData tower)
        {
            if (previewTower != null)
                Destroy(previewTower);

            selectedTower = tower;

            validCellsForSelectedTower = HexManager.Instance.GetValidCells(towerTileBase);
            StartAnimateValidCells();

            // TODO: Change to POE's position.
            // TODO: add do move function to camera script
            // TODO: move camera movement to state
            // cam.transform.DOMove(Vector3.zero, .5f);
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        public void SetSelectedCell(HexCell cell)
        {
            selectedCell = cell;

            if (selectedCell.Tile == null)
            {
                // placement confirmation state
                GameManager.Instance.SwitchToTowerPlacementConfirmationSubState();
            }
            else if (selectedCell.Tile != null)
            {
                // cell fusion checks
                // fusion confirmation state
                GameManager.Instance.SwitchToFusionConfirmationSubState();
            }
        }

        public List<TowerData> GetTowerDatasForPlacement()
        {
            // TODO: add select 3 random tile
            towerDatas.Shuffle();
            return towerDatas.Take(3).ToList();
        }

        public void StartAnimateValidCells()
        {
            // TODO: add settings for colors
            foreach (HexCell cell in validCellsForSelectedTower)
            {
                cell.OuterColor = Color.white;
                cell.InnerColor = Color.white;
            }
        }

        // public void StopAnimateValidCells()
        // {
        //     // TODO: add settings for colors
        //     foreach (HexCell cell in validCellsForSelectedTile)
        //     {
        //         cell.OuterColor = Color.black;
        //         cell.InnerColor = Color.black;
        //     }
        // }

        public void PreviewTowerOnTheCell()
        {
            if (previewTower != null)
                Destroy(previewTower);

            //     validRotationsForSelectedTile = HexManager.Instance.GetValidTileRotations(selectedTile, selectedCell);
            //     currentTileRotationIndex = 0;

            //     previewTile = Instantiate(selectedTile.TilePrefab);
            //     previewTile.transform.SetParent(selectedCell.transform);
            //     previewTile.transform.localPosition = Vector3.zero;
            //     previewTile.transform.rotation = Quaternion.Euler(0, validRotationsForSelectedTile[currentTileRotationIndex], 0);

            //     // TODO: Camera zoom in problem.
            //     // cam.ZoomIn();
            //     cam.transform.DOMove(selectedCell.transform.position, .5f);
        }
    }
}
