using FG_GP2_T3;

class GameplayBottomPanel : MovePanel
{
	void OnEnable()
	{
		GameflowEvents.OnEnteredGameplayState += Show;
		GameflowEvents.OnExitedGameplayState += Hide;
	}

	void OnDisable()
	{
		GameflowEvents.OnEnteredGameplayState -= Show;
		GameflowEvents.OnExitedGameplayState -= Hide;
	}

	protected override void SetEnabledPosition()
	{
		showPosition = rectTransform.anchoredPosition;
	}

	protected override void SetDisabledPosition()
	{
		hidePosition = showPosition;
		hidePosition.y = -rectTransform.sizeDelta.y;
	}
}