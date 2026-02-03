namespace FG_GP2_T3
{
	class TileRotationState : SubState
	{
		TilePlacementController tilePlacementController;

		public TileRotationState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			this.tilePlacementController = tilePlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredTileRotationSubGameplayState?.Invoke();
			tilePlacementController.PreviewTileOnTheCell();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedTileRotationSubGameplayState?.Invoke();
		}
	}
}