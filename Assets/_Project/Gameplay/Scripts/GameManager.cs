using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FG_GP2_T3
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public GameSpeedController GameSpeedController => gameSpeedController;
        public bool IsGameOver => isGameOver;

        [SerializeField] bool showLoadingScreen;
        [SerializeField] bool unlimitedCompost;

        bool isGameOver;

        [SerializeField] TilePlacementController tilePlacementController;
        [SerializeField] TowerPlacementController towerPlacementController;
        [SerializeField] LoadingScreen loadingScreen;
        [SerializeField] GameSpeedController gameSpeedController;

        private StateMachine gameflowStateMachine;
        private MainMenuState mainMenuState;
        private GameplayState gameplayState;
        private GameOverState gameOverState;
        private GameWinState gameWinState;


        void Awake()
        {
            Instance = this;
            gameflowStateMachine = new StateMachine();
            mainMenuState = new MainMenuState(gameflowStateMachine);
            gameplayState = new GameplayState(gameflowStateMachine, tilePlacementController, towerPlacementController);
            gameOverState = new GameOverState(gameflowStateMachine);
            gameWinState = new GameWinState(gameflowStateMachine);
        }

        void Start()
        {
            // QualitySettings.vSyncCount = 0;
            Time.timeScale = 1;
            isGameOver = false;

            SwitchToMainMenuState();
#if UNITY_EDITOR
            if (showLoadingScreen == true)
#endif
                loadingScreen.TriggerLoadingBar().Forget();
#if UNITY_EDITOR
            if (unlimitedCompost == true)
                CompostManager.Instance.AddCompost(500000);
#endif
        }

        void Update()
        {
            gameflowStateMachine.Update();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }

        public void PauseGame()
        {
            gameSpeedController.PauseGame();
        }

        public void ResumeGame()
        {
            gameSpeedController.ResumeGame();
        }

        #region State Switches

        [Button]
        public void SwitchToMainMenuState()
        {
            gameflowStateMachine.ChangeState(mainMenuState);
        }

        [Button]
        public void SwitchToGameplayState()
        {
            gameflowStateMachine.ChangeState(gameplayState);
        }

        [Button]
        public void SwitchToGameOverState()
        {
            isGameOver = true;
            gameflowStateMachine.ChangeState(gameOverState);
        }

        [Button]
        public void SwitchToGameWinState()
        {
            isGameOver = true;
            gameflowStateMachine.ChangeState(gameWinState);
        }

        [Button]
        public void SwitchToTileSelectionSubState()
        {
            if (IsGameOver == false)
                gameplayState.SwitchToTileSelectionState();
        }

        [Button]
        public void SwitchToTilePlacementSubState()
        {
            gameplayState.SwitchToTilePlacementState();
        }

        [Button]
        public void SwitchToTileRotationSubState()
        {
            gameplayState.SwitchToTileRotationState();
        }

        [Button]
        public void SwitchToTileToTowerTransitionSubState()
        {
            gameplayState.SwitchToTileToTowerTransitionState();
        }

        [Button]
        public void SwitchToTowerSelectionSubState()
        {
            gameplayState.SwitchToTowerSelectionState();
        }

        [Button]
        public void SwitchToTowerPlacementSubState()
        {
            gameplayState.SwitchToTowerPlacementState();
        }

        [Button]
        public void SwitchToTowerPlacementConfirmationSubState()
        {
            gameplayState.SwitchToTowerPlacementConfirmationState();
        }

        [Button]
        public void SwitchToFusionConfirmationSubState()
        {
            gameplayState.SwitchToTowerFusionConfirmationState();
        }

        [Button]
        public void SwitchToEnemyWaveSubState()
        {
            gameplayState.SwitchToEnemyWaveState();
        }

        #endregion
    }
}
