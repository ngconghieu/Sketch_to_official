using UnityEngine;

public interface IInputReader
{
    Vector2 Move { get; }
    InputBuffer Jump { get; }
    InputBuffer Dash { get; }
    InputBuffer Attack { get; }
}
