using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
	public abstract class AnimatedPanel : MonoBehaviour
	{
		[SerializeField] protected float duration = .5f;
		[SerializeField] protected Ease showEase = Ease.OutBack;
		[SerializeField] protected Ease hideEase = Ease.InBack;

		protected RectTransform rectTransform;
		private bool isShowing = true;

		public abstract void ShowLogic();
		public abstract void HideLogic();

		protected virtual void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		[Button]
		public void Show()
		{
			if (isShowing == true)
				return;

			ShowLogic();
			isShowing = true;
		}

		[Button]
		public void Hide()
		{
			if (isShowing == false)
				return;

			HideLogic();
			isShowing = false;
		}
	}
}