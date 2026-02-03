namespace FG_GP2_T3
{
	class TowerFusionConfirmationState : SubState
	{
		TowerPlacementController towerPlacementController;
		public TowerFusionConfirmationState(StateMachine stateMachine, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			this.towerPlacementController = towerPlacementController;
		}
	}
}