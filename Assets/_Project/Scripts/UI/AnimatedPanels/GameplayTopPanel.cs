using FG_GP2_T3;

class GameplayTopPanel : MovePanel
{
	

	protected override void SetEnabledPosition()
	{
		showPosition = rectTransform.anchoredPosition;
	}

	protected override void SetDisabledPosition()
	{
		hidePosition = showPosition;
		hidePosition.y = rectTransform.sizeDelta.y;
	}
}