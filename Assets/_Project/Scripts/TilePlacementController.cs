using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TilePlacementController : MonoBehaviour
    {
        // TODO: create a function for
        // if (previewTile != null)
        //         Destroy(previewTile);

        public HexTile CurrentSelectedTile => selectedTile;
        public List<HexCell> ValidCellsForSelectedTile => validCellsForSelectedTile;

        [SerializeField] StickyCameraMovement cam;
        [SerializeField] TileRotationConfirmation tileRotationConfirmation;
        [SerializeField] List<HexTile> tiles = new List<HexTile>();

        [ReadOnly][SerializeField] private HexTile selectedTile;
        [ReadOnly][SerializeField] private HexCell selectedCell;
        [ReadOnly][SerializeField] private GameObject previewTile;
        [ReadOnly][SerializeField] private int currentTileRotationIndex;
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
        }

        void CancelPreview()
        {
            if (previewTile != null)
                Destroy(previewTile);

            GameManager.Instance.SwitchToTileSelectionSubState();
        }

        void ConfirmPreview()
        {
            if (selectedCell.TrySetTile(selectedTile, previewTile.transform.eulerAngles.y))
            {
                NavmeshManager.Instance.RebakeNavmesh();
                Destroy(previewTile.gameObject);
            }

            previewTile = null;
            GameManager.Instance.SwitchToTileToTowerTransitionSubState();
        }

        public void SetSelectedHexTile(HexTile tile)
        {
            if (previewTile != null)
                Destroy(previewTile);

            selectedTile = tile;
            validCellsForSelectedTile = HexManager.Instance.GetValidCells(selectedTile);
            // TODO: Change to POE's position.
            // TODO: add do move function to camera script
            cam.transform.DOMove(Vector3.zero, .5f);
            GameManager.Instance.SwitchToTilePlacementSubState();
        }

        public void SetSelectedCell(HexCell cell)
        {
            selectedCell = cell;
            GameManager.Instance.SwitchToTileRotationSubState();
        }

        public List<HexTile> GetHexTilesForPlacement()
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
                Destroy(previewTile);

            validRotationsForSelectedTile = HexManager.Instance.GetValidTileRotations(selectedTile, selectedCell);
            currentTileRotationIndex = 0;

            previewTile = Instantiate(selectedTile.TilePrefab);
            previewTile.transform.SetParent(selectedCell.transform);
            previewTile.transform.localPosition = Vector3.zero;
            previewTile.transform.rotation = Quaternion.Euler(0, validRotationsForSelectedTile[currentTileRotationIndex], 0);

            // TODO: Camera zoom in problem.
            // cam.ZoomIn();
            cam.transform.DOMove(selectedCell.transform.position, .5f);
        }
    }
}
