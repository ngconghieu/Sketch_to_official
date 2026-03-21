public abstract class State
{
    protected StateMachine machine;
    public State Parent;
    public State ActiveChild;

    public State(State parent = null)
    {
        this.Parent = parent;
    }

    public virtual State InitialState => null;
    public virtual State GetTransition() => null;

    // Lifecycle hooks
    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    protected virtual void OnUpdate(float deltaTime) { }

    public void Enter()
    {
        if (Parent != null) Parent.ActiveChild = this;
        var state = InitialState;
        state?.Enter();
    }

    public void Update(float deltaTime)
    {
        var state = GetTransition();
        if (state != null)
        {
            
        }
    }

    public void FixedUpdate(float deltaTime)
    {
    }
    public void Exit()
    {
        ActiveChild?.Exit();
        ActiveChild = null;
    }


}