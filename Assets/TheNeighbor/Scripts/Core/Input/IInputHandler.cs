using System;
using UnityEngine;

namespace Trellcko.Core.Input
{
    public interface IInputHandler
    {
        Vector2 GetMoveVector();
        Vector2 GetMouseDelta();
        Vector2 GetMousePosition();
        event Action<Vector2> Moved;
        event Action MovedCanceled;
        event Action Sprint;
        event Action SprintCanceled;
        event Action EscapePressed;
        event Action<Vector2> MouseMoved;
        event Action Interacted;
        event Action SpaceClicked;
        void SetMouseIntensity(float intensity);
    }
}