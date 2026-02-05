using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TowerPlacementController : MonoBehaviour
    {
        public TowerData SelectedTower => selectedTowerData;
        public List<HexCell> ValidCellsForSelectedTile => validCellsForSelectedTower;

        [SerializeField] StickyCameraMovement cam;
        [SerializeField] TowerConfirmation towerConfirmation;
        [SerializeField] NextWave nextWave;
        [SerializeField] HexTileData towerTile;
        [Expandable][SerializeField] List<TowerData> towerDatas = new List<TowerData>();
        [SerializeField] TowerBase towerBasePrefab;

        [ReadOnly][SerializeField] TowerData selectedTowerData;
        [ReadOnly][SerializeField] HexCell selectedCell;
        [ReadOnly][SerializeField] HexTowerTile previewTile;
        [ReadOnly][SerializeField] TowerBase previewTowerBase;
        [ReadOnly][SerializeField] TowerBase previousTowerBase;
        [ReadOnly][SerializeField] TowerBase towerBaseToFuse;
        [ReadOnly][SerializeField] List<HexCell> validCellsForSelectedTower = new List<HexCell>();

        void Awake()
        {
            nextWave.Button.onClick.AddListener(GoToNextRound);
        }

        void GoToNextRound()
        {
            // Destroy previews
            // Hide panels
            GameManager.Instance.SwitchToEnemyWaveSubState();
            // GameManager.Instance.SwitchToTileSelectionSubState();
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
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            if (previewTowerBase != null)
                Destroy(previewTowerBase.gameObject);

            GameManager.Instance.SwitchToTowerSelectionSubState();
        }

        void ConfirmPlacement()
        {
            if (previewTile != null)
            {
                Destroy(previewTile.gameObject);
            }

            selectedCell.TrySetTile(towerTile);
            selectedCell.Tile.GetComponent<HexTowerTile>().SetTowerBase(previewTowerBase);

            previewTowerBase.BuildTower();
            previewTile = null;
            previewTowerBase = null;

            GameManager.Instance.SwitchToTowerSelectionSubState();
        }

        void CancelFusion()
        {
            if (towerBaseToFuse != null)
                Destroy(towerBaseToFuse.gameObject);

            towerBaseToFuse = null;
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        void ConfirmFusion()
        {
            bool isSuccess = FusionAPI.TryFuse(previousTowerBase, towerBaseToFuse);
            if (isSuccess)
            {
                previousTowerBase = null;
                towerBaseToFuse = null;
                GameManager.Instance.SwitchToTowerSelectionSubState();
            }
            else
            {
                Debug.Log("<color=red>Fusion ERROR</color>");
            }
        }

        public void SetSelectedTower(TowerData tower)
        {
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            if (previewTowerBase != null)
                Destroy(previewTowerBase.gameObject);

            if (towerBaseToFuse != null)
                Destroy(towerBaseToFuse.gameObject);

            selectedTowerData = tower;
            validCellsForSelectedTower = HexManager.Instance.GetValidCells(towerTile).Union(HexManager.Instance.GetTowerCells()).ToList();

            // TODO: Change to POE's position.
            // TODO: add do move function to camera script
            // TODO: move camera movement to state
            cam.transform.DOMove(Vector3.zero, .5f);
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        public void SelectCellForTower(HexCell cell)
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
                    if (towerBaseToFuse != null)
                        Destroy(towerBaseToFuse.gameObject);

                    previousTowerBase = hexTowerTile.TowerBase;
                    towerBaseToFuse = Instantiate(towerBasePrefab, Vector3.up * 500f, Quaternion.identity);
                    towerBaseToFuse.Data = selectedTowerData;
                    towerBaseToFuse.BuildTower();

                    bool canFuse = FusionAPI.CanFuse(previousTowerBase, towerBaseToFuse);
                    if (canFuse == false)
                    {
                        Debug.Log("<color=red>can fuse == false</color>");
                        return;
                    }

                    GameManager.Instance.SwitchToFusionConfirmationSubState();
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
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            if (previewTowerBase != null)
                Destroy(previewTowerBase.gameObject);

            previewTile = Instantiate(towerTile.TilePrefab).GetComponent<HexTowerTile>();
            previewTowerBase = Instantiate(towerBasePrefab);
            previewTowerBase.Data = selectedTowerData;
            previewTowerBase.UpdateTowerVisual();

            previewTile.transform.position = selectedCell.transform.position;
            previewTowerBase.transform.position = selectedCell.transform.position;

            // TODO: Camera zoom in problem.
            // cam.ZoomIn();
            cam.transform.DOMove(selectedCell.transform.position, .5f);
        }

        public void PreviewFusionOnTower()
        {
            // TODO: Camera zoom in problem.
            // cam.ZoomIn();
            cam.transform.DOMove(selectedCell.transform.position, .5f);
        }
    }
}
