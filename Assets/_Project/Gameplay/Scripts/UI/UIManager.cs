using UnityEngine;

namespace FG_GP2_T3
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public GameplayUI GameplayUI => gameplayUI;
        GameplayUI gameplayUI;

        void Awake()
        {
            Instance = this;
            gameplayUI = FindAnyObjectByType<GameplayUI>();
        }
    }
}
