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
        [SerializeField] TowerConfirmation towerConfirmation;
        [SerializeField] NextWave nextWave;
        [SerializeField] HexTileData towerTile;
        [SerializeField] List<TowerData> towerDatas = new List<TowerData>();

        [ReadOnly][SerializeField] TowerData selectedTower;
        [ReadOnly][SerializeField] HexCell selectedCell;
        [ReadOnly][SerializeField] GameObject previewTower;
        List<HexCell> validCellsForSelectedTower = new List<HexCell>();

        void Awake()
        {
            nextWave.Button.onClick.AddListener(GoToNextRound);
        }

        void GoToNextRound()
        {
            // Destroy previews
            // Hide panels
        }

        void RemoveAllLisenersFromConfirmationButtons()
        {
            towerConfirmation.CancelButton.onClick.RemoveAllListeners();
            towerConfirmation.ConfirmButton.onClick.RemoveAllListeners();
        }

        public void SetPlacementListeners()
        {
            RemoveAllLisenersFromConfirmationButtons();
            towerConfirmation.CancelButton.onClick.AddListener(CancelPlacement);
            towerConfirmation.ConfirmButton.onClick.AddListener(ConfirmPlacement);
        }

        public void SetFusionListeners()
        {
            RemoveAllLisenersFromConfirmationButtons();
            towerConfirmation.CancelButton.onClick.AddListener(CancelFusion);
            towerConfirmation.ConfirmButton.onClick.AddListener(ConfirmFusion);
        }

        void CancelPlacement()
        {
            if (previewTower != null)
                Destroy(previewTower);

            GameManager.Instance.SwitchToTowerSelectionSubState();
        }

        void ConfirmPlacement()
        {
            selectedCell.TrySetTile(towerTile);
            // NavmeshManager.Instance.RebakeNavmesh();
            // Destroy(previewTower.gameObject);
            previewTower = null;
            GameManager.Instance.SwitchToTowerSelectionSubState();
        }

        void CancelFusion()
        {
            //     if (previewTile != null)
            //         Destroy(previewTile);

            //     GameManager.Instance.SwitchToTileSelectionSubState();
        }

        void ConfirmFusion()
        {
            //     if (selectedCell.TrySetTile(selectedTile, validRotationsForSelectedTile[currentTileRotationIndex]))
            //     {
            //         NavmeshManager.Instance.RebakeNavmesh();
            //         Destroy(previewTile.gameObject);
            //     }

            //     previewTile = null;
            //     GameManager.Instance.SwitchToTileToTowerTransitionSubState();
        }

        public void SetSelectedTower(TowerData tower)
        {
            if (previewTower != null)
                Destroy(previewTower);

            selectedTower = tower;
            validCellsForSelectedTower = HexManager.Instance.GetValidCells(towerTile);
            // TODO: Change to POE's position.
            // TODO: add do move function to camera script
            // TODO: move camera movement to state
            cam.transform.DOMove(Vector3.zero, .5f);
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        public void SetSelectedCell(HexCell cell)
        {
            selectedCell = cell;

            // Creating tower on empty cell
            if (selectedCell.Tile == null)
            {
                GameManager.Instance.SwitchToTowerPlacementConfirmationSubState();
            }

            // Fusing tower with another tower
            // else if (selectedCell.Tile != null)
            // {
            //     // cell fusion checks
            //     // fusion confirmation state
            //     GameManager.Instance.SwitchToFusionConfirmationSubState();
            // }
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

        public void StopAnimateValidCells()
        {
            // TODO: add settings for colors
            foreach (HexCell cell in validCellsForSelectedTower)
            {
                cell.OuterColor = Color.black;
                cell.InnerColor = Color.black;
            }
        }

        public void PreviewTowerOnEmptyCell()
        {
            if (previewTower != null)
                Destroy(previewTower);

            //     validRotationsForSelectedTile = HexManager.Instance.GetValidTileRotations(selectedTile, selectedCell);
            //     currentTileRotationIndex = 0;

            previewTower = new GameObject("Tower");
            GameObject tile = Instantiate(towerTile.TilePrefab, previewTower.transform);
            // GameObject tower = Instantiate(selectedTower, previewTower.transform);

            previewTower.transform.SetParent(selectedCell.transform);
            previewTower.transform.localPosition = Vector3.zero;

            // TODO: Camera zoom in problem.
            // cam.ZoomIn();
            cam.transform.DOMove(selectedCell.transform.position, .5f);
        }
    }
}
