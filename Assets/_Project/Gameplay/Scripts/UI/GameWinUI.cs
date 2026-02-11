using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
	public class GameWinUI : MonoBehaviour
	{
		[SerializeField] ScaleUpPanel gameWinPanel;
		[SerializeField] Button restartButton;

		void OnEnable()
		{
			GameflowEvents.OnEnterGameWinState += gameWinPanel.Show;
		}

		void OnDisable()
		{
			GameflowEvents.OnEnterGameWinState -= gameWinPanel.Show;
		}

		void Start()
		{
			gameWinPanel.Hide();
			restartButton.onClick.AddListener(GameManager.Instance.ReloadScene);
		}
	}
}