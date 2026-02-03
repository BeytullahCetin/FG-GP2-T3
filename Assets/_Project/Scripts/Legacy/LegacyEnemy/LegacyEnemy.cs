using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace FG_GP2_T3
{
	public class LegacyEnemy : MonoBehaviour
	{
		private Health _health;
		private NavMeshAgent _agent;
		private POE _poe;

		public Health Health => _health;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_health = GetComponent<Health>();
			_poe = LegacyEnemyManager.Instance.Poe;

			_health.OnDead += Die;

			StartEnemyBehaviour().Forget();
		}

		public void Die()
		{
			Destroy(gameObject);
		}

		[Button]
		public async UniTaskVoid StartEnemyBehaviour()
		{
			try
			{
				await SetDestination();
				await WaitUntilReachDestination();
				await StartAttack();
			}
			catch (Exception ex)
			{
				Debug.Log("Exception handled!");
			}
		}

		private async UniTask WaitUntilReachDestination()
		{
			while (_agent.remainingDistance >= _agent.stoppingDistance)
			{
				Debug.Log(_agent.remainingDistance);
				await UniTask.WaitForSeconds(1f);
			}

			_agent.ResetPath();
			Debug.Log("Agent is close enough!");
		}

		private async UniTask StartAttack()
		{
			Debug.Log("Attack Started!");

			while (true)
			{
				if (_health.IsAlive == false || _poe.Health <= 0)
					break;

				_poe.TakeDamage(20);
				await UniTask.WaitForSeconds(1f);
			}

			Debug.Log("Attack Ended!");
		}

		public async UniTask SetDestination()
		{
			_agent.SetDestination(_poe.transform.position);
			await UniTask.WaitUntil(() => _agent.hasPath == true);
			Debug.Log("Destination set");
		}
	}
}
