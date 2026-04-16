using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace TheNeighbor.Scripts.GamePlay.Triggers
{
    public class WhenDoorInteractedNotifier : Notifier
    {
        [SerializeField] private Door _door;

        public override void StartWatching()
        {
            
            Debug.Log("Subscribed");
            _door.InteractionStarted += OnInteracted;
        }

        private void OnInteracted()
        {
            Debug.Log("Interacted");
            InvokeNotified();
            _door.InteractionStarted -= OnInteracted;
        }
    }
}