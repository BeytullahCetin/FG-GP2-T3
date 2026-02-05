using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] bool showLoadingScreen;

        [SerializeField] TilePlacementController tilePlacementController;
        [SerializeField] TowerPlacementController towerPlacementController;
        [SerializeField] LoadingScreen loadingScreen;

        private StateMachine gameflowStateMachine;
        private MainMenuState mainMenuState;
        private GameplayState gameplayState;
        private GameOverState gameOverState;

        void Awake()
        {
            Instance = this;
            gameflowStateMachine = new StateMachine();
            mainMenuState = new MainMenuState(gameflowStateMachine);
            gameplayState = new GameplayState(gameflowStateMachine, tilePlacementController, towerPlacementController);
            gameOverState = new GameOverState(gameflowStateMachine);
        }

        void Start()
        {
            SwitchToMainMenuState();

            if (showLoadingScreen == true)
                loadingScreen.TriggerLoadingBar().Forget();
        }

        void Update()
        {
            gameflowStateMachine.Update();
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
            gameflowStateMachine.ChangeState(gameOverState);
        }

        [Button]
        public void SwitchToTileSelectionSubState()
        {
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
