using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FG_GP2_T3
{
	public class SomeGameScript : MonoBehaviour
	{
		private void Update() 
		{
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
			List<HexTile> tiles = HexManager.Instance.GetRandomValidTiles(1);
			if(cell.Tile != null)
				cell.TrySetTile(null);
			else
				cell.TrySetTile(tiles[0]);
		}
	}
}