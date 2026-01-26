using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace FG_GP2_T3
{
	public class Enemy : MonoBehaviour
	{
		Health health;
		NavMeshAgent agent;

		[SerializeField] Transform destination;
		[SerializeField] Health towerHealth;

		void Awake()
		{
			agent = GetComponent<NavMeshAgent>();
			health = GetComponent<Health>();

			health.OnDead += TestFunction;
		}

		void TestFunction()
		{
			Debug.Log("OnDead Test");
		}

		async UniTaskVoid DebugRemainingDistance()
		{
			await UniTask.WaitForEndOfFrame();
			while (agent.remainingDistance > agent.stoppingDistance)
			{
				Debug.Log(agent.remainingDistance);
				await UniTask.WaitForSeconds(1f);
			}

			Debug.Log("Agent is close enough!");
			StartAttack().Forget();
		}

		async UniTaskVoid StartAttack()
		{
			while (health.IsAlive == true && towerHealth.IsAlive == true)
			{
				towerHealth.TakeDamage(5);
				await UniTask.WaitForSeconds(1f);
			}
		}

		[Button]
		void SetDestination()
		{
			agent.SetDestination(destination.position);
			DebugRemainingDistance().Forget();
		}
	}
}
