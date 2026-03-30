using System;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay
{
    public class TVController : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        
        [SerializeField] private Light _light;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private ParticleSystem _redParticle;
        [SerializeField] private AudioSource _audio;

        public bool IsWorked { get; private set; } = false;

        public event Action InteractionStarted;
        public event Action InteractionFinished;

        public bool IsInteractable => true;


        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = neededItem;
            if (!IsInteractable)
                return false;
            InteractionStarted?.Invoke();
            if (IsWorked)
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
            IsWorked = true;
            _light.color = Color.white;
            _light.enabled = true;
            _particle.Play();
            _audio.Play();
        }

        public void TurnOnRedMode()
        {
            IsWorked = true;
            _light.color = Color.red;
            _light.enabled = true;
            _redParticle.Play();
            _audio.Play();
        }

        public void TurnOff()
        {
            IsWorked = false;
            _light.enabled = false;
            _redParticle.Stop();
            _particle.Stop();
            _audio.Stop();
        }
    }
}