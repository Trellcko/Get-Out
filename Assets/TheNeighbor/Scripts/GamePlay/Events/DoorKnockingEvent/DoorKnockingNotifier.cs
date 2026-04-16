using UnityEngine;

namespace Trellcko.Gameplay.QuestLogic
{
    public class DoorKnockingNotifier : Notifier
    {
        [SerializeField] private QuestInteractable _door;
        
        public override void StartWatching()
        {
            _door.InteractionFinished += OnInteractionFinished;
        }

        private void OnInteractionFinished()
        {
            _door.InteractionFinished -= OnInteractionFinished;
            InvokeNotified();
        }
    }
}