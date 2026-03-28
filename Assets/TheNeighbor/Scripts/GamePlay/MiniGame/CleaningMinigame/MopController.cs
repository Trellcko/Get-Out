using System;
using Trellcko.Core.Input;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class MopController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _target;
        [SerializeField] private Transform _mopPoint;
        [SerializeField] private Texture2D _maskTexture;
        [SerializeField] private float _sensitivity;
        
        private IInputHandler _inputHandler;

        private Camera _camera;
        private Vector3 _startPosition;
        [SerializeField] private int _brushSize = 100;

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
        }

        private void Update()
        {
            MoveMop();
            PaintMaskAt(_mopPoint.position);
        }

        private void MoveMop()
        {
            Vector3 mouseDelta = _inputHandler.GetMouseDelta();
            mouseDelta *= _sensitivity * Time.deltaTime;

            transform.position += mouseDelta;
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
                int tx = px + x;
                int ty = py + y;
                if (tx >= 0 && tx < _maskTexture.width && ty >= 0 && ty < _maskTexture.height)
                    _maskTexture.SetPixel(tx, ty, Color.black);
            }

            _maskTexture.Apply();
        }
    }
}