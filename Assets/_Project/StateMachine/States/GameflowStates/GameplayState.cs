using System;

namespace FG_GP2_T3
{
	[Serializable]
	class GameplayState : State
	{
		private StateMachine subStateMachine;
		private TileSelectionState tileSelectionState;
		private TilePlacementState tilePlacementState;
		private TileRotationState tileRotationState;

		private TowerSelectionState towerSelectionState;

		private TilePlacementController tilePlacementController;

		public GameplayState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			subStateMachine = new StateMachine();
			this.tilePlacementController = tilePlacementController;
		}

		public override void Enter()
		{
			base.Enter();
			tileSelectionState = new TileSelectionState(subStateMachine, tilePlacementController);
			tilePlacementState = new TilePlacementState(subStateMachine, tilePlacementController);
			tileRotationState = new TileRotationState(subStateMachine, tilePlacementController);

			towerSelectionState = new TowerSelectionState(subStateMachine);

			GameflowEvents.OnEnteredGameplayState?.Invoke();
			SwitchToTileSelectionState();
		}

		public override void Exit()
		{
			base.Exit();
			GameflowEvents.OnExitedGameplayState?.Invoke();
		}

		public override void Update()
		{
			subStateMachine.Update();
		}

		#region State Switches

		public void SwitchToTileSelectionState()
		{
			subStateMachine.ChangeState(tileSelectionState);
		}

		public void SwitchToTilePlacementState()
		{
			subStateMachine.ChangeState(tilePlacementState);
		}

		public void SwitchToTileRotationState()
		{
			subStateMachine.ChangeState(tileRotationState);
		}

		public void SwitchToTowerSelectionState()
		{
			subStateMachine.ChangeState(towerSelectionState);
		}

		#endregion
	}
}