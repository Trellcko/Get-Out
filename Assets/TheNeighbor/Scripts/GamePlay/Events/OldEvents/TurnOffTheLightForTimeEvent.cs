using System;
using System.Collections;
using UnityEngine;

namespace Trellcko.Gameplay.Events
{
    public class TurnOffTheLightForTimeEvent : BaseEvent
    {
        [SerializeField] private Light[] _lights;
        [SerializeField] private float _withoutLightTime = 10f;
        [SerializeField] private float _winkingTime = 0.2f;
        [SerializeField] private float _winkingCounts = 3;

        private int _currentCount;
        private float _currentTime;

        private bool _isWinking;

        private IEnumerator StartCorun()
        {
            _currentTime = 0f;
            while (_currentCount < _winkingCounts)
            {
                if (_currentTime > _winkingTime)
                {
                    _currentTime = 0f;
                    _currentCount++;
                    SwitchLightsState();
                }

                _currentTime += Time.deltaTime;
                yield return null;
            }
            DisableLights();
            yield return new WaitForSeconds(_withoutLightTime);

            EnableLights();
        }

        private void SwitchLightsState()
        {
            foreach (Light light1 in _lights)
            {
                light1.enabled = !light1.enabled;
            }
        }
        
        private void DisableLights()
        {
            foreach (Light light1 in _lights)
            {
                light1.enabled = false;
            }
        }
        private void EnableLights()
        {
            foreach (Light light1 in _lights)
            {
                light1.enabled = true;
            }
        }

        protected override void OnBeforeNotifierInvoked()
        {
            
        }

        protected override void OnMakeVisible()
        {
            _currentCount = 0;
            _currentTime = 0;
            StartCoroutine(StartCorun());
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}