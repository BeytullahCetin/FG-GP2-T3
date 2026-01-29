using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
	class GameOverUI : MonoBehaviour
	{
		[SerializeField] GameObject gameOverPanel;
		[SerializeField] Button restartButton;

		public GameObject GameOverPanel => gameOverPanel;
		public Button RestartButton => restartButton;
	}
}