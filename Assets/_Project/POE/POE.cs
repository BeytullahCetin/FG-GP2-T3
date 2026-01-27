using UnityEngine;

namespace FG_GP2_T3
{
	public class POE : MonoBehaviour
	{
		private Health health;
		public Health Health => health;

		void Awake()
		{
			health = GetComponent<Health>();
		}
	}
}
