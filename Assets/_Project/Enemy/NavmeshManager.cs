using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;

namespace FG_GP2_T3
{
    public class NavmeshManager : MonoBehaviour
    {
        public static NavmeshManager Instance;
        [SerializeField] NavMeshSurface navMeshSurface;

        void Awake()
        {
            Instance = this;
        }

        [Button]
        public void RebakeNavmesh()
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}
