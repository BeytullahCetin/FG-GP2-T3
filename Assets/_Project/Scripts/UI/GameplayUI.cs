using System.Collections.Generic;
using FormatableTextNS;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class GameplayUI : MonoBehaviour
    {
        [SerializeField] private GameObject _topBar;
        [SerializeField] private GameObject _bottomBar;
        [SerializeField] private GameObject _rotationPopup;
        [SerializeField] private GameObject _tilePlacementPanel;
        [SerializeField] private GameObject _towerPlacementPanel;

        [SerializeField] private FormatableText _txtPhase;
        [SerializeField] private Button _btnNextRound;

        [SerializeField] private Button _btnCancelRotation;
        [SerializeField] private Button _btnRotate;
        [SerializeField] private Button _btnConfirmRotation;

        [SerializeField] List<SelectionButton> _tileButtons = new List<SelectionButton>();
        [SerializeField] List<SelectionButton> _towerButtons = new List<SelectionButton>();

        public FormatableText TxtPhase => _txtPhase;
        public Button BtnNextRound => _btnNextRound;
        public GameObject RotationPopup => _rotationPopup;

        public List<SelectionButton> TileButtons => _tileButtons;
        public List<SelectionButton> TowerButtons => _towerButtons;

        public GameObject TilePlacementPanel => _tilePlacementPanel;
        public GameObject TowerPlacementPanel => _towerPlacementPanel;

        public Button BtnCancelRotation => _btnCancelRotation;
        public Button BtnRotate => _btnRotate;
        public Button BtnConfirmRotation => _btnConfirmRotation;

    }
}
