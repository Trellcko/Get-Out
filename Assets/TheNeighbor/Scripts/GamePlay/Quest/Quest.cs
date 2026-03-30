using System;
using UnityEngine;

namespace Trellcko.Gameplay.QuestLogic
{
    [Serializable]
    public class Quest
    {
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public QuestInteractable QuestInteractable { get; private set; }
        public event Action Completed;
        public void Activate()
        {
            QuestInteractable.EnableInteraction();
            QuestInteractable.InteractionStarted += OnInteractionStarted;
        }

        public void ForceCompleteQuest()
        {
            OnInteractionStarted();
        }
        
        private void OnInteractionStarted()
        {
            QuestInteractable.InteractionStarted -= OnInteractionStarted;
            Completed?.Invoke();
        }
    }
}