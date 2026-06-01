using System;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Effects
{
    public class BloodDrop : MonoBehaviour
    {

        [SerializeField] private int _showDay = 6;
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private ParticleSystem _drop;

        private IQuestSystem _questSystem;

        [Inject]
        private void Construct(IQuestSystem questSystem)
        {
            _questSystem = questSystem;
        }

        private void OnEnable()
        {
            _questSystem.DayStarted += OnDayStarted;
        }

        private void OnDisable()
        {
            _questSystem.DayStarted -= OnDayStarted;
        }

        private void OnDayStarted()
        {
            if (_questSystem.Day >= _showDay)
            {
                _drop.Play();
            }
        }

        private void OnParticleCollision(GameObject other)
        {
            _audioSource.Play();
        }
        
    }
}