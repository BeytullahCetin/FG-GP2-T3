namespace FG_GP2_T3
{
	class TowerPlacementConfirmationState : SubState
	{
		TowerPlacementController towerPlacementController;
		public TowerPlacementConfirmationState(StateMachine stateMachine, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			this.towerPlacementController = towerPlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTowerPlacementConfirmationSubGameplayState?.Invoke();
			towerPlacementController.PreviewTowerOnEmptyCell();
			towerPlacementController.SetPlacementListeners();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTowerPlacementConfirmationSubGameplayState?.Invoke();
		}
	}
}