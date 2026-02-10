namespace FG_GP2_T3
{
	class TowerSelectionState : SubState
	{
		TowerPlacementController towerPlacementController;
		public TowerSelectionState(StateMachine stateMachine, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			this.towerPlacementController = towerPlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState?.Invoke();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTowerSelectionSubGameplayState?.Invoke();
		}

		public override void Update()
		{
			base.Update();
			towerPlacementController.ListenSelectCellClicks();
		}
	}
}