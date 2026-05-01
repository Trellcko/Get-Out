using Trellcko.Gameplay.Events;
using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class PhoneEvent : BaseEvent
    {
        [SerializeField] private Phone _phone;
        [SerializeField] private AudioClip _clip;
        
        protected override void OnBeforeNotifierInvoked()
        {
            _phone.PlayVoice(_clip);
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