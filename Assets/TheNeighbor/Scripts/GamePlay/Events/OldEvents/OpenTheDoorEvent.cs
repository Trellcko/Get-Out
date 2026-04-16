using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.Events
{
    public class OpenTheDoorEvent : BaseEvent
    {
        [SerializeField] private Door _door;


        protected override void OnBeforeNotifierInvoked()
        {

        }

        protected override void OnMakeVisible()
        {
            _door.TryInteract(out _, QuestItem.None);
        }

        protected override void OnNotifierInvokeHandle()
        {

        }
    }
}