using System.Collections.Generic;
using BrunoMikoski.AnimationSequencer;
using FormatableTextNS;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class GameplayUI : MonoBehaviour
    {
        [Header("Top Bar")]
        [SerializeField] private GameObject _topBar;
        [SerializeField] private FormatableText _txtPhase;

        [Header("Bottom Bar")]
        [SerializeField] private GameObject _bottomBar;
        [SerializeField] private GameObject _tilePlacementPanel;
        [SerializeField] private GameObject _towerPlacementPanel;
        [SerializeField] private Transform tilePlacementButtonsParent;
        [SerializeField] private Transform towerPlacementButtonsParent;
        [SerializeField] private GameObject selectionButtonPrefab;

        [Header("Others")]
        [SerializeField] private Button _btnNextRound;
        [SerializeField] private GameObject _rotationPopup;
        [SerializeField] private Button _btnCancelRotation;
        [SerializeField] private Button _btnRotate;
        [SerializeField] private Button _btnConfirmRotation;

        [SerializeField] AnimationSequencerController topBarAnimation;
        [SerializeField] AnimationSequencerController bottomBarAnimation;

        void Awake()
        {
            // animationSequencerController.PlayBackwards();
            topBarAnimation.Play();
            bottomBarAnimation.Play();
        }
    }
}
