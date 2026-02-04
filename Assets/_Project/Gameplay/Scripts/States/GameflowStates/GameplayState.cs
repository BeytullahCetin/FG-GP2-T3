using System;

namespace FG_GP2_T3
{
	[Serializable]
	class GameplayState : State
	{
		private TilePlacementController tilePlacementController;
		private TowerPlacementController towerPlacementController;

		private StateMachine subStateMachine;
		private TileSelectionState tileSelectionState;
		private TilePlacementState tilePlacementState;
		private TileRotationState tileRotationState;

		private TileToTowerTransitionState tileToTowerTransitionState;

		private TowerSelectionState towerSelectionState;
		private TowerPlacementState towerPlacementState;
		private TowerPlacementConfirmationState towerPlacementConfirmationState;
		private TowerFusionConfirmationState towerFusionConfirmationState;
		private EnemyWaveState enemyWaveState;


		public GameplayState(StateMachine stateMachine, TilePlacementController tilePlacementController, TowerPlacementController towerPlacementController) : base(stateMachine)
		{
			subStateMachine = new StateMachine();
			this.tilePlacementController = tilePlacementController;
			this.towerPlacementController = towerPlacementController;
		}

		public override void Enter()
		{
			base.Enter();

			tileSelectionState = new TileSelectionState(subStateMachine, tilePlacementController);
			tilePlacementState = new TilePlacementState(subStateMachine, tilePlacementController);
			tileRotationState = new TileRotationState(subStateMachine, tilePlacementController);

			tileToTowerTransitionState = new TileToTowerTransitionState(subStateMachine);

			towerSelectionState = new TowerSelectionState(subStateMachine, towerPlacementController);
			towerPlacementState = new TowerPlacementState(subStateMachine, towerPlacementController);
			towerPlacementConfirmationState = new TowerPlacementConfirmationState(stateMachine, towerPlacementController);
			towerFusionConfirmationState = new TowerFusionConfirmationState(stateMachine, towerPlacementController);
			enemyWaveState = new EnemyWaveState(subStateMachine);

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

		#region Tile States

		public void SwitchToTileSelectionState()
		{
			subStateMachine.ChangeState(tileSelectionState);
			UIManager.Instance.GameplayUI.SetPhaseText("Tile Placement Phase");
		}

		public void SwitchToTilePlacementState()
		{
			subStateMachine.ChangeState(tilePlacementState);
		}

		public void SwitchToTileRotationState()
		{
			subStateMachine.ChangeState(tileRotationState);
		}

		#endregion

		public void SwitchToTileToTowerTransitionState()
		{
			subStateMachine.ChangeState(tileToTowerTransitionState);
		}

		#region Tower States

		public void SwitchToTowerSelectionState()
		{
			subStateMachine.ChangeState(towerSelectionState);
			UIManager.Instance.GameplayUI.SetPhaseText("Tower Placement Phase");
		}

		public void SwitchToTowerPlacementState()
		{
			subStateMachine.ChangeState(towerPlacementState);
		}

		public void SwitchToTowerPlacementConfirmationState()
		{
			subStateMachine.ChangeState(towerPlacementConfirmationState);
		}

		public void SwitchToTowerFusionConfirmationState()
		{
			subStateMachine.ChangeState(towerFusionConfirmationState);
		}

		public void SwitchToEnemyWaveState()
		{
			subStateMachine.ChangeState(enemyWaveState);
		}

		#endregion

		#endregion
	}
}