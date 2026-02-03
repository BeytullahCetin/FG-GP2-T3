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

		public override void Update()
		{
			base.Update();
			CheckValidCellClicked();
		}

		public override void Enter()
		{
			base.Enter();
			GameflowEvents.OnEnteredTilePlacementSubGameplayState?.Invoke();
			tilePlacementController.StartAnimateValidCells();
		}

		public override void Exit()
		{
			base.Exit();
			tilePlacementController.StopAnimateValidCells();
			GameflowEvents.OnExitedTilePlacementSubGameplayState?.Invoke();
		}

		private void CheckValidCellClicked()
		{
			if (Input.GetMouseButtonDown(0) == false)
				return;

			if (tilePlacementController.CurrentSelectedTile == null)
				return;

			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			// TODO: Add collider to cells. detect cell with getcomponent when raycast hit
			bool isHit = Physics.Raycast(inputRay, out RaycastHit hit);

			if (isHit == false)
				return;


			bool hasGetCell = HexGrid.Instance.TryGetCell(hit.point, out HexCell cell);
			if (hasGetCell == false)
				return;

			if (cell.Tile != null || tilePlacementController.ValidCellsForSelectedTile.Contains(cell) == false)
				return;


			tilePlacementController.SetSelectedCell(cell);
		}
	}
}