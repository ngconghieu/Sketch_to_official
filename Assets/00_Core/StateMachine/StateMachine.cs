public class StateMachine
{
    private State _currentState;
    public StateMachine(State rootState)
    {
        _currentState = rootState;
    }

    public void Tick(float deltaTime)
    {
        _currentState.Update(deltaTime);
    }

    public void FixedTick(float deltaTime)
    {
        _currentState.FixedUpdate(deltaTime);
    }
}