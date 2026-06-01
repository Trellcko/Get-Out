using Trellcko.Gameplay.MiniGame;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Trellcko.Gameplay.Events
{
    public class OuchEvent : BaseEvent
    {
        [SerializeField] private Volume _volume;
        
        private BadEffectPlayer _effectPlayer;

        [Inject]
        private void Construct(BadEffectPlayer effectPlayer)
        {
            _effectPlayer = effectPlayer;
        }
        
        protected override void OnBeforeNotifierInvoked()
        {
            _effectPlayer.PlayOuchEffect(_volume);
        }

        protected override void OnMakeVisible()
        {
        }

        protected override void OnNotifierInvokeHandle()
        {
        }
    }
}