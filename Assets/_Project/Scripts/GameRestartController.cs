using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class GameRestartController : MonoBehaviour
    {
        [SerializeField] Button restartButton;

        void Start()
        {
            restartButton.onClick.AddListener(GameManager.Instance.ReloadScene);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                GameManager.Instance.ReloadScene();
        }


    }
}
