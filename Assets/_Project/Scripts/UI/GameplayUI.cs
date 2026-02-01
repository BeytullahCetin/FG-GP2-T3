using System.Collections.Generic;
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

        void OnEnable()
        {
            GameflowEvents.OnEnteredGameplayState += topPanel.Show;
            GameflowEvents.OnEnteredGameplayState += bottomPanel.Show;

            GameflowEvents.OnExitedGameplayState += topPanel.Hide;
            GameflowEvents.OnExitedGameplayState += bottomPanel.Hide;

            GameflowEvents.OnEnteredTileSelectionSubGameplayState += ResetTileSelectionButtons;
            GameflowEvents.OnEnteredTileSelectionSubGameplayState += EnableTileSelectionButtons;
            GameflowEvents.OnExitedTileSelectionSubGameplayState += bottomPanel.Hide;
        }

        void OnDisable()
        {
            GameflowEvents.OnEnteredGameplayState -= topPanel.Show;
            GameflowEvents.OnEnteredGameplayState -= bottomPanel.Show;

            GameflowEvents.OnExitedGameplayState -= topPanel.Hide;
            GameflowEvents.OnExitedGameplayState -= bottomPanel.Hide;

            GameflowEvents.OnEnteredTileSelectionSubGameplayState -= ResetTileSelectionButtons;
            GameflowEvents.OnEnteredTileSelectionSubGameplayState -= EnableTileSelectionButtons;
            GameflowEvents.OnExitedTileSelectionSubGameplayState -= bottomPanel.Hide;
        }

        void Start()
        {
            topPanel.Hide();
            bottomPanel.Hide();
            tileRotationPanel.Hide();
            nextPhasePanel.Hide();
        }

        void EnableTileSelectionButtons()
        {
            tileSelectionButtonsParent.gameObject.SetActive(true);
            towerSelectionButtonsParent.gameObject.SetActive(false);
        }

        void EnableTowerSelectionButtons()
        {
            towerSelectionButtonsParent.gameObject.SetActive(true);
            tileSelectionButtonsParent.gameObject.SetActive(false);
        }

        void ResetTileSelectionButtons()
        {
            foreach (Transform child in tileSelectionButtonsParent)
            {
                Destroy(child.gameObject);
            }

            foreach (HexTile hexTile in tilePlacementController.GetHexTilesForPlacement())
            {
                SelectionButton selectionButton = Instantiate(selectionButtonPrefab, tileSelectionButtonsParent);
                selectionButton.Button.onClick.AddListener(() =>
                {
                    tilePlacementController.SetSelectedHexTile(hexTile);
                });
            }
        }
    }
}
