using UnityEngine;

namespace FG_GP2_T3
{
    public abstract class State
    {
        protected StateMachine stateMachine;

        public State(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {
            Debug.Log($"<color=green>{GetType()} - Enter()</color>");
        }
        public virtual void Exit()
        {
            Debug.Log($"<color=red>{GetType()} - Exit()</color>");
        }
        public virtual void Update() { }
    }
}
