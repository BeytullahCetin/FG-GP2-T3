namespace FG_GP2_T3
{
	class TowerFusionConfirmationState : SubState
	{
		TowerPlacementController towerPlacementController;
		public TowerFusionConfirmationState(StateMachine stateMachine, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			this.towerPlacementController = towerPlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTowerFusionConfirmationSubGameplayState?.Invoke();
			towerPlacementController.PreviewFusionOnTower();
			towerPlacementController.SetFusionListeners();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTowerFusionConfirmationSubGameplayState?.Invoke();
		}
	}
}