using Trellcko.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.House
{
    public class WallEyeballController : MonoBehaviour
    {
        [SerializeField] private float _lookStrength = 0.15f;
        [SerializeField] private float _yCorrection = 0f;
        
        [SerializeField] private Transform _center;
        [SerializeField] private float _xRadius;
        [SerializeField] private float _yRadius;

        [SerializeField] private float _speed;
        [SerializeField] private int _segments = 20;
        
        private PlayerFacade _playerFacade;

        [Inject]
        private void Construct(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }

        private void Update()
        {
            LookAtTarget();
        }

        private void LookAtTarget()
        {
            Vector3 targetPosition = _playerFacade.PlayerRotation.transform.position;
            Vector3 toTarget = targetPosition - _center.position;

            float x = Vector3.Dot(toTarget, _center.right);
            float y = Vector3.Dot(toTarget, _center.up);

            float distanceForward = Vector3.Dot(toTarget, _center.forward);

            if (Mathf.Abs(distanceForward) < 0.001f)
                distanceForward = 0.001f;

            Vector2 perspectiveOffset = new Vector2(
                x / Mathf.Abs(distanceForward),
                y / Mathf.Abs(distanceForward)
            );

            perspectiveOffset.y += _yCorrection;

            Vector2 offset = new Vector2(
                perspectiveOffset.x * _lookStrength,
                perspectiveOffset.y * _lookStrength
            );

            offset = ClampToEllipse(offset);

            Vector3 targetWorldPos =
                _center.position +
                _center.right * offset.x +
                _center.up * offset.y;

            transform.position = Vector3.Lerp(
                transform.position,
                targetWorldPos,
                Time.deltaTime * _speed);
        }
        private Vector2 ClampToEllipse(Vector2 point)
        {
            float ellipseValue =
                (point.x * point.x) / (_xRadius * _xRadius) +
                (point.y * point.y) / (_yRadius * _yRadius);

            if (ellipseValue <= 1f)
                return point;

            float scale = 1f / Mathf.Sqrt(ellipseValue);

            return point * scale;
        }

        private Vector3 GetPoint(float angle)
        {
            float x = Mathf.Cos(angle) * _xRadius;
            float y = Mathf.Sin(angle) * _yRadius;

            return _center.position
                   + _center.right * x
                   + _center.up * y;
        }
    }
}