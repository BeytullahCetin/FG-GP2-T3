using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
	public class GameOverUI : MonoBehaviour
	{
		[SerializeField] ScaleUpPanel gameoverPanel;
		[SerializeField] Button restartButton;

		void OnEnable()
		{
			GameflowEvents.OnEnterGameOverState += gameoverPanel.Show;
		}

		void OnDisable()
		{
			GameflowEvents.OnEnterGameOverState -= gameoverPanel.Show;
		}

		void Start()
		{
			gameoverPanel.Hide();
		}
	}
}