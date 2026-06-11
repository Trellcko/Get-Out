using System;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Interactable
{
    public class TapeRecorder : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _putClip;
        [SerializeField] private AudioClip _getClip;
        public bool IsInteractable => true;
        public event Action InteractionEnabled;
        public event Action InteractionStarted;

        private ISoundController _soundController;
        private Ambience _lastAmbience;

        public event Action InteractionFinished;

        [Inject]
        private void Construct(ISoundController soundController)
        {
            _soundController = soundController;
        }

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = neededItem;
            InteractionStarted?.Invoke();
            if (_soundController.IsAmbiencPlaying)
            {
                _audioSource.clip = _getClip;
                _lastAmbience = _soundController.CurrentAmbience;
                _soundController.StopPlayingAmbience();
            }
            else
            {
                _audioSource.clip = _putClip;
                _soundController.PlayAmbience(_lastAmbience);
            }

            _audioSource.Play();
            InteractionFinished?.Invoke();
            return true;
        }
    }
}