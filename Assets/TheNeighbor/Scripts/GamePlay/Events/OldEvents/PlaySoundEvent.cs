using Trellcko.Gameplay.Events;
using UnityEngine;

namespace Trellcko.Gameplay.Events
{
    public class PlaySoundEvent : BaseEvent
    {
        [SerializeField] private AudioSource _audioSource;
        
        protected override void OnBeforeNotifierInvoked()
        {

        }

        protected override void OnMakeVisible()
        {
            _audioSource.Play();
        }

        protected override void OnNotifierInvokeHandle()
        {
            Debug.Log("Stoped absouluttely");
            _audioSource.Stop();
        }
    }
}