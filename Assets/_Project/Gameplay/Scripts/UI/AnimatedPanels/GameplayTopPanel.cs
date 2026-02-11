using DG.Tweening;
using TMPro;
using UnityEngine;

namespace FG_GP2_T3
{
    public class GameplayTopPanel : ScaleUpPanel
    {
        [Header("Text Animation")]
        [SerializeField] TMP_Text topPanelText;
        [SerializeField] float bounceScale = 1.1f;
        [SerializeField] float bounceDuration = 1f;
        [SerializeField] Ease bounceEase = Ease.InOutCubic;

        private Tween textTween;

        public override void ShowLogic()
        {
            base.ShowLogic();

            textTween?.Kill();

            if (topPanelText != null)
            {
                topPanelText.transform.localScale = Vector3.one;
                textTween = topPanelText.transform.DOScale(bounceScale, bounceDuration)
                    .SetEase(bounceEase)
                    .SetLoops(-1, LoopType.Yoyo);
            }
        }

        public override void HideLogic()
        {
            base.HideLogic();

            textTween?.Kill();

            if (topPanelText != null)
            {
                topPanelText.transform.localScale = Vector3.one;
            }
        }
    }
}