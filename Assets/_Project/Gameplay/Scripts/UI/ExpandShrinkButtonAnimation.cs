using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

class ExpandShrinkButtonAnimation : MonoBehaviour
{
	[SerializeField] float size = 1.2f;
	[SerializeField] float duration = .5f;
	[SerializeField] Ease ease = Ease.InOutBack;

	private Image icon;

	void Awake()
	{
		icon = GetComponent<Image>();
	}

	void Start()
	{
		icon.transform.DOScale(size, duration).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
	}
}