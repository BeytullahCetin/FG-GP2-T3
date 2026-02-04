using System.Collections;
using UnityEngine;

namespace FG_GP2_T3
{
	public class POE : MonoBehaviour
	{
		[SerializeField] private float _health;
		public float Health => _health;

		private CapsuleCollider _collider;
		public CapsuleCollider Collider => _collider;

		private void Awake()
		{
			_collider = GetComponentInChildren<CapsuleCollider>();
		}

		public void TakeDamage(float damage)
		{
			_health -= damage;

			if(_health <= 0)
			{
				Debug.LogError("Oh no POE is Dead! :(");
				Destroy(gameObject);
				return;
			}

			StartCoroutine(FlashRedCoroutine());
		}

		private IEnumerator FlashRedCoroutine()
        {
            Renderer renderer = GetComponent<Renderer>();
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
    
            renderer.GetPropertyBlock(propBlock);
            propBlock.SetColor("_Color", Color.red);
            renderer.SetPropertyBlock(propBlock);

            yield return new WaitForSeconds(0.25f);
            if (renderer == null) yield break;

            propBlock.SetColor("_Color", Color.white);
            renderer.SetPropertyBlock(propBlock);
        }
	}
}
