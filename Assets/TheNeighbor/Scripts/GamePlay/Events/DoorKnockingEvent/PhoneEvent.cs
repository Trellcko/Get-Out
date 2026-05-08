using Trellcko.Dialog;
using Trellcko.Gameplay.Events;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Interactable
{
    public class PhoneEvent : BaseEvent
    {
        [SerializeField] private Phone _phone;
        [SerializeField] private DialogData _dialogData;
        
        protected override void OnBeforeNotifierInvoked()
        {
            _phone.PlayDialog(_dialogData);
        }

        protected override void OnMakeVisible()
        {
            _phone.PlayRing();
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}