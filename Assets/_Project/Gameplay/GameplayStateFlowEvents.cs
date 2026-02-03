using System;

public class GameplayStateFlowEvents
{
	public static Action OnEnteredTileSelectionSubGameplayState;
	public static Action OnExitedTileSelectionSubGameplayState;

	public static Action OnEnteredTilePlacementSubGameplayState;
	public static Action OnExitedTilePlacementSubGameplayState;

	public static Action OnEnteredTileRotationSubGameplayState;
	public static Action OnExitedTileRotationSubGameplayState;

	public static Action OnEnteredTowerSelectionSubGameplayState;
	public static Action OnExitedTowerSelectionSubGameplayState;

	public static Action OnEnteredTowerPlacementSubGameplayState;
	public static Action OnExitedTowerPlacementSubGameplayState;

	public static Action OnEnteredTowerPlacementConfirmationSubGameplayState;
	public static Action OnExitedTowerPlacementConfirmationSubGameplayState;

	public static Action OnEnteredTowerFusionConfirmationSubGameplayState;
	public static Action OnExitedTowerFusionConfirmationSubGameplayState;

}