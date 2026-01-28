using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FG_GP2_T3
{
	public class SomeGameScript : MonoBehaviour
	{
		bool _tileSelected = false;
		HexTile _selectedTile;
		List<HexCell> _highlightedCells = new();

		private void Update() 
		{
			if(!_tileSelected)
			{
				foreach(HexCell cell in _highlightedCells) //Clearing highlighting
					cell.InnerColor = Color.black;

				_selectedTile = HexManager.Instance.GetRandomValidTiles(3, true)[0]; //Always picking the first option out of 3 returned by the func

				_highlightedCells = HexManager.Instance.GetValidCells(_selectedTile); //Getting new cells to highlight
				foreach(HexCell cell in _highlightedCells)
					cell.InnerColor = Color.white;

				_tileSelected = true;
			}

			if(Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
				HandleInput();
		}

		private void HandleInput() 
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(inputRay, out RaycastHit hit) && HexGrid.Instance.TryGetCell(hit.point, out HexCell cell))
				EditCell(cell);
		}

		private void EditCell(HexCell cell)
		{
			cell.TrySetTile(_selectedTile, HexManager.Instance.GetValidTileRotations(_selectedTile, cell)[0]); //Setting the cell with first valid rotation
			_tileSelected = false;
		}
	}
}