using System;
using System.Collections;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class Phone : MonoBehaviour, IInteractable
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _ringClip;
        [SerializeField] private AudioClip _hangUpPhoneClip;
        [SerializeField] private AudioClip _hangDownPhoneClip;
        [SerializeField] private AudioClip _sound;
        [SerializeField] private float _speekingTime;
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        public bool IsInteractable { get; private set; }
        public event Action InteractionStarted;
        public event Action InteractionFinished;


        public void Activate()
        {
            IsInteractable = true;
            _audioSource.clip = _ringClip;
            _audioSource.Play();
            _audioSource.loop = true;
        }

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = neededItem;
            if (!IsInteractable) return false;
            InteractionStarted?.Invoke();
            IsInteractable = false;
            _audioSource.clip = _hangUpPhoneClip;
            _audioSource.Play();
            _audioSource.loop = false;
            StartCoroutine(PlaySound());
            InteractableOutline.Disable();
            InteractionFinished?.Invoke();
            return true;
        }

        private IEnumerator PlaySound()
        {
            yield return new WaitForSeconds(1f);
            _audioSource.clip = _sound;
            _audioSource.Play();
            yield return new WaitForSeconds(_speekingTime);
            _audioSource.clip = _hangDownPhoneClip;
            _audioSource.Play();
        }
    }
}