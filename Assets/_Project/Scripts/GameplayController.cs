using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FG_GP2_T3
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField] GameplayUI gameplayUI;
        [Expandable][SerializeField] private List<HexTile> _tiles = new List<HexTile>();
        [ReadOnly][SerializeField] private HexTile _currentSelectedTile;

        [SerializeField] private GameObject previewTile;
        [SerializeField] private HexCell selectedCell;
        [SerializeField] private List<HexCell> validCellsForSelectedHexTile;
        [SerializeField] private List<float> validTileRotationsForSelectedHexTile;
        private int currentValidRotationIndex = 0;

        void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                HandleInput();
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
            gameplayUI.BtnNextRound.gameObject.SetActive(false);
            gameplayUI.RotationPopup.SetActive(false);
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
                NavmeshManager.Instance.RebakeNavmesh();

                Destroy(previewTile);
                previewTile = null;
                _currentSelectedTile = null;
            }
        }
    }
}
