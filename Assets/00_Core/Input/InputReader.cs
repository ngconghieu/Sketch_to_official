using UnityEngine;

public class InputReader : IInputReader
{
    public Vector2 Move { get; set; }

    public InputBuffer JumpBuffer = new();
    public InputBuffer DashBuffer = new();
    public InputBuffer AttackBuffer = new();

    public InputBuffer Jump => JumpBuffer;
    public InputBuffer Dash => DashBuffer;
    public InputBuffer Attack => AttackBuffer;

    public void Tick(float deltaTime)
    {
        JumpBuffer.Tick(deltaTime);
        DashBuffer.Tick(deltaTime);
        AttackBuffer.Tick(deltaTime);
    }
}