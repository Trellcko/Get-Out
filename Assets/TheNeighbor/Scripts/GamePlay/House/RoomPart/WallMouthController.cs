using System;
using CastleWarriors.GameLogic.Utils;
using UnityEngine;

namespace Trellcko.Gameplay.House
{
    public class WallMouthController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;

        [SerializeField] private Texture _openMouth;
        [SerializeField] private Texture _closeMouth;
        
        [SerializeField] private Vector2 _timeToChangeMouthStateMinMax;

        private BetterTimer _timer;

        private bool _isOpened;

        private void Awake()
        {
            _timer = new(_timeToChangeMouthStateMinMax, true, true);
            _timer.Completed += OnCompleted;
            _timer.Reset();
        }

        private void Update()
        {
            _timer.Tick();
        }

        private void OnCompleted()
        {
            Texture texture = _isOpened ? _closeMouth : _openMouth;
            _renderer.material.mainTexture = texture;
            _isOpened = !_isOpened;
        }
    }
}