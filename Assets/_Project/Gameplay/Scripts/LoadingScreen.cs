using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Slider loadingBar;

        [SerializeField] float loadingDuration = 2f;
        [SerializeField] Ease loadingEase = Ease.InQuart;

        [SerializeField] float canvasGroupDuration = .5f;
        [SerializeField] Ease canvasGroupEase = Ease.Linear;

        public async UniTaskVoid TriggerLoadingBar()
        {
            gameObject.SetActive(true);
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
            loadingBar.value = loadingBar.minValue;

            await loadingBar.DOValue(loadingBar.maxValue, loadingDuration)
                .SetEase(loadingEase)
                .ToUniTask();

            await canvasGroup.DOFade(0, canvasGroupDuration)
                .SetEase(canvasGroupEase)
                .ToUniTask();

            canvasGroup.blocksRaycasts = false;
            gameObject.SetActive(false);
        }
    }
}
