using System;
using FG_GP2_T3;
using NaughtyAttributes;
using UnityEngine;

public class GameSpeedController : MonoBehaviour
{
	public static event Action<float> OnGameSpeedSet;

	[Expandable][SerializeField] GameSpeedSettings gameSpeedSettings;
	private int currentGameSpeedIndex = 0;

	void OnEnable()
	{
		GameplayStateFlowEvents.OnEnteredEnemyWaveState += ResetGameSpeed;
		GameplayStateFlowEvents.OnExitedEnemyWaveState += ResetGameSpeed;
	}

	void OnDisable()
	{
		GameplayStateFlowEvents.OnEnteredEnemyWaveState -= ResetGameSpeed;
		GameplayStateFlowEvents.OnExitedEnemyWaveState -= ResetGameSpeed;
	}

	public void CycleGameSpeeds()
	{
		currentGameSpeedIndex = (currentGameSpeedIndex + 1) % gameSpeedSettings.GameSpeeds.Count;
		SetGameSpeed(currentGameSpeedIndex);
	}

	public void ResetGameSpeed()
	{
		currentGameSpeedIndex = 0;
		SetGameSpeed(currentGameSpeedIndex);
	}

	private void SetGameSpeed(int gameSpeedIndex)
	{
		Time.timeScale = gameSpeedSettings.GameSpeeds[gameSpeedIndex];
		OnGameSpeedSet?.Invoke(gameSpeedSettings.GameSpeeds[gameSpeedIndex]);
	}

	public void ResumeGame()
	{
		Time.timeScale = gameSpeedSettings.GameSpeeds[currentGameSpeedIndex];
	}

	public void PauseGame()
	{
		Time.timeScale = 0;
	}
}