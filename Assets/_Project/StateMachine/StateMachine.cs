namespace FG_GP2_T3
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public void ChangeState(State newState)
        {
            CurrentState?.Exit();

            CurrentState = newState;
            CurrentState.Enter();
        }

        public void Update()
        {
            CurrentState?.Update();
        }
    }
}