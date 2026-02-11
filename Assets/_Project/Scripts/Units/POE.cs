using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace FG_GP2_T3
{
	public class POE : MonoBehaviour
	{
		[SerializeField] private float _startingHealth;
		[ReadOnly][SerializeField] private float _health;
		public float Health => _health;

		private CapsuleCollider _collider;
		public CapsuleCollider Collider => _collider;

		Color _originalColor;
		Renderer[] _renderer;

		private void Awake()
		{
			_health = _startingHealth;
			_collider = GetComponent<CapsuleCollider>();
			_renderer = GetComponentsInChildren<Renderer>();
			foreach (Renderer rend in _renderer)
				if (rend != null) _originalColor = rend.material.color;
		}

		public void TakeDamage(float damage)
		{
			_health -= damage;

			EventManager.Invoke(new OnCoreDamageEvent(Mathf.RoundToInt(_health), Mathf.Clamp01(_health / _startingHealth), Mathf.RoundToInt(damage)));

			if (_health <= 0)
			{
				GameManager.Instance.SwitchToGameOverState();
				EventManager.Invoke(new OnGameEndedEvent(false));
				Destroy(gameObject);
				return;
			}

			StartCoroutine(FlashRedCoroutine());
		}

		private IEnumerator FlashRedCoroutine()
		{
			foreach (Renderer rend in _renderer)
			{
				if (rend == null) continue;
				rend.material.color = Color.red;
			}

			yield return new WaitForSeconds(0.1f);

			foreach (Renderer rend in _renderer)
			{
				if (rend == null) continue;
				rend.material.color = _originalColor;
			}
		}
	}
}
