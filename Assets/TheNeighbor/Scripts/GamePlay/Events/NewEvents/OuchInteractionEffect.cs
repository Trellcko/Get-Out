using System;
using Trellcko.Gameplay.MiniGame;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Trellcko.Gameplay.Events
{
    public class OuchInteractionEffect : MonoBehaviour
    {
        [SerializeField] private QuestInteractable _questInteractable;
        [SerializeField] private Volume _volume;
        
        private BadEffectPlayer _effectPlayer;

        private void OnEnable()
        {
            _questInteractable.InteractionFinished += OnInteractionFinished;
        }

        private void OnDisable()
        {
            _questInteractable.InteractionFinished -= OnInteractionFinished;
        }

        private void OnInteractionFinished()
        {
            _effectPlayer.PlayOuchEffect(_volume);
        }

        [Inject]
        private void Construct(BadEffectPlayer effectPlayer)
        {
            _effectPlayer = effectPlayer;
        }
    }
}