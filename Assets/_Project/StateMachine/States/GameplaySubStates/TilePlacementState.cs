using UnityEngine;

namespace FG_GP2_T3
{
	class TilePlacementState : SubState
	{
		TilePlacementController tilePlacementController;

		public TilePlacementState(StateMachine stateMachine, TilePlacementController tilePlacementController) : base(stateMachine)
		{
			this.tilePlacementController = tilePlacementController;
		}

		public override void Update()
		{
			base.Update();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("Space pressed");
			}
		}

		public override void Enter()
		{
			base.Enter();
			tilePlacementController.SetValidCellsForSelectedHexTile();
			tilePlacementController.StartAnimateValidCells();
		}

		public override void Exit()
		{
			base.Exit();
			tilePlacementController.StopAnimateValidCells();
		}
	}
}