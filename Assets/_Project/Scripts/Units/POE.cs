using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FG_GP2_T3
{
	public class POE : MonoBehaviour
	{
		[SerializeField] private float _health;
		public float Health => _health;

		private CapsuleCollider _collider;
		public CapsuleCollider Collider => _collider;

		Color _originalColor;
        Renderer[] _renderer;

		private void Awake()
		{
			_collider = GetComponentInChildren<CapsuleCollider>();
			_renderer = GetComponentsInChildren<Renderer>();
			foreach(Renderer rend in _renderer)
				 if (rend != null) _originalColor = rend.material.color;
		}

		public void TakeDamage(float damage)
		{
			_health -= damage;

			if(_health <= 0)
			{
				GameManager.Instance.SwitchToGameOverState();
				Destroy(gameObject);
				return;
			}

			StartCoroutine(FlashRedCoroutine());
		}

		private IEnumerator FlashRedCoroutine()
        {
			foreach(Renderer rend in _renderer)
            {
                if (rend == null) continue;
                rend.material.color = Color.red;
            }

            yield return new WaitForSeconds(0.1f);

            foreach(Renderer rend in _renderer)
			{
				if (rend == null) continue;
				rend.material.color = _originalColor;
			}
        }
	}
}
