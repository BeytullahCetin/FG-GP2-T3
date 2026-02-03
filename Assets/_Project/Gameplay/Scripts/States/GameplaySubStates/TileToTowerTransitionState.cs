namespace FG_GP2_T3
{
	class TileToTowerTransitionState : SubState
	{
		public TileToTowerTransitionState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public async override void Enter()
		{
			base.Enter();
			await UIManager.Instance.GameplayUI.TransitionFromTileToTower();
			GameManager.Instance.SwitchToTowerSelectionSubState();
		}

		public override void Exit()
		{
			base.Exit();
		}
	}
}