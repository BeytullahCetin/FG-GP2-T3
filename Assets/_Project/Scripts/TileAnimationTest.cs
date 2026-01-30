using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

namespace FG_GP2_T3
{
    public class TileAnimationTest : MonoBehaviour
    {
        [SerializeField] Transform cellsParent;
        [SerializeField] float dropHeight = 100f;
        [SerializeField] float dropDuration = 1f;
        [SerializeField] float delayBetweenCells = .1f;

        [SerializeField] List<HexCell> cells = new List<HexCell>();

        void Awake()
        {
            cells = cellsParent.GetComponentsInChildren<HexCell>(true).ToList();
        }

        [Button]
        public void SetSpiralAnimation()
        {
            cells = cells
                .OrderBy(cell => Math.Abs(cell.Coordinates.Q) + Math.Abs(cell.Coordinates.R) + Math.Abs(cell.Coordinates.S))
                .ThenBy(cell => Math.Atan2(cell.Coordinates.R, cell.Coordinates.Q))
                .ToList();
        }

        [Button]
        public void SetClosestAnimation()
        {
            cells = cells
                .OrderBy(cell => Math.Abs(cell.Coordinates.Q) + Math.Abs(cell.Coordinates.R) + Math.Abs(cell.Coordinates.S))
                .ToList();
        }

        [Button]
        public void PlayAnimation()
        {
            DisableAllCells();
            Sequence seq = DOTween.Sequence();
            for (int i = 0; i < cells.Count; i++)
            {
                HexCell cell = cells[i];

                cell.transform.position = new Vector3(cell.transform.position.x, dropHeight, cell.transform.position.z);
                cell.transform
                    .DOMoveY(0, dropDuration)
                    .SetDelay(delayBetweenCells * i)
                    .SetEase(Ease.OutBack)
                    .OnStart(() => cell.gameObject.SetActive(true));
            }
        }

        public void ResetCellPositions()
        {
            foreach (Transform child in cellsParent)
            {
                child.position = new Vector3(child.position.x, 0, child.position.z);
            }
        }

        public void DisableAllCells()
        {
            foreach (Transform child in cellsParent)
            {
                child.position = new Vector3(child.position.x, 50, child.position.z);
                child.gameObject.SetActive(false);
            }
        }
    }
}
