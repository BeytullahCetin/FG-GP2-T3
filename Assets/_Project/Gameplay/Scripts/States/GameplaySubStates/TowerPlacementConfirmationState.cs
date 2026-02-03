namespace FG_GP2_T3
{
	class TowerPlacementConfirmationState : SubState
	{
		TowerPlacementController towerPlacementController;
		public TowerPlacementConfirmationState(StateMachine stateMachine, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			this.towerPlacementController = towerPlacementController;
		}
	}
}