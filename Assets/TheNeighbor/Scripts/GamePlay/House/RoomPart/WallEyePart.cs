using Trellcko.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.House
{
    public class WallEyePart : MonoBehaviour
    {
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
            Vector3 targetPosition = _playerFacade.transform.position;

            Debug.LogError($"TARGET POSITION: {targetPosition}");

            Plane eyePlane = new Plane(_center.forward, _center.position);

            Vector3 projectedTarget =
                eyePlane.ClosestPointOnPlane(targetPosition);

            Debug.LogError($"PROJECTED TARGET: {projectedTarget}");

            Vector3 fromCenter =
                projectedTarget - _center.position;

            Debug.LogError($"FROM CENTER: {fromCenter}");

            float x = Vector3.Dot(fromCenter, _center.right);
            float y = Vector3.Dot(fromCenter, _center.up);

            Debug.LogError($"DOT X: {x}");
            Debug.LogError($"DOT Y: {y}");

            Vector2 direction = new Vector2(x, y);

            Debug.LogError($"RAW DIRECTION: {direction}");

            if (direction.sqrMagnitude > 0.0001f)
            {
                direction.Normalize();
            }
            else
            {
                direction = Vector2.zero;
            }

            Debug.LogError($"NORMALIZED DIRECTION: {direction}");

            Vector2 offset = new Vector2(
                direction.x * _xRadius,
                direction.y * _yRadius
            );

            Debug.LogError($"OFFSET: {offset}");

            Vector3 targetWorldPos =
                _center.position +
                _center.right * offset.x +
                _center.up * offset.y;

            Debug.LogError($"TARGET WORLD POS: {targetWorldPos}");
            Debug.LogError($"CURRENT POS: {transform.position}");

            transform.position = Vector3.Lerp(
                transform.position,
                targetWorldPos,
                Time.deltaTime * _speed
            );
        }

        private void OnDrawGizmos()
        {
            if (_center == null)
                return;

            Gizmos.color = Color.red;
            Gizmos.DrawRay(_center.position, _center.right);

            Gizmos.color = Color.green;
            Gizmos.DrawRay(_center.position, _center.up);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(_center.position, _center.forward);
            
            Gizmos.color = Color.green;

            Vector3 previousPoint = GetPoint(0f);

            for (int i = 1; i <= _segments; i++)
            {
                float angle = i / (float)_segments * Mathf.PI * 2f;
                Vector3 currentPoint = GetPoint(angle);

                Gizmos.DrawLine(previousPoint, currentPoint);

                previousPoint = currentPoint;
            }
        }
        private Vector2 ClampToEllipse(Vector2 point)
        {
            float value =
                (point.x * point.x) / (_xRadius * _xRadius) +
                (point.y * point.y) / (_yRadius * _yRadius);

            if (value <= 1f)
                return point;

            return point / Mathf.Sqrt(value);
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