using System;
using DG.Tweening;
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

        public event Action<bool> ClothesGenerated;
        public event Action Reseted;
        public event Action ClothesRunOut;
        public event Action Interacted;

        [Inject]
        private void Construct(DiContainer container, MiniGameBadEffect miniGameBadEffect)
        {
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
            ClothesDraggable clothesInstance = _container.InstantiatePrefab(_clothesPrefab).GetComponent<ClothesDraggable>();
            clothesInstance.transform.position = transform.position;
            clothesInstance.Putted += OnPutted;
            clothesInstance.UpdateSprite(TakeRandomSprite(out bool isCorpse));
            if (isCorpse)
            {
                _miniGameBadEffect.PlayCorpseEffect(volume);
            }
            ClothesGenerated?.Invoke(isCorpse);
            IsInteractable = false;
            return true;
        }

        private void OnPutted(ClothesDraggable obj)
        {
            obj.Putted -= OnPutted;
            if (_currentClothes <= 0 && _currentCorpses <= 0)
            {
                ClothesRunOut?.Invoke();
                return;
            }
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