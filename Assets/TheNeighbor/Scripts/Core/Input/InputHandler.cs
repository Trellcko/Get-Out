using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Trellcko.Core.Input
{
    public class InputHandler : IInputHandler, IDisposable
    {
        private readonly InputSystemActions _actions = new();
        private float _intensity = 1f;

        public Vector2 GetMoveVector() => _actions.Player.Move.ReadValue<Vector2>();
        public Vector2 GetMouseDelta() => _actions.Player.Rotation.ReadValue<Vector2>() * _intensity;
        public Vector2 GetMousePosition() => Mouse.current.position.ReadValue();

        public event Action<Vector2> Moved;
        public event Action MovedCanceled;
        public event Action Sprint;
        public event Action SprintCanceled;
        public event Action EscapePressed;
        public event Action<Vector2> MouseMoved;
        public event Action Interacted;
        public event Action SpaceClicked;

        public InputHandler()
        {
            _actions.Enable();
            _actions.Player.Rotation.performed += OnRotationPerformed;
            _actions.Player.Move.performed += OnMovePerformed;
            _actions.Player.Move.canceled += OnMoveCanceled;
            _actions.Player.Interact.performed += OnInteracted;
            _actions.Player.Sprint.performed += OnSprintPerformed;
            _actions.Player.Sprint.canceled += OnSprintCanceled;
            _actions.Player.Space.performed += OnSpacePerformed;
            _actions.Player.MainMenu.performed += OnPerformed;
        }

        public void Dispose()
        {
            _actions.Player.Move.performed -= OnMovePerformed;
            _actions.Player.Rotation.performed -= OnRotationPerformed;
            _actions.Player.Move.canceled -= OnMoveCanceled;
            _actions.Player.Interact.performed -= OnInteracted;
            _actions.Player.Sprint.performed -= OnSprintPerformed;
            _actions.Player.Sprint.canceled -= OnSprintCanceled;
            _actions.Player.Space.performed -= OnSpacePerformed;
            _actions.Player.MainMenu.performed -= OnPerformed;
        }

        public void SetMouseIntensity(float intensity)
        {
            _intensity = intensity;
        }

        private void OnPerformed(InputAction.CallbackContext obj)
        {
            EscapePressed?.Invoke();
        }

        private void OnSpacePerformed(InputAction.CallbackContext obj)
        {
            SpaceClicked?.Invoke();
        }

        private void OnInteracted(InputAction.CallbackContext obj)
        {
            Interacted?.Invoke();
        }

        private void OnMoveCanceled(InputAction.CallbackContext obj)
        {
            MovedCanceled?.Invoke();
        }

        private void OnMovePerformed(InputAction.CallbackContext obj)
        {
            Moved?.Invoke(obj.ReadValue<Vector2>());
        }

        private void OnRotationPerformed(InputAction.CallbackContext obj)
        {
            MouseMoved?.Invoke(obj.ReadValue<Vector2>());
        }

        private void OnSprintCanceled(InputAction.CallbackContext obj)
        {
            SprintCanceled?.Invoke();
        }

        private void OnSprintPerformed(InputAction.CallbackContext obj)
        {
            Sprint?.Invoke();
        }
    }
}