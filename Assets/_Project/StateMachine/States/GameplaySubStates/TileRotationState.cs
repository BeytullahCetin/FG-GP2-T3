namespace FG_GP2_T3
{
	class TileRotationState : SubState
	{
		TilePlacementController tilePlacementController;

		public TileRotationState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			this.tilePlacementController = tilePlacementController;
		}
	}
}