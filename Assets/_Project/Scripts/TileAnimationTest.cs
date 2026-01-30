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
        [SerializeField] float dropDuration = .5f;
        [SerializeField] float delayBetweenCells = .1f;

        [SerializeField] List<HexCell> cells = new List<HexCell>();

        [Button]
        public void AnimateCells()
        {
            DisableAllCells();
            foreach (Transform child in cellsParent)
            {
                child.position = new Vector3(child.position.x, 50, child.position.z);
            }

            GetHexCellReferances();

            Sequence seq = DOTween.Sequence();
            int cellIndex = 0;
            foreach (HexCell cell in cells)
            {
                cell.transform.DOMoveY(0, dropDuration)
                    .SetDelay(delayBetweenCells * cellIndex++)
                    .OnStart(() => cell.gameObject.SetActive(true));
            }
        }

        [Button]
        public void GetHexCellReferances()
        {
            Debug.Log(cellsParent.name);

            cells = cellsParent.GetComponentsInChildren<HexCell>(true)
                .OrderBy(cell => Math.Abs(cell.Coordinates.Q) + Math.Abs(cell.Coordinates.R) + Math.Abs(cell.Coordinates.S))
                .ToList();
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
