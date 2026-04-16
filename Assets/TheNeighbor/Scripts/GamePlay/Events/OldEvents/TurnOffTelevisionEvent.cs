using UnityEngine;

namespace Trellcko.Gameplay.Events
{
    public class TurnOffTelevisionEvent : BaseEvent
    {
        [SerializeField] private TVController _TVController;
        protected override void OnBeforeNotifierInvoked()
        {
            
        }

        protected override void OnMakeVisible()
        {
            _TVController.TurnOff();
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}