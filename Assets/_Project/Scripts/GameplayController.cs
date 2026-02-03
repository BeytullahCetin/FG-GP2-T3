using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace FG_GP2_T3
{
    public enum GameplayState
    {
        TilePlacement,
        TowerPlacement,
        EnemyAttack,
        GameOverState
    }

    public class GameplayController : MonoBehaviour
    {
        [SerializeField] GameplayUI gameplayUI;
        [SerializeField] GameOverUI gameOverUI;
        [SerializeField] StickyCameraMovement cameraController;

        private GameplayState currentGameState;

        [Header("Tile Placement")]
        [Expandable][SerializeField] private List<HexTileData> _tiles = new List<HexTileData>();
        private HexTileData _currentSelectedTile;
        private GameObject previewTile;
        private HexCell selectedCell;
        private List<HexCell> validCellsForSelectedHexTile;
        private List<float> validTileRotationsForSelectedHexTile;
        private int currentValidRotationIndex = 0;

        [Header("Tower Placement")]
        [Expandable][SerializeField] List<TowerData> towerDatas = new List<TowerData>();
        [SerializeField] TowerBase baseTowerPrefab;
        [SerializeField] private LayerMask groundDetectionLayerMask;
        private TowerData selectedTowerData;
        private TowerBase previewTower;

        void Awake()
        {
            currentGameState = GameplayState.TilePlacement;

            Initialize();
            StartTilePlacementPhase();
        }

        void Start()
        {
            //EnemyManager.Instance.OnAllEnemiesDead += StartTilePlacementPhase;
            //EnemyManager.Instance.Poe.Health.OnDead += ShowGameOverPanel;
        }

        private void ShowGameOverPanel()
        {
            currentGameState = GameplayState.GameOverState;
            gameOverUI.GameOverPanel.SetActive(true);
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

            if (Input.GetKeyDown(KeyCode.R))
            {
                ReloadScene();
            }
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        private void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out RaycastHit hit) && HexGrid.Instance.TryGetCell(hit.point, out HexCell cell))
            {
                if (cell.Tile == null && _currentSelectedTile != null && validCellsForSelectedHexTile.Contains(cell))
                {
                    selectedCell = cell;
                    PreviewTileOnTheSelectedCell();
                }
            }
        }

        private void TowerPlacementUpdate()
        {
            if (selectedTowerData != null && previewTower != null)
            {
                Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool isRayHit = Physics.Raycast(inputRay, out RaycastHit hit, 250, groundDetectionLayerMask);
                previewTower.gameObject.SetActive(isRayHit);

                if (isRayHit == false)
                    return;

                previewTower.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    SetTower();
                }
            }
        }


        private void Initialize()
        {
            for (int i = 0; i < _tiles.Count; i++)
            {
                HexTileData hexTile = _tiles[i];
                SelectionButton tileButton = gameplayUI.TileButtons[i];
                tileButton.Title.SetText(hexTile.name);

                tileButton.Button.onClick.AddListener(() =>
                {
                    Debug.Log($"tileButtonClicked - {hexTile.name}");

                    if (_currentSelectedTile != null)
                        StopAnimateValidCellsForSelectedHexTile();
                    CancelPreview();
                    SelectHexTile(hexTile);
                    StartAnimateValidCellsForSelectedHexTile();
                });
            }

            gameplayUI.BtnNextRound.onClick.AddListener(() =>
            {
                gameplayUI.TopBar.SetActive(false);
                gameplayUI.BottomBar.SetActive(false);
                gameplayUI.BtnNextRound.gameObject.SetActive(false);
                //EnemyManager.Instance.SpawnEnemiesTest();
                currentGameState = GameplayState.EnemyAttack;
            });

            gameplayUI.BtnCancelRotation.onClick.AddListener(() => CancelPreview());
            gameplayUI.BtnRotate.onClick.AddListener(() => RotatePreviewTile());
            gameplayUI.BtnConfirmRotation.onClick.AddListener(() => SetTile());

            for (int i = 0; i < towerDatas.Count; i++)
            {
                TowerData towerData = towerDatas[i];
                SelectionButton towerButton = gameplayUI.TowerButtons[i];
                towerButton.Title.SetText(towerData.TowerName);

                towerButton.Button.onClick.AddListener(() =>
                {
                    SelectTowerType(towerData);
                });
            }

            gameOverUI.RestartButton.onClick.AddListener(ReloadScene);
            gameOverUI.GameOverPanel.SetActive(false);
        }

        private void StartTilePlacementPhase()
        {
            if (currentGameState == GameplayState.GameOverState)
                return;

            gameplayUI.TxtPhase.FillText("Tile Placement");
            gameplayUI.TopBar.SetActive(true);
            gameplayUI.BottomBar.SetActive(true);
            gameplayUI.TilePlacementPanel.SetActive(true);
            gameplayUI.TowerPlacementPanel.SetActive(false);
            gameplayUI.BtnNextRound.gameObject.SetActive(false);
            gameplayUI.RotationPopup.SetActive(false);

            currentGameState = GameplayState.TilePlacement;
        }

        public void SelectHexTile(HexTileData hexTile)
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

            cameraController.transform.position = previewTile.transform.position;
            cameraController.ZoomIn();
        }

        public void CancelPreview()
        {
            cameraController.transform.position = EnemyManager.Instance.GetTarget().transform.position;
            cameraController.ZoomOut();
            gameplayUI.RotationPopup.SetActive(false);

            if (previewTile != null)
                Destroy(previewTile.gameObject);

            previewTile = null;
            _currentSelectedTile = null;
        }

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
                //NavmeshManager.Instance.RebakeNavmesh();

                Destroy(previewTile.gameObject);
                previewTile = null;
                _currentSelectedTile = null;
                currentGameState = GameplayState.TowerPlacement;
                gameplayUI.TxtPhase.FillText("Tower Placement");
            }
        }

        public void SelectTowerType(TowerData towerData)
        {
            if (selectedTowerData != null && previewTower != null)
                Destroy(previewTower.gameObject);

            selectedTowerData = towerData;
            previewTower = Instantiate(baseTowerPrefab);
            previewTower.Data = towerData;
            previewTower.SetMaterials();
            previewTower.gameObject.SetActive(false);
        }


        private void SetTower()
        {
            previewTower = null;
            selectedTowerData = null;
        }
    }
}
