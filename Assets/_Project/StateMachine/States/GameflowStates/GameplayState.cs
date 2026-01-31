using System;

namespace FG_GP2_T3
{
	[Serializable]
	class GameplayState : State
	{
		protected StateMachine subStateMachine;

		public GameplayState(StateMachine stateMachine) : base(stateMachine)
		{
			subStateMachine = new StateMachine();
		}

		public override void Enter()
		{
			base.Enter();
			subStateMachine.ChangeState(new TileSelectionState(subStateMachine));
		}

		public override void Update()
		{
			subStateMachine.Update();
		}

		public void ChangeSubState(State newState)
		{
			subStateMachine.ChangeState(newState);
		}
	}
}