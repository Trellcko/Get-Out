using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.Events;
using UnityEngine;

namespace Trellcko.Gameplay.Events
{
    public class ShutTheDoorEvent : BaseEvent
    {
        [SerializeField] private Door _door;


        protected override void OnBeforeNotifierInvoked()
        {
            
        }

        protected override void OnMakeVisible()
        {
            _door.ReturnToInit();
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}