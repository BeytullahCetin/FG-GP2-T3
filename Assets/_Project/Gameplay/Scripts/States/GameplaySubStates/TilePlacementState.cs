using UnityEngine;

namespace FG_GP2_T3
{
	class TilePlacementState : SubState
	{
		TilePlacementController tilePlacementController;

		public TilePlacementState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			this.tilePlacementController = tilePlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTilePlacementSubGameplayState?.Invoke();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTilePlacementSubGameplayState?.Invoke();
		}

		public override void Update()
		{
			base.Update();
			CheckValidCellClicked();
		}

		private void CheckValidCellClicked()
		{
			if (Input.GetMouseButtonDown(0) == false)
				return;

			if (tilePlacementController.CurrentSelectedTile == null)
				return;

			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			bool isHit = Physics.Raycast(inputRay, out RaycastHit hit);

			if (isHit == false)
				return;

			// Debug.Log("ray hit");

			bool hasGetCell = HexGrid.Instance.TryGetCell(hit.point, out HexCell cell);
			if (hasGetCell == false)
				return;

			// Debug.Log("get cell");

			if (cell.Tile != null || tilePlacementController.ValidCellsForSelectedTile.Contains(cell) == false)
				return;


			tilePlacementController.SetSelectedCell(cell);
		}
	}
}