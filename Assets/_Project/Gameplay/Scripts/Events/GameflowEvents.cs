using System;

public class GameflowEvents
{
	public static Action OnEnteredMainMenuState;
	public static Action OnExitedMainMenuState;

	public static Action OnEnteredGameplayState;
	public static Action OnExitedGameplayState;

	public static Action OnEnterGameOverState;
}