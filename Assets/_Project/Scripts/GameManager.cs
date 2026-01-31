using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        private StateMachine gameflowStateMachine;
        private MainMenuState mainMenuState;
        private GameplayState gameplayState;
        private GameOverState gameOverState;

        void Awake()
        {
            Instance = this;
            gameflowStateMachine = new StateMachine();
            mainMenuState = new MainMenuState(gameflowStateMachine);
            gameplayState = new GameplayState(gameflowStateMachine);
            gameOverState = new GameOverState(gameflowStateMachine);
        }

        void Start()
        {
            SwitchToMainMenu();
        }

        void Update()
        {
            gameflowStateMachine.Update();
        }

        [Button]
        public void SwitchToMainMenu()
        {
            gameflowStateMachine.ChangeState(mainMenuState);
        }

        [Button]
        public void StartGame()
        {
            gameflowStateMachine.ChangeState(gameplayState);
        }

        [Button]
        public void GameOver()
        {
            gameflowStateMachine.ChangeState(gameOverState);
        }
    }
}
