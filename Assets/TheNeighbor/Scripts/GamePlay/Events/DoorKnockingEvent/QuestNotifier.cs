using UnityEngine;

namespace Trellcko.Gameplay.QuestLogic
{
    public class QuestNotifier : Notifier
    {
        [SerializeField] private QuestInteractable _questInteractable;
        
        public override void StartWatching()
        {
            _questInteractable.InteractionFinished += OnInteractionFinished;
        }

        private void OnInteractionFinished()
        {
            _questInteractable.InteractionFinished -= OnInteractionFinished;
            InvokeNotified();
        }
    }
}