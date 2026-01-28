using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace FG_GP2_T3
{
	public class Enemy : MonoBehaviour
	{
		private Health _health;
		private NavMeshAgent _agent;
		private POE _poe;

		private void Awake()
		{
			_agent = GetComponent<NavMeshAgent>();
			_health = GetComponent<Health>();
			_poe = EnemyManager.Instance.Poe;

			StartEnemyBehaviour().Forget();
		}

		[Button]
		public async UniTaskVoid StartEnemyBehaviour()
		{
			SetDestination();
			// Wait for navmesh updates itself.
			await UniTask.WaitForEndOfFrame();
			await WaitUntilReachDestination();
			await StartAttack();
		}

		private async UniTask WaitUntilReachDestination()
		{
			while (_agent.remainingDistance >= _agent.stoppingDistance)
			{
				Debug.Log(_agent.remainingDistance);
				await UniTask.WaitForSeconds(1f);
			}

			Debug.Log("Agent is close enough!");
		}

		private async UniTask StartAttack()
		{
			Debug.Log("Attack Started!");

			while (true)
			{
				if (_health.IsAlive == false || _poe.Health.IsAlive == false)
					break;

				_poe.Health.TakeDamage(20);
				await UniTask.WaitForSeconds(1f);
			}

			Debug.Log("Attack Ended!");
		}

		public void SetDestination()
		{
			_agent.SetDestination(_poe.transform.position);
			Debug.Log("Destination set");
		}
	}
}
