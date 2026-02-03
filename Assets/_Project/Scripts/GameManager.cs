using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] TilePlacementController tilePlacementController;

        private StateMachine gameflowStateMachine;
        private MainMenuState mainMenuState;
        private GameplayState gameplayState;
        private GameOverState gameOverState;

        void Awake()
        {
            Instance = this;
            gameflowStateMachine = new StateMachine();
            mainMenuState = new MainMenuState(gameflowStateMachine);
            gameplayState = new GameplayState(gameflowStateMachine, tilePlacementController);
            gameOverState = new GameOverState(gameflowStateMachine);
        }

        void Start()
        {
            SwitchToMainMenuState();
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
        public void SwitchToTowerSelectionSubState()
        {
            gameplayState.SwitchToTowerSelectionState();
        }

        #endregion
    }
}
