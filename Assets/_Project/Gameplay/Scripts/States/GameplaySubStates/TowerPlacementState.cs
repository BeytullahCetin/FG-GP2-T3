using UnityEngine;

namespace FG_GP2_T3
{
	class TowerPlacementState : SubState
	{
		TowerPlacementController towerPlacementController;
		public TowerPlacementState(StateMachine stateMachine, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			this.towerPlacementController = towerPlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTowerPlacementSubGameplayState?.Invoke();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTowerPlacementSubGameplayState?.Invoke();
		}

		public override void Update()
		{
			base.Update();
			ListenSelectCellClicks();
		}

		private void ListenSelectCellClicks()
		{
			if (Input.GetMouseButtonDown(0) == false)
				return;

			if (towerPlacementController.SelectedTower == null)
			{
				return;
			}

			Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			bool isHit = Physics.Raycast(inputRay, out RaycastHit hit);

			if (isHit == false)
				return;

			bool hasGetCell = HexGrid.Instance.TryGetCell(hit.point, out HexCell cell);
			if (hasGetCell == false)
				return;

			if (towerPlacementController.ValidCellsForSelectedTile.Contains(cell) == false)
				return;

			towerPlacementController.SelectCellForTower(cell);
		}
	}
}