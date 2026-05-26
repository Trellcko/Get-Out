using System;
using System.Collections;
using CastleWarriors.GameLogic.Utils;
using UnityEngine;

namespace Trellcko.Gameplay.House
{
    public class WallEyeController : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private WallEyeballController _wallEyeballController;
        
        [SerializeField] private Texture _open;
        [SerializeField] private Texture _close;
        [SerializeField] private Vector2 _timeBetweenBlinks;
        [SerializeField] private float _blinkTime = 0.2f;
        private BetterTimer _timer;

        private void Awake()
        {
            _timer = new(_timeBetweenBlinks, true, true);
            _timer.Completed += Blink;
            _timer.Reset();
        }
        
        private void OnDestroy()
        {
            _timer.Completed -= Blink;
        }

        private void Update()
        {
            _timer.Tick();
        }

        private void Blink()
        {
            StartCoroutine(BlinkCorun());
        }

        private IEnumerator BlinkCorun()
        {
            _renderer.material.mainTexture = _close;
            _wallEyeballController.gameObject.SetActive(false);
            yield return new WaitForSeconds(_blinkTime);
            _renderer.material.mainTexture = _open;
            _wallEyeballController.gameObject.SetActive(true);
        }
        
    }
}