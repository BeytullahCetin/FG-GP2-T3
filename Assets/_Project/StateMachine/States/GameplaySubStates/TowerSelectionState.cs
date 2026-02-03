namespace FG_GP2_T3
{
	class TowerSelectionState : SubState
	{
		public TowerSelectionState(StateMachine stateMachine) : base(stateMachine)
		{
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
	}
}