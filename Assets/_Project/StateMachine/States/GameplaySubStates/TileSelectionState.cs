namespace FG_GP2_T3
{
	class TileSelectionState : SubState
	{
		TilePlacementController tilePlacementController;
		public TileSelectionState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			this.tilePlacementController = tilePlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState?.Invoke();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTileSelectionSubGameplayState?.Invoke();
		}
	}
}