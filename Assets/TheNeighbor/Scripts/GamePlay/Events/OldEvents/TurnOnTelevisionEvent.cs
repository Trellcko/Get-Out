using UnityEngine;

namespace Trellcko.Gameplay.Events
{
    public class TurnOnTelevisionEvent : BaseEvent
    {
        [SerializeField] private TVController _TVController;
        [SerializeField] private bool _normalMode = true;
        
        protected override void OnBeforeNotifierInvoked()
        {
            
        }

        protected override void OnMakeVisible()
        {
            if(_normalMode)
                _TVController.TurnOn();
            else
                _TVController.TurnOnRedMode();
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}