using Trellcko.Gameplay.Events;
using UnityEngine;

namespace Trellcko.Gameplay.QuestLogic
{
    public class DoorKnockingEvent : BaseEvent
    {
        [SerializeField] private AudioSource _doorAudioSource;
        protected override void OnBeforeNotifierInvoked()
        {
            _doorAudioSource.Stop();
        }

        protected override void OnMakeVisible()
        {
            _doorAudioSource.Play();
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}