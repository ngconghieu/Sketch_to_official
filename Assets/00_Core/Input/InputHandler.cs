using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private InputReader _inputReader;
    private InputSystem_Actions _inputSystem;

    private void Awake()
    {
        _inputReader = new();
        _inputSystem = new();
        ServicesLocator.Register<IInputReader>(_inputReader);
    }

    private void OnDestroy()
    {
        ServicesLocator.Unregister<IInputReader>();
    }

    private void OnEnable()
    {
        _inputSystem.Enable();

        _inputSystem.Player.Move.performed += ctx => _inputReader.Move = ctx.ReadValue<Vector2>();
        _inputSystem.Player.Move.canceled += _ => _inputReader.Move = Vector2.zero;
        _inputSystem.Player.Jump.performed += _ => _inputReader.Jump.Trigger();
        _inputSystem.Player.Dash.performed += _ => _inputReader.Dash.Trigger();
        _inputSystem.Player.Attack.performed += _ => _inputReader.Attack.Trigger();
    }

    private void OnDisable()
    {
        _inputSystem.Disable();


        _inputSystem.Player.Move.performed -= ctx => _inputReader.Move = ctx.ReadValue<Vector2>();
        _inputSystem.Player.Move.canceled -= _ => _inputReader.Move = Vector2.zero;
        _inputSystem.Player.Jump.performed -= _ => _inputReader.Jump.Trigger();
        _inputSystem.Player.Dash.performed -= _ => _inputReader.Dash.Trigger();
        _inputSystem.Player.Attack.performed -= _ => _inputReader.Attack.Trigger();
    }

    private void Update()
    {
        _inputReader.Tick(Time.deltaTime);
    }
}

[System.Serializable]
public class InputBuffer
{
    private readonly float duration = 0.2f;
    private float _counter;

    public void Trigger() => _counter = duration;

    public void Tick(float deltaTime)
    {
        if (_counter > 0) _counter -= deltaTime;
    }

    public bool IsValid => _counter > 0;
    public void Consume() => _counter = 0;
}