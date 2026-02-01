namespace FG_GP2_T3
{
	class TilePlacementState : SubState
	{
		TilePlacementController tilePlacementController;

		public TilePlacementState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			this.tilePlacementController = tilePlacementController;
		}
	}
}