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
			towerPlacementController.ListenSelectCellClicks();
		}
	}
}