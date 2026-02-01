using DG.Tweening;

namespace FG_GP2_T3
{
    public class ScaleUpPanel : AnimatedPanel
    {
        public override void HideLogic()
        {
            rectTransform.DOScale(0, duration).SetEase(hideEase);
        }

        public override void ShowLogic()
        {
            rectTransform.DOScale(1, duration).SetEase(showEase);
        }
    }
}
