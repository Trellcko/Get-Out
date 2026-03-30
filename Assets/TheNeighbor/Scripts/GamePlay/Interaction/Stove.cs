using System;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class Stove : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        [SerializeField] private AudioSource _interactAudio;
        [SerializeField] private GameObject _fire;
        public bool IsInteractable => true;
        
        public event Action InteractionStarted;
        public event Action InteractionFinished;

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            InteractionStarted?.Invoke();
            getItem = neededItem;
            _interactAudio.Play();
            _fire.gameObject.SetActive(!_fire.activeSelf);
            InteractionFinished?.Invoke();
            return true;
        }
    }
}