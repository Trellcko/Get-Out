using System;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay
{
    public class TVController : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }

        [SerializeField] private QuestInteractable _questInteractable;
        [SerializeField] private Light _light;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSystem _redParticle;
        [SerializeField] private AudioSource _audio;

        private bool _isWorked;
        public event Action InteractionEnabled;
        public event Action InteractionStarted;
        public event Action InteractionFinished;

        public bool IsInteractable { get; private set; } = true;

        private void OnEnable()
        {
            if(!_questInteractable) return;
            _questInteractable.InteractionEnabled += OnInteractionEnabled;
            _questInteractable.InteractionFinished += OnInteractionFinished;
        }

        private void OnDisable()
        {
            if(!_questInteractable) return;
            _questInteractable.InteractionEnabled -= OnInteractionEnabled;
            _questInteractable.InteractionFinished -= OnInteractionFinished;
        }

        private void OnInteractionFinished()
        {
            IsInteractable = true;
        }

        private void OnInteractionEnabled()
        {
            TurnOff();
            IsInteractable = false;
        }

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = neededItem;
            if (!IsInteractable)
                return false;
            InteractionStarted?.Invoke();
            if (_isWorked)
            {
                TurnOff();
            }
            else
            {
                TurnOn();
            }
            InteractionFinished?.Invoke();
            return true;
        }

        public void TurnOn()
        {
            _isWorked = true;
            _light.color = Color.white;
            _light.enabled = true;
            _particle.Play();
            _audio.Play();
        }

        public void TurnOnRedMode()
        {
            _isWorked = true;
            _light.color = Color.red;
            _light.enabled = true;
            _redParticle.Play();
            _audio.Play();
        }

        public void TurnOff()
        {
            _isWorked = false;
            _light.enabled = false;
            _redParticle.Stop();
            _particle.Stop();
            _audio.Stop();
        }
    }
}