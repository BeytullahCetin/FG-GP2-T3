namespace FG_GP2_T3
{
	class MainMenuState : State
	{
		public MainMenuState(StateMachine stateMachine) : base(stateMachine)
		{

		}

		public override void Enter()
		{
			base.Enter();
			GameflowEvents.OnEnteredMainMenuState?.Invoke();
		}

		public override void Exit()
		{
			base.Exit();
			GameflowEvents.OnExitedMainMenuState?.Invoke();
		}
	}
}