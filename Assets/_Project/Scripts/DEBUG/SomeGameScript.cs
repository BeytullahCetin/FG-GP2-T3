using UnityEngine;
using UnityEngine.EventSystems;

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
		if(Physics.Raycast(inputRay, out RaycastHit hit))
			EditCell(HexGrid.Instance.GetCell(hit.point));
	}

	void EditCell(HexCell cell)
    {
        cell.InnerColor = Color.red;
    }
}