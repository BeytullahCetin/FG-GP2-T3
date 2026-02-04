namespace FG_GP2_T3
{
	class EnemyWaveState : SubState
	{
		public EnemyWaveState(StateMachine stateMachine) : base(stateMachine)
		{
		}

		public override void Enter()
		{
			base.Enter();
			GameplayStateFlowEvents.OnEnteredEnemyWaveState?.Invoke();

			EnemyManager.Instance.StartWave();
		}

		public override void Exit()
		{
			base.Exit();
			GameplayStateFlowEvents.OnExitedEnemyWaveState?.Invoke();
		}
	}
}