using Unity.VisualScripting;

namespace FG_GP2_T3
{
	class GameWinState : State
	{
		public GameWinState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
			GameflowEvents.OnEnterGameWinState?.Invoke();
		}
	}
}