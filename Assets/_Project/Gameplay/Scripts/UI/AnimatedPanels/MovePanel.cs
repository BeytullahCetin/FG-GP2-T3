using DG.Tweening;
using UnityEngine;

namespace FG_GP2_T3
{
    public abstract class MovePanel : AnimatedPanel
    {
        protected Vector2 showPosition;
        protected Vector2 hidePosition;

        protected abstract void SetEnabledPosition();
        protected abstract void SetDisabledPosition();

        protected override void Awake()
        {
            base.Awake();
            SetEnabledPosition();
            SetDisabledPosition();
        }

        public override void ShowLogic()
        {
            rectTransform.anchoredPosition = hidePosition;
            rectTransform.DOAnchorPos(showPosition, duration).SetEase(showEase);
        }

        public override void HideLogic()
        {
            rectTransform.anchoredPosition = showPosition;
            rectTransform.DOAnchorPos(hidePosition, duration).SetEase(hideEase);
        }
    }
}
