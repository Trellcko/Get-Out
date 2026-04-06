using System;
using Trellcko.Core.Input;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class MopController : MonoBehaviour
    {
        private const float Step = 0.0390f; // half brush Size / 256
        [SerializeField] private MeshRenderer _target;
        [SerializeField] private Transform _mopPoint;
        [SerializeField] private Texture2D _maskTexture;
        [SerializeField] private float _sensitivity;
        [SerializeField] private int _brushSize = 100;

        public event Action MaskUpdated;
        
        private IInputHandler _inputHandler;
        private Vector3 _lastMopPosition;

        private void Start()
        {
            _lastMopPosition = _mopPoint.position;
        }

        private Camera _camera;
        private Vector3 _startPosition;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(_mopPoint.position, _brushSize);
        }

        [Inject]
        private void Construct(IInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _startPosition = transform.position;
        }

        private void OnEnable()
        {
            transform.position = _startPosition;
            _lastMopPosition = _startPosition;
        }

        private void Update()
        {
            if (MoveMop())
            {
                PaintLine(_lastMopPosition, _mopPoint.position);
                MaskUpdated?.Invoke();
                _lastMopPosition = _mopPoint.position;
            }
        }

        private void PaintLine(Vector3 lastMopPosition, Vector3 mopPointPosition)
        {
            float distance = Vector3.Distance(mopPointPosition, lastMopPosition);

            for (float t = 0; t <= 1f; t += Step / distance)
            {
                Vector3 point = Vector3.Lerp(lastMopPosition, mopPointPosition, t);
                PaintMaskAt(point);
            }
        }

        private bool MoveMop()
        {
            Vector3 mouseDelta = _inputHandler.GetMouseDelta();

            if (mouseDelta.magnitude == 0)
                return false;
            
            mouseDelta *= _sensitivity * Time.deltaTime;

            transform.position += mouseDelta;
            return true;
        }

        private void PaintMaskAt(Vector3 worldPos)
        {
            Vector3 localPos = _target.transform.InverseTransformPoint(worldPos);

            float u = localPos.x + 0.5f;
            float v = localPos.y + 0.5f;

            int px = Mathf.RoundToInt(u * _maskTexture.width);
            int py = Mathf.RoundToInt(v * _maskTexture.height);

            int halfBrush = _brushSize / 2;

            for (int x = -halfBrush; x < halfBrush; x++)
            for (int y = -halfBrush; y < halfBrush; y++)
            {
                if (x * x + y * y > _brushSize)
                    continue;

                int tx = px + x;
                int ty = py + y;

                if (tx >= 0 && tx < _maskTexture.width && ty >= 0 && ty < _maskTexture.height)
                {
                    _maskTexture.SetPixel(tx, ty, Color.black);
                }
            }

            _maskTexture.Apply();
        }
    }
}