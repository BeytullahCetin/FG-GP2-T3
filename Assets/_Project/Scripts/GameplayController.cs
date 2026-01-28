using System;
using System.Collections.Generic;
using NaughtyAttributes;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FG_GP2_T3
{
    public enum GameplayState
    {
        TilePlacement,
        TowerPlacement,
        EnemyAttack
    }

    public class GameplayController : MonoBehaviour
    {
        [SerializeField] GameplayUI gameplayUI;
        private GameplayState currentGameState;

        [Header("Tile Placement")]
        [Expandable][SerializeField] private List<HexTile> _tiles = new List<HexTile>();
        [ReadOnly][SerializeField] private HexTile _currentSelectedTile;

        [SerializeField] private GameObject previewTile;
        [SerializeField] private HexCell selectedCell;
        [SerializeField] private List<HexCell> validCellsForSelectedHexTile;
        [SerializeField] private List<float> validTileRotationsForSelectedHexTile;
        private int currentValidRotationIndex = 0;

        [Header("Tower Placement")]
        [SerializeField] List<TowerData> towerDatas = new List<TowerData>();
        [SerializeField] private TowerData selectedTowerData;
        [SerializeField] private TowerBase previewTower;

        void Awake()
        {
            currentGameState = GameplayState.TilePlacement;
            Initialize();
        }

        private void Update()
        {
            if (currentGameState == GameplayState.TilePlacement)
            {
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    HandleInput();
            }
            else if (currentGameState == GameplayState.TowerPlacement)
            {
                TowerPlacementUpdate();
            }
        }

        private void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out RaycastHit hit) && HexGrid.Instance.TryGetCell(hit.point, out HexCell cell))
            {
                if (cell.Tile == null && _currentSelectedTile != null)
                {
                    selectedCell = cell;
                    PreviewTileOnTheSelectedCell();
                }
            }
        }

        private void TowerPlacementUpdate()
        {

        }

        private void Initialize()
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                HexTile hexTile = _tiles[i];
                SelectionButton tileButton = gameplayUI.TileButtons[i];
                tileButton.Title.SetText(hexTile.name);

                tileButton.Button.onClick.AddListener(() =>
                {
                    Debug.Log($"tileButtonClicked - {hexTile.name}");
                    StopAnimateValidCellsForSelectedHexTile();
                    SelectHexTile(hexTile);
                    StartAnimateValidCellsForSelectedHexTile();
                });
            }

            gameplayUI.BtnNextRound.onClick.AddListener(() =>
            {
                EnemyManager.Instance.SpawnEnemiesTest();
            });

            gameplayUI.BtnRotate.onClick.AddListener(() => RotatePreviewTile());
            gameplayUI.BtnConfirmRotation.onClick.AddListener(() => SetTile());

            gameplayUI.TilePlacementPanel.SetActive(false);
            gameplayUI.TowerPlacementPanel.SetActive(true);
            gameplayUI.BtnNextRound.gameObject.SetActive(false);
            gameplayUI.RotationPopup.SetActive(false);

            for (int i = 0; i < towerDatas.Count; i++)
            {
                TowerData towerData = towerDatas[i];
                SelectionButton towerButton = gameplayUI.TowerButtons[i];
                towerButton.Title.SetText(towerData.TowerName);

                towerButton.Button.onClick.AddListener(() =>
                {
                    selectedTowerData = towerData;
                });
            }
        }

        public void SelectHexTile(HexTile hexTile)
        {
            _currentSelectedTile = hexTile;
            validCellsForSelectedHexTile = HexManager.Instance.GetValidCells(_currentSelectedTile);
        }

        public void StartAnimateValidCellsForSelectedHexTile()
        {
            foreach (HexCell cell in validCellsForSelectedHexTile)
            {
                cell.OuterColor = Color.white;
                cell.InnerColor = Color.white;
            }
        }

        public void StopAnimateValidCellsForSelectedHexTile()
        {
            foreach (HexCell cell in validCellsForSelectedHexTile)
            {
                cell.OuterColor = Color.black;
                cell.InnerColor = Color.black;
            }
        }

        [Button]
        public void PreviewTileOnTheSelectedCell()
        {
            if (previewTile != null)
                Destroy(previewTile);

            validTileRotationsForSelectedHexTile = HexManager.Instance.GetValidTileRotations(_currentSelectedTile, selectedCell);
            currentValidRotationIndex = 0;

            previewTile = Instantiate(_currentSelectedTile.TilePrefab);
            previewTile.transform.SetParent(selectedCell.transform);
            previewTile.transform.localPosition = Vector3.zero;
            previewTile.transform.rotation = Quaternion.Euler(0, validTileRotationsForSelectedHexTile[currentValidRotationIndex], 0);

            gameplayUI.RotationPopup.SetActive(true);
        }

        [Button]
        public void RotatePreviewTile()
        {
            currentValidRotationIndex = (currentValidRotationIndex + 1) % validTileRotationsForSelectedHexTile.Count;
            previewTile.transform.rotation = Quaternion.Euler(0, validTileRotationsForSelectedHexTile[currentValidRotationIndex], 0);
        }

        public void SetTile()
        {
            if (selectedCell.TrySetTile(_currentSelectedTile, previewTile.transform.eulerAngles.y))
            {
                gameplayUI.RotationPopup.SetActive(false);
                gameplayUI.BtnNextRound.gameObject.SetActive(true);
                gameplayUI.TilePlacementPanel.SetActive(false);
                gameplayUI.TowerPlacementPanel.SetActive(true);
                NavmeshManager.Instance.RebakeNavmesh();

                Destroy(previewTile);
                previewTile = null;
                _currentSelectedTile = null;
                currentGameState = GameplayState.TowerPlacement;
            }
        }
    }
}
