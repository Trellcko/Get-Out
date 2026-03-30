using System;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class InteractableTemple : MonoBehaviour, IInteractable
    {
        public event Action InteractionStarted;
        public event Action InteractionFinished;
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        public bool IsInteractable => true;

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            InteractionStarted?.Invoke();
            getItem = neededItem;
            InteractionFinished?.Invoke();
            return true;
        }
    }
}