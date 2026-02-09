using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TilePlacementController : MonoBehaviour
    {
        public static event Action OnTilePlaced;
        // TODO: create a function for
        // if (previewTile != null)
        //         Destroy(previewTile);

        public HexTileData CurrentSelectedTile => selectedHexTileData;
        public List<HexCell> ValidCellsForSelectedTile => validCellsForSelectedTile;

        [SerializeField] StickyCameraMovement cam;
        [SerializeField] TileRotationConfirmation tileRotationConfirmation;
        [SerializeField] List<HexTileData> tiles = new List<HexTileData>();

        [ReadOnly][SerializeField] private HexTileData selectedHexTileData;
        [ReadOnly][SerializeField] private HexCell selectedCell;
        [ReadOnly][SerializeField] private HexTile previewTile;
        [ReadOnly][SerializeField] private int currentTileRotationIndex;

        [Header("Animations")]
        [Expandable][SerializeField] TilePlacementSettings tilePlacementSettings;

        private List<HexCell> validCellsForSelectedTile = new List<HexCell>();
        private List<float> validRotationsForSelectedTile = new List<float>();

        void Awake()
        {
            tileRotationConfirmation.CancelButton.onClick.AddListener(CancelPreview);
            tileRotationConfirmation.RotateButton.onClick.AddListener(RotatePreview);
            tileRotationConfirmation.ConfirmButton.onClick.AddListener(ConfirmPreview);
        }

        void RotatePreview()
        {
            currentTileRotationIndex++;
            currentTileRotationIndex = currentTileRotationIndex % validRotationsForSelectedTile.Count;
            // TODO: serialize magic number duration to settings file
            previewTile.transform.DOLocalRotate(new Vector3(0, validRotationsForSelectedTile[currentTileRotationIndex], 0), .5f);
            EventManager.Invoke(new OnCellEvent(selectedCell, CellEventType.Rotate));
        }

        void CancelPreview()
        {
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            cam.ZoomOut(null, .5f);
            GameManager.Instance.SwitchToTileSelectionSubState();
            EventManager.Invoke(new OnCellEvent(selectedCell, CellEventType.Cancel));
        }

        void ConfirmPreview()
        {
            if (selectedCell.TrySetTile(selectedHexTileData, validRotationsForSelectedTile[currentTileRotationIndex]))
            {
                Destroy(previewTile.gameObject);
            }

            selectedCell.Tile.transform.localPosition = Vector3.up * 50;
            Sequence seq = DOTween.Sequence();
            seq.Append(selectedCell.Tile.transform.DOLocalMoveY(0, tilePlacementSettings.placementDuration).SetEase(tilePlacementSettings.placementEase));
            seq.Append(selectedCell.Tile.transform.DOShakePosition(tilePlacementSettings.shakeDuration, tilePlacementSettings.shakeStrenght, tilePlacementSettings.shakeVibrato));

            previewTile = null;
            cam.ZoomOut(null, .5f);
            GameManager.Instance.SwitchToTileToTowerTransitionSubState();
            // GameManager.Instance.SwitchToTileSelectionSubState();
            OnTilePlaced?.Invoke();
            EventManager.Invoke(new OnCellEvent(selectedCell, CellEventType.Place));
        }

        public void SetSelectedHexTile(HexTileData tile)
        {
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            selectedHexTileData = tile;
            StopAnimateValidCells();
            validCellsForSelectedTile = HexManager.Instance.GetValidCells(selectedHexTileData);
            StartAnimateValidCells();
            // TODO: Change to POE's position.
            // TODO: move camera movement to state
            cam.ZoomOut(null, .5f);
            // cam.transform.DOMove(Vector3.zero, .5f);
            GameManager.Instance.SwitchToTilePlacementSubState();
        }

        public void SetSelectedCell(HexCell cell)
        {
            selectedCell = cell;
            GameManager.Instance.SwitchToTileRotationSubState();
            EventManager.Invoke(new OnCellEvent(cell, CellEventType.Click));
        }

        public List<HexTileData> GetHexTilesForPlacement()
        {
            // TODO: add select 3 random tile
            return tiles;
        }

        public void StartAnimateValidCells()
        {
            // TODO: add settings for colors
            foreach (HexCell cell in validCellsForSelectedTile)
            {
                cell.OuterColor = Color.white;
                cell.InnerColor = Color.white;
            }
        }

        public void StopAnimateValidCells()
        {
            // TODO: add settings for colors
            foreach (HexCell cell in validCellsForSelectedTile)
            {
                cell.OuterColor = Color.black;
                cell.InnerColor = Color.black;
            }
        }

        public void PreviewTileOnTheCell()
        {
            if (previewTile != null)
                Destroy(previewTile.gameObject);

            StopAnimateValidCells();
            validRotationsForSelectedTile = HexManager.Instance.GetValidTileRotations(selectedHexTileData, selectedCell);
            currentTileRotationIndex = 0;
            tileRotationConfirmation.RotateButton.interactable = validRotationsForSelectedTile.Count > 1;

            previewTile = Instantiate(selectedHexTileData.TilePrefab);
            previewTile.transform.SetParent(selectedCell.transform);
            previewTile.transform.localPosition = Vector3.zero;
            previewTile.transform.rotation = Quaternion.Euler(0, validRotationsForSelectedTile[currentTileRotationIndex], 0);
            previewTile.Outlinable.enabled = true;

            // TODO: Camera zoom in problem.
            cam.ZoomIn(null, .5f);
            cam.MoveTo(selectedCell.transform.position, .5f);
        }
    }
}
