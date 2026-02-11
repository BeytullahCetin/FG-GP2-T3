using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FormatableTextNS;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GameplayUI : MonoBehaviour
    {
        public TowerInfoUI TowerInfoUI => towerInfoUI;
        [SerializeField] TilePlacementController tilePlacementController;
        [SerializeField] TowerPlacementController towerPlacementController;
        [SerializeField] TowerInfoUI towerInfoUI;

        [SerializeField] FormatableText phaseText;
        [SerializeField] Transform tileSelectionButtonsParent;
        [SerializeField] Transform towerSelectionButtonsParent;

        [Header("Panels")]
        [SerializeField] AnimatedPanel topPanel;
        [SerializeField] AnimatedPanel bottomPanel;
        [SerializeField] ScaleUpPanel tileRotationPanel;
        [SerializeField] ScaleUpPanel towerConfirmationPanel;
        [SerializeField] ScaleUpPanel nextPhasePanel;
        [SerializeField] ScaleUpPanel rerollTowersPanel;
        [SerializeField] ScaleUpPanel speedUpPanel;

        [Header("Prefabs")]
        [SerializeField] SelectionButton tileSelectionButtonPrefab;
        [SerializeField] TowerSelectionButton towerSelectionButtonPrefab;

        private List<SelectionButton> currentTileSelectionButtons = new List<SelectionButton>();
        private List<SelectionButton> currentTowerSelectionButtons = new List<SelectionButton>();

        void OnEnable()
        {
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += topPanel.Show;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += bottomPanel.Show;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += nextPhasePanel.Hide;

            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += ResetTileSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState += EnableTileSelectionButtons;

            GameplayStateFlowEvents.OnEnteredTileRotationSubGameplayState += tileRotationPanel.Show;
            GameplayStateFlowEvents.OnExitedTileRotationSubGameplayState += tileRotationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += EnableTowerSelectionButtons;
            // GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += ResetTowerSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += nextPhasePanel.Show;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += rerollTowersPanel.Show;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += towerPlacementController.UpdateRerollButton;

            GameplayStateFlowEvents.OnEnteredTowerPlacementConfirmationSubGameplayState += towerConfirmationPanel.Show;
            GameplayStateFlowEvents.OnExitedTowerPlacementConfirmationSubGameplayState += towerConfirmationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerFusionConfirmationSubGameplayState += towerConfirmationPanel.Show;
            GameplayStateFlowEvents.OnExitedTowerFusionConfirmationSubGameplayState += towerConfirmationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredEnemyWaveState += topPanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState += bottomPanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState += nextPhasePanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState += rerollTowersPanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState += speedUpPanel.Show;
            GameplayStateFlowEvents.OnExitedEnemyWaveState += speedUpPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerFusionConfirmationSubGameplayState += towerConfirmationPanel.Show;
            GameplayStateFlowEvents.OnExitedTowerFusionConfirmationSubGameplayState += towerConfirmationPanel.Hide;
        }

        void OnDisable()
        {
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= topPanel.Show;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= bottomPanel.Show;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= nextPhasePanel.Hide;

            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= ResetTileSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTileSelectionSubGameplayState -= EnableTileSelectionButtons;

            GameplayStateFlowEvents.OnEnteredTileRotationSubGameplayState -= tileRotationPanel.Show;
            GameplayStateFlowEvents.OnExitedTileRotationSubGameplayState -= tileRotationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= EnableTowerSelectionButtons;
            // GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= ResetTowerSelectionButtons;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= nextPhasePanel.Show;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= rerollTowersPanel.Show;
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= towerPlacementController.UpdateRerollButton;

            GameplayStateFlowEvents.OnEnteredTowerPlacementConfirmationSubGameplayState -= towerConfirmationPanel.Show;
            GameplayStateFlowEvents.OnExitedTowerPlacementConfirmationSubGameplayState -= towerConfirmationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerFusionConfirmationSubGameplayState -= towerConfirmationPanel.Show;
            GameplayStateFlowEvents.OnExitedTowerFusionConfirmationSubGameplayState -= towerConfirmationPanel.Hide;

            GameplayStateFlowEvents.OnEnteredEnemyWaveState -= topPanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState -= bottomPanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState -= nextPhasePanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState -= rerollTowersPanel.Hide;
            GameplayStateFlowEvents.OnEnteredEnemyWaveState -= speedUpPanel.Show;
            GameplayStateFlowEvents.OnExitedEnemyWaveState -= speedUpPanel.Hide;

            GameplayStateFlowEvents.OnEnteredTowerFusionConfirmationSubGameplayState -= towerConfirmationPanel.Show;
            GameplayStateFlowEvents.OnExitedTowerFusionConfirmationSubGameplayState -= towerConfirmationPanel.Hide;
        }

        void Start()
        {
            topPanel.Hide();
            bottomPanel.Hide();
            tileRotationPanel.Hide();
            towerConfirmationPanel.Hide();
            nextPhasePanel.Hide();
            rerollTowersPanel.Hide();
            speedUpPanel.Hide();
            towerInfoUI.Hide();
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

        public void DeselectSelectionButtons(List<SelectionButton> selectionButtons)
        {
            foreach (SelectionButton selectionButton in selectionButtons)
            {
                selectionButton.Deselect();
            }
        }

        public void DeselectTowerSelectionButtons()
        {
            DeselectSelectionButtons(currentTowerSelectionButtons);
        }

        void ResetTileSelectionButtons()
        {
            DestroyAllChildren(tileSelectionButtonsParent);
            currentTileSelectionButtons.Clear();

            foreach (HexTileData hexTileData in tilePlacementController.GetHexTilesForPlacement())
            {
                SelectionButton selectionButton = Instantiate(tileSelectionButtonPrefab, tileSelectionButtonsParent);
                selectionButton.Title.SetText(hexTileData.name);
                selectionButton.Image.sprite = hexTileData.TileIcon;
                currentTileSelectionButtons.Add(selectionButton);

                selectionButton.Button.onClick.AddListener(() =>
                {
                    tilePlacementController.SetSelectedHexTile(hexTileData);
                    DeselectSelectionButtons(currentTileSelectionButtons);
                    selectionButton.Select();
                });

            }

            DeselectSelectionButtons(currentTileSelectionButtons);
        }


        [Button]
        public void ResetTowerSelectionButtons()
        {
            DestroyAllChildren(towerSelectionButtonsParent);
            currentTowerSelectionButtons.Clear();

            foreach (TowerData towerData in towerPlacementController.GetTowerDatasForPlacement())
            {
                TowerSelectionButton selectionButton = Instantiate(towerSelectionButtonPrefab, towerSelectionButtonsParent);
                selectionButton.SetTowerData(towerData);
                selectionButton.Title.SetText(towerData.TowerName);
                selectionButton.Image.sprite = towerData.TowerIcon;
                selectionButton.CostText.FillText(towerData.Cost.ToString());
                selectionButton.UnaffordableCostText.FillText(towerData.Cost.ToString());
                currentTowerSelectionButtons.Add(selectionButton);


                selectionButton.Button.onClick.AddListener(() =>
                {
                    towerPlacementController.SetSelectedTower(towerData);
                    DeselectSelectionButtons(currentTowerSelectionButtons);
                    selectionButton.Select();
                    towerInfoUI.SetTowerInfo(towerData);
                    towerInfoUI.Show();
                });
            }

            DeselectSelectionButtons(currentTowerSelectionButtons);
            UpdateTowerSelectionButtons();
        }

        public void UpdateTowerSelectionButtons()
        {
            foreach (TowerSelectionButton selectionButton in currentTowerSelectionButtons)
            {
                bool isAffordable = CompostManager.Instance.CurrentCompostAmount >= selectionButton.TowerData.Cost;
                selectionButton.Button.interactable = isAffordable;
                selectionButton.CostText.gameObject.SetActive(isAffordable);
                selectionButton.UnaffordableCostText.gameObject.SetActive(!isAffordable);
            }
        }
    }
}
