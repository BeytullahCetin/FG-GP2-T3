using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

namespace FG_GP2_T3
{
    public class NavmeshManager : MonoBehaviour
    {
        [SerializeField] NavMeshSurface navMeshSurface;

        [Button]
        public void RebakeNavmesh()
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}
