using System;
using DG.Tweening;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;
using Random = UnityEngine.Random;

namespace Trellcko.Gameplay.MiniGame
{
    public class ClothesInteractable : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }

        [SerializeField] private ClothesDraggable _clothesPrefab;
        [SerializeField] private ClothesSpriteData _clothesSpriteData;

        [SerializeField] private Volume volume;
        public bool IsInteractable { get; private set; }

        private int _currentCorpses;
        private int _currentClothes;
        
        private MiniGameBadEffect _miniGameBadEffect;
        
        private DiContainer _container;
        private ISoundController _soundController;

        public event Action<bool> Generated;
        public event Action<bool> Putted;
        public event Action Reseted;
        public event Action RunOut;
        public event Action InteractionStarted;
        public event Action InteractionFinished;

        [Inject]
        private void Construct(DiContainer container, MiniGameBadEffect miniGameBadEffect, ISoundController soundController)
        {
            _soundController = soundController;
            _miniGameBadEffect = miniGameBadEffect;
            _container = container;
        }

        public void SetMiniGameData(ClosetMiniGameData closetMiniGameData)
        {
            _currentClothes = closetMiniGameData.ClothesCount;
            _currentCorpses = closetMiniGameData.CorpseCount;
            IsInteractable = true;
            Reseted?.Invoke();
        }

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = neededItem;
            if (!IsInteractable) return false;
            InteractionStarted?.Invoke();
            ClothesDraggable clothesInstance = _container.InstantiatePrefab(_clothesPrefab).GetComponent<ClothesDraggable>();
            clothesInstance.transform.position = transform.position;
            clothesInstance.Putted += OnPutted;
            clothesInstance.UpdateSprite(TakeRandomSprite(out bool isCorpse), isCorpse);
            if (isCorpse)
            {
                _miniGameBadEffect.PlayCorpseEffect(volume);
            }
            else
            {
                _soundController.PlayOtherSound(OtherSound.Pick);
            }
            Generated?.Invoke(isCorpse);
            IsInteractable = false;
            InteractionFinished?.Invoke();
            return true;
        }

        private void OnPutted(ClothesDraggable obj)
        {
            obj.Putted -= OnPutted;
            if (_currentClothes <= 0 && _currentCorpses <= 0)
            {
                RunOut?.Invoke();
                return;
            }
            Putted?.Invoke(obj.IsCorpse);
            Reseted?.Invoke();
            IsInteractable = true;

        }

        private Sprite TakeRandomSprite(out bool isCorpse)
        {
            bool takeFirst;
            isCorpse = false;

            if (_currentClothes <= 0)
                takeFirst = false;
          
            else if (_currentCorpses <= 0)
                takeFirst = true;
            else
                takeFirst = Random.Range(0, 1f) > 0.5f;
            
            if (takeFirst)
            {
                _currentClothes--;
                return _clothesSpriteData.ClothesSprites[Random.Range(0, _clothesSpriteData.ClothesSprites.Count)];
            }

            isCorpse = true;
            _currentCorpses--;
            return _clothesSpriteData.CorpsesSprites[Random.Range(0, _clothesSpriteData.CorpsesSprites.Count)];
        }
    }
}