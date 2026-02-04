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

        public TowerData SelectedTower => selectedTowerData;
        public List<HexCell> ValidCellsForSelectedTile => validCellsForSelectedTower;

        [SerializeField] StickyCameraMovement cam;
        [SerializeField] TowerConfirmation towerConfirmation;
        [SerializeField] NextWave nextWave;
        [SerializeField] HexTileData towerTile;
        [SerializeField] List<TowerData> towerDatas = new List<TowerData>();
        [SerializeField] TowerBase towerBasePrefab;

        [ReadOnly][SerializeField] TowerData selectedTowerData;
        [ReadOnly][SerializeField] HexCell selectedCell;
        [ReadOnly][SerializeField] GameObject previewParent;
        [ReadOnly][SerializeField] TowerBase previewTowerBase;
        [ReadOnly][SerializeField] List<HexCell> validCellsForSelectedTower = new List<HexCell>();

        void Awake()
        {
            nextWave.Button.onClick.AddListener(GoToNextRound);
        }

        void GoToNextRound()
        {
            // Destroy previews
            // Hide panels
            // GameManager.Instance.SwitchToEnemyWaveSubState();
            GameManager.Instance.SwitchToTileSelectionSubState();
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
            DestroyPreview();
            GameManager.Instance.SwitchToTowerSelectionSubState();
        }

        void ConfirmPlacement()
        {
            selectedCell.TrySetTile(towerTile);
            previewTowerBase.BuildTower();
            previewParent = null;

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
            DestroyPreview();

            selectedTowerData = tower;
            validCellsForSelectedTower = HexManager.Instance.GetValidCells(towerTile).Union(HexManager.Instance.GetTowerCells()).ToList();

            // TODO: Change to POE's position.
            // TODO: add do move function to camera script
            // TODO: move camera movement to state
            cam.transform.DOMove(Vector3.zero, .5f);
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        public void SetSelectedCell(HexCell cell)
        {
            selectedCell = cell;

            if (selectedCell.Tile == null)
            {
                // Creating tower on empty cell
                GameManager.Instance.SwitchToTowerPlacementConfirmationSubState();
            }
            else if (selectedCell.Tile != null)
            {
                // Fusing tower with another tower
                Debug.Log($"selectedCell.Coordinates: {selectedCell.Coordinates} - selectedCell.Tile.Data.name: {selectedCell.Tile.Data.name}");
                if (selectedCell.Tile is HexTowerTile hexTowerTile)
                {
                    // hexTowerTile 
                    //     cell fusion checks
                    //      fusion confirmation state
                    //     GameManager.Instance.SwitchToFusionConfirmationSubState();
                }

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
            DestroyPreview();
            previewParent = new GameObject("TowerParent");
            HexTowerTile tile = Instantiate(towerTile.TilePrefab, previewParent.transform).GetComponent<HexTowerTile>();
            previewTowerBase = Instantiate(towerBasePrefab, previewParent.transform);
            previewTowerBase.Data = selectedTowerData;
            previewTowerBase.UpdateTowerVisual();

            previewParent.transform.SetParent(selectedCell.transform);
            previewParent.transform.localPosition = Vector3.zero;
            tile.transform.localPosition = Vector3.zero;
            previewTowerBase.transform.localPosition = Vector3.zero;

            // TODO: Camera zoom in problem.
            // cam.ZoomIn();
            cam.transform.DOMove(selectedCell.transform.position, .5f);
        }

        public void PreviewFusionOnTower()
        {


            previewParent.transform.SetParent(selectedCell.transform);
            previewParent.transform.localPosition = Vector3.zero;
            // tile.transform.localPosition = Vector3.zero;
            previewTowerBase.transform.localPosition = Vector3.zero;

            // TODO: Camera zoom in problem.
            // cam.ZoomIn();
            cam.transform.DOMove(selectedCell.transform.position, .5f);
        }

        public void DestroyPreview()
        {
            if (previewParent != null)
                Destroy(previewParent);
        }
    }
}
