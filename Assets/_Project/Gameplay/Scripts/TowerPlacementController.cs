using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TowerPlacementController : MonoBehaviour
    {
        public TowerData SelectedTower => selectedTowerData;
        public List<HexCell> ValidCellsForSelectedTile => validCellsForSelectedTower;

        [SerializeField] int rerollCost = 25;

        [SerializeField] StickyCameraMovement cam;
        [SerializeField] TowerConfirmation towerConfirmation;
        [SerializeField] NextWave nextWave;
        [SerializeField] HexTileData towerTile;
        [Expandable][SerializeField] List<TowerData> towerDatas = new List<TowerData>();
        [SerializeField] TowerBase towerBasePrefab;
        [SerializeField] TowerRerollButton rerollButton;

        [ReadOnly][SerializeField] TowerData selectedTowerData;
        [ReadOnly][SerializeField] HexCell selectedCell;
        [ReadOnly][SerializeField] HexTowerTile previewTile;
        [ReadOnly][SerializeField] TowerBase previewTowerBase;
        [ReadOnly][SerializeField] TowerBase previousTowerBase;
        [ReadOnly][SerializeField] TowerBase towerBaseToFuse;
        [ReadOnly][SerializeField] List<HexCell> validCellsForSelectedTower = new List<HexCell>();
        [ReadOnly][SerializeField] int rerollCount = 0;

        private int GetRerollCost()
        {
            return Mathf.RoundToInt(rerollCost * Mathf.Pow(2, rerollCount));
        }

        void Awake()
        {
            nextWave.Button.onClick.AddListener(GoToNextRound);
            rerollButton.Button.onClick.AddListener(RerollTowerSelection);
        }

        public void UpdateRerollButton()
        {
            rerollButton.UpdateRerollButton(GetRerollCost());
        }

        void GoToNextRound()
        {
            // Destroy previews
            // Hide panels

            if (previewTile != null)
                Destroy(previewTile.gameObject);

            if (previewTowerBase != null)
                Destroy(previewTowerBase.gameObject);

            if (towerBaseToFuse != null)
                Destroy(towerBaseToFuse.gameObject);

            cam.ZoomOut(null, .5f);
            cam.MoveTo(EnemyManager.Instance.GetTarget().transform.position, .5f);
            StopAnimateValidCells();
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
            previewTowerBase.TowerRangePreview.HideRange();

            if (previewTile != null)
                Destroy(previewTile.gameObject);

            if (previewTowerBase != null)
                Destroy(previewTowerBase.gameObject);

            cam.ZoomOut(null, .5f);
            StartAnimateValidCells();
            EventManager.Invoke(new OnCellEvent(selectedCell, CellEventType.Cancel));
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        void ConfirmPlacement()
        {
            if (previewTile != null)
            {
                Destroy(previewTile.gameObject);
            }

            selectedCell.TrySetTile(towerTile);
            selectedCell.Tile.GetComponent<HexTowerTile>().SetTowerBase(previewTowerBase);

            EventManager.Invoke(new OnTowerEvent(selectedCell.Tile.GetComponent<HexTowerTile>().TowerBase, TowerEventType.Build));

            CompostManager.Instance.UseCompost(selectedTowerData.Cost);
            UIManager.Instance.GameplayUI.UpdateTowerSelectionButtons();
            UIManager.Instance.GameplayUI.DeselectTowerSelectionButtons();
            previewTowerBase.BuildTower();
            previewTowerBase.TowerRangePreview.HideRange();
            previewTile = null;
            previewTowerBase = null;
            selectedTowerData = null;

            cam.ZoomOut(null, .5f);
            validCellsForSelectedTower.Clear();
            GameManager.Instance.SwitchToTowerSelectionSubState();
        }

        void CancelFusion()
        {
            previousTowerBase.TowerRangePreview.HideRange();

            if (towerBaseToFuse != null)
                Destroy(towerBaseToFuse.gameObject);

            towerBaseToFuse = null;
            StartAnimateValidCells();
            UIManager.Instance.GameplayUI.TowerInfoUI.SetTowerInfo(selectedTowerData);
            cam.ZoomOut(null, .5f);
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        void ConfirmFusion()
        {
            bool isSuccess = FusionAPI.TryFuse(previousTowerBase, towerBaseToFuse);
            if (isSuccess)
            {
                CompostManager.Instance.UseCompost(selectedTowerData.Cost);
                UIManager.Instance.GameplayUI.UpdateTowerSelectionButtons();
                UIManager.Instance.GameplayUI.DeselectTowerSelectionButtons();
                EventManager.Invoke(new OnTowerEvent(previousTowerBase, TowerEventType.Fuse));

                previousTowerBase.TowerRangePreview.HideRange();
                //previousTowerBase = null;
                towerBaseToFuse = null;
                cam.ZoomOut(null, .5f);
                validCellsForSelectedTower.Clear();
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

            if (previousTowerBase != null)
                previousTowerBase.TowerRangePreview.HideRange();

            selectedTowerData = tower;
            StopAnimateValidCells();
            validCellsForSelectedTower = HexManager.Instance.GetValidCells(towerTile).Union(HexManager.Instance.GetTowerCells()).ToList();
            StartAnimateValidCells();
            EventManager.Invoke(new OnUITowerEvent(selectedTowerData, UIEventType.Open));

            UIManager.Instance.GameplayUI.TowerInfoUI.Shrink();
            cam.ZoomOut(null, .5f);
            GameManager.Instance.SwitchToTowerPlacementSubState();
        }

        public void SelectCellForTower(HexCell cell)
        {
            selectedCell = cell;

            // TODO: Change these events location.
            // Even if the cell couldn't clickable or tower already fused, 
            // events are still triggering.
            EventManager.Invoke(new OnUITowerEvent(selectedTowerData, UIEventType.Close));
            EventManager.Invoke(new OnCellEvent(cell, CellEventType.Click));

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
            return towerDatas;
            List<TowerData> towersForPlacement = new List<TowerData>();
            List<TowerData> otherTowers = new List<TowerData>();

            List<TowerData> affordableTowers = towerDatas.Where(x => x.Cost <= CompostManager.Instance.CurrentCompostAmount).ToList();
            if (affordableTowers.Count() > 0)
            {
                affordableTowers.Shuffle();
                towersForPlacement.Add(affordableTowers[0]);

                otherTowers = towerDatas.Except(towersForPlacement).ToList();
                otherTowers.Shuffle();

                towersForPlacement.AddRange(otherTowers.Take(2));

                string affordableTowersString = "";
                affordableTowers.ForEach(x => affordableTowersString += x.name + " - ");
                Debug.Log($"<color=green>Affordable towers detected: {affordableTowersString}</color>");
            }
            else
            {
                Debug.Log($"<color=red>No affordable towers!</color>");
                towerDatas.Shuffle();
                towersForPlacement = towerDatas.Take(3).ToList();
            }

            return towersForPlacement.OrderBy(x => x.Cost).ToList();
        }

        public void StartAnimateValidCells()
        {
            foreach (HexCell cell in validCellsForSelectedTower)
                cell.TogglePlacementHighlight(true);
        }

        public void StopAnimateValidCells()
        {
            foreach (HexCell cell in validCellsForSelectedTower)
                cell.TogglePlacementHighlight(false);
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
            previewTowerBase.TowerRangePreview.ShowRange();

            previewTile.transform.position = selectedCell.transform.position;
            previewTowerBase.transform.position = selectedCell.transform.position;

            StopAnimateValidCells();
            cam.ZoomIn(null, .5f);
            cam.MoveTo(selectedCell.transform.position, .5f);
        }

        public void PreviewFusionOnTower()
        {
            StopAnimateValidCells();
            cam.ZoomIn(null, .5f);
            cam.MoveTo(selectedCell.transform.position, .5f);
            previousTowerBase.TowerRangePreview.ShowRange(selectedTowerData);
            UIManager.Instance.GameplayUI.TowerInfoUI.SetTowerFusionInfo(previousTowerBase.Data, selectedTowerData);
            UIManager.Instance.GameplayUI.TowerInfoUI.Show();
        }

        public void ListenSelectCellClicks()
        {
            if (Input.GetMouseButtonDown(0) == false)
                return;

            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(inputRay, out RaycastHit hit);

            if (isHit == false)
                return;

            bool hasGetCell = HexGrid.Instance.TryGetCell(hit.point, out HexCell cell);
            if (hasGetCell == false)
                return;

            if (SelectedTower == null && cell.Tile is HexTowerTile hexTowerTile)
            {
                TowerBase towerBase = hexTowerTile.TowerBase;
                towerBase.TowerRangePreview.ShowRange();
                UIManager.Instance.GameplayUI.TowerInfoUI.SetTowerInfo(towerBase.Data, towerBase);
                UIManager.Instance.GameplayUI.TowerInfoUI.Show();
                cam.ZoomIn(null, .5f);
                cam.MoveTo(cell.transform.position, .5f);
                return;
            }

            if (ValidCellsForSelectedTile.Contains(cell) == false)
                return;

            SelectCellForTower(cell);
        }

        public void RerollTowerSelection()
        {
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            if (previewTowerBase != null)
                Destroy(previewTowerBase.gameObject);

            if (towerBaseToFuse != null)
                Destroy(towerBaseToFuse.gameObject);

            previewTile = null;
            previewTowerBase = null;
            towerBaseToFuse = null;
            selectedCell = null;
            selectedTowerData = null;

            CompostManager.Instance.UseCompost(GetRerollCost());
            UIManager.Instance.GameplayUI.ResetTowerSelectionButtons();
            GameManager.Instance.SwitchToTowerSelectionSubState();

            rerollCount++;
            UpdateRerollButton();
            cam.ZoomOut(null, .5f);
            cam.MoveTo(EnemyManager.Instance.GetTarget().transform.position, .5f);
        }
    }
}
