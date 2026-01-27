using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace FG_GP2_T3
{
	public class Enemy : MonoBehaviour
	{
		private Health health;
		private NavMeshAgent agent;

		[SerializeField] private Transform destination;
		[SerializeField] private POE poe;

		private void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();
		}

		[Button]
		private async UniTask StartEnemyBehaviour()
		{
			SetDestination();
			// Wait for navmesh updates itself.
			await UniTask.WaitForEndOfFrame();
			await WaitUntilReachDestination();
			await StartAttack();
		}

		private async UniTask WaitUntilReachDestination()
		{
			while (agent.remainingDistance >= agent.stoppingDistance)
			{
				Debug.Log(agent.remainingDistance);
				await UniTask.WaitForSeconds(1f);
			}

			Debug.Log("Agent is close enough!");
		}

		private async UniTask StartAttack()
		{
			Debug.Log("Attack Started!");

			while (true)
			{
				if (health.IsAlive == false || poe.Health.IsAlive == false)
					break;

				poe.Health.TakeDamage(20);
				await UniTask.WaitForSeconds(1f);
			}

			Debug.Log("Attack Ended!");
		}

		private void SetDestination()
		{
			agent.SetDestination(destination.position);
			Debug.Log("Destination set");
		}
	}
}
