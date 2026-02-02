using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] Button tapToScreenButton;
        [SerializeField] TMP_Text tapToScreenText;

        [SerializeField] float scale = 1.2f;
        [SerializeField] float duration = .5f;
        [SerializeField] Ease ease = Ease.InOutBack;

        void OnEnable()
        {
            GameflowEvents.OnEnteredMainMenuState += EnableTapToScreen;
            GameflowEvents.OnExitedMainMenuState += DisableTapToScreen;
        }

        void OnDisable()
        {
            GameflowEvents.OnEnteredMainMenuState -= EnableTapToScreen;
            GameflowEvents.OnExitedMainMenuState -= DisableTapToScreen;
        }

        void Start()
        {
            tapToScreenButton.onClick.AddListener(() =>
            {
                GameManager.Instance.SwitchToGameplayState();
            });

            tapToScreenText.transform
                .DOScale(scale, duration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }

        void EnableTapToScreen()
        {
            tapToScreenButton.gameObject.SetActive(true);
        }

        void DisableTapToScreen()
        {
            tapToScreenButton.gameObject.SetActive(false);
        }
    }
}
