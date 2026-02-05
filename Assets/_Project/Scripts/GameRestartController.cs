using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class GameRestartController : MonoBehaviour
    {
        [SerializeField] Button restartButton;

        void Start()
        {
            restartButton.onClick.AddListener(ReloadScene);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                ReloadScene();
        }

        void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
