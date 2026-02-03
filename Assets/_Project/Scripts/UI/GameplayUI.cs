using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormatableTextNS;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] TilePlacementController tilePlacementController;
        [SerializeField] FormatableText phaseText;
        [SerializeField] Transform tileSelectionButtonsParent;
        [SerializeField] Transform towerSelectionButtonsParent;

        [Header("Panels")]
        [SerializeField] AnimatedPanel topPanel;
        [SerializeField] AnimatedPanel bottomPanel;
        [SerializeField] ScaleUpPanel tileRotationPanel;
        [SerializeField] ScaleUpPanel nextPhasePanel;

        [Header("Prefabs")]
        [SerializeField] SelectionButton selectionButtonPrefab;

        private List<SelectionButton> currentTileSelectionButtons = new List<SelectionButton>();
        private List<SelectionButton> currentTowerSelectionButtons = new List<SelectionButton>();

        void OnEnable()
        {
            GameflowEvents.OnEnteredGameplayState += topPanel.Show;
            GameflowEvents.OnEnteredGameplayState += bottomPanel.Show;

            GameflowEvents.OnExitedGameplayState += topPanel.Hide;
            GameflowEvents.OnExitedGameplayState += bottomPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += ResetTileSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += EnableTileSelectionButtons;

            GameplayStateFlowEvents.OnEnteredTileRotationSubGameplayState += tileRotationPanel.Show;
            GameplayStateFlowEvents.OnExitedTileRotationSubGameplayState += tileRotationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += EnableTowerSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += ResetTowerSelectionButtons;
        }

        void OnDisable()
        {
            GameflowEvents.OnEnteredGameplayState -= topPanel.Show;
            GameflowEvents.OnEnteredGameplayState -= bottomPanel.Show;

            GameflowEvents.OnExitedGameplayState -= topPanel.Hide;
            GameflowEvents.OnExitedGameplayState -= bottomPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= ResetTileSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= EnableTileSelectionButtons;

            GameplayStateFlowEvents.OnEnteredTileRotationSubGameplayState -= tileRotationPanel.Show;
            GameplayStateFlowEvents.OnExitedTileRotationSubGameplayState -= tileRotationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= EnableTowerSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= ResetTowerSelectionButtons;
        }

        void Start()
        {
            topPanel.Hide();
            bottomPanel.Hide();
            tileRotationPanel.Hide();
            nextPhasePanel.Hide();
        }

        void DestroyAllChildren(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }


        public void SetPhaseText(string value)
        {
            phaseText.FillText(value);
        }

        void EnableTileSelectionButtons()
        {
            tileSelectionButtonsParent.gameObject.SetActive(true);
            towerSelectionButtonsParent.gameObject.SetActive(false);
        }

        void EnableTowerSelectionButtons()
        {
            tileSelectionButtonsParent.gameObject.SetActive(false);
            towerSelectionButtonsParent.gameObject.SetActive(true);
        }

        public async UniTask TransitionFromTileToTower()
        {
            topPanel.Hide();
            bottomPanel.Hide();

            await UniTask.WaitForSeconds(.5f);

            topPanel.Show();
            bottomPanel.Show();
        }

        void DeselectSelectionButtons(List<SelectionButton> selectionButtons)
        {
            foreach (SelectionButton selectionButton in selectionButtons)
            {
                selectionButton.Deselect();
            }
        }

        void ResetTileSelectionButtons()
        {
            DestroyAllChildren(tileSelectionButtonsParent);
            currentTileSelectionButtons.Clear();

            foreach (HexTile hexTile in tilePlacementController.GetHexTilesForPlacement())
            {
                SelectionButton selectionButton = Instantiate(selectionButtonPrefab, tileSelectionButtonsParent);
                currentTileSelectionButtons.Add(selectionButton);

                selectionButton.Button.onClick.AddListener(() =>
                {
                    tilePlacementController.SetSelectedHexTile(hexTile);
                    DeselectSelectionButtons(currentTileSelectionButtons);
                    selectionButton.Select();
                });
            }
        }

        void ResetTowerSelectionButtons()
        {
            DestroyAllChildren(towerSelectionButtonsParent);
            // currentTileSelectionButtons.Clear();

            // foreach (HexTile hexTile in tilePlacementController.GetHexTilesForPlacement())
            // {
            //     SelectionButton selectionButton = Instantiate(selectionButtonPrefab, tileSelectionButtonsParent);
            //     currentTileSelectionButtons.Add(selectionButton);

            //     selectionButton.Button.onClick.AddListener(() =>
            //     {
            //         tilePlacementController.SetSelectedHexTile(hexTile);
            //         DeselectSelectionButtons(currentTileSelectionButtons);
            //         selectionButton.Select();
            //     });
            // }
        }
    }
}
