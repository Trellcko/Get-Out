using System;
using Trellcko.Gameplay.Interactable;

namespace Trellcko.Gameplay.QuestLogic
{
    public interface IInteractable
    {
        event Action InteractionStarted;
        event Action InteractionFinished;
        public InteractableOutline InteractableOutline { get; }
        bool IsInteractable { get; }
        bool TryInteract(out QuestItem getItem, QuestItem neededItem);
    }
}