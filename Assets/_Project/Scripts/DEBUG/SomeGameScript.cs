using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FG_GP2_T3
{
	public class SomeGameScript : MonoBehaviour
	{
		void Update() 
		{
			if(Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
				HandleInput();
		}

		void HandleInput() 
		{
			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(inputRay, out RaycastHit hit) && HexGrid.Instance.TryGetCell(hit.point, out HexCell cell))
				EditCell(cell);
		}

		void EditCell(HexCell cell)
		{
			List<HexTile> tiles = HexManager.Instance.GetRandomValidTiles(1);
			if(cell.Tile != null)
				cell.TrySetTile(null);
			else
				cell.TrySetTile(tiles[0]);
		}
	}
}