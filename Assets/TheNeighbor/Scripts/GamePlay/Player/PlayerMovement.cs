using Trellcko.Core.Input;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Transform _forward;
        [SerializeField] private float _speed;
        [SerializeField] private float _runningSpeed = 5f;
        
        private IInputHandler _inputHandler;

        private float _currentSpeed;

        public bool IsEnabled { get; set; } = true;
        
        [Inject]
        private void Inject(IInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Awake()
        {
            _currentSpeed = _speed;
        }

        private void OnEnable()
        {
            _inputHandler.Sprint += OnSprint;
            _inputHandler.SprintCanceled += OnSprintCanceled;
        }

        private void OnDisable()
        {
            _inputHandler.Sprint -= OnSprint;
            _inputHandler.SprintCanceled -= OnSprintCanceled;
        }

        private void FixedUpdate()
        {
            if(IsEnabled)
                Move(_inputHandler.GetMoveVector());
			_rb.linearVelocity = Vector3.zero;
        }

        private void OnSprintCanceled()
        {
            _currentSpeed = _speed;
        }

        private void OnSprint()
        {
            _currentSpeed = _runningSpeed;
        }

        private void Move(Vector2 inputVector)
        {
            Vector3 moveVector =  (_forward.forward * inputVector.y + _forward.right * inputVector.x);

            moveVector.y = 0;
            moveVector.Normalize();
            
            _rb.MovePosition( _rb.position + _currentSpeed * Time.fixedDeltaTime * moveVector);
        }
    }
}