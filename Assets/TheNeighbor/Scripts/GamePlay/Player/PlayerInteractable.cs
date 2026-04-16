using System;
using System.Linq;
using TheNeighbor.Scripts.Constants;
using Trellcko.Core.Input;
using Trellcko.Core.Physics;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Player
{
    public class PlayerInteractable : MonoBehaviour
    {
        [SerializeField] private float _rayLength;
        [SerializeField] private Bringing _bringing;

        public bool IsEnabled { get; set; } = true;
        
        private QuestItem _item;
        
        private IInputHandler _inputHandler;
        private IInteractable _lastInteractable;
        private IRayGetter _rayGetter;

        private Camera _camera;
        private readonly RaycastHit[] _hits = new RaycastHit[5];

        [Inject]
        private void Inject(IInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void OnEnable()
        {
            _inputHandler.MouseMoved += OnMoveInputInvoked;
            _inputHandler.Moved += OnMoveInputInvoked;
            _inputHandler.Interacted += OnInteractButtonClicked;
        }

        private void OnDisable()
        {
            _inputHandler.MouseMoved -= OnMoveInputInvoked;
            _inputHandler.Moved -= OnMoveInputInvoked;
            _inputHandler.Interacted -= OnInteractButtonClicked;
        }

        private void Awake()
        {
            _camera = Camera.main;
            ResetRayCameraGetter();
        }

        private void ResetRayCameraGetter()
        {
            _rayGetter = new RayCameraGetter(_camera);
        }

        public void SetItem(QuestItem item)
        {
            _item = item;
            _bringing.SetItem(item);
        }

        private void OnMoveInputInvoked(Vector2 obj)
        {
            CheckForSelectables();
        }

        private void CheckForSelectables()
        {
            if(!IsEnabled) return;
            
            _lastInteractable?.InteractableOutline?.EnableInteractOutline(false);
            if (TryGetInteractable(out IInteractable questInteractable))
            {
                if (questInteractable.IsInteractable)
                {
                    questInteractable.InteractableOutline?.EnableSelectOutline();
                    _lastInteractable = questInteractable;
                }
            }
            else
            {
                if (_lastInteractable is not QuestInteractable)
                {
                    _lastInteractable?.InteractableOutline?.Disable();
                }
            }
        }

        private bool TryGetInteractable(out IInteractable questInteractable)
        {
            questInteractable = null;
            Ray ray = _rayGetter.GetRay();
            Debug.DrawRay(ray.origin, ray.direction * _rayLength, Color.red);
            int count = Physics.RaycastNonAlloc(ray, _hits, _rayLength, Layers.Interactable);
            float minDistance = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                if (_hits[i].distance < minDistance)
                {
                    questInteractable = _hits[i].transform.GetComponentsInChildren<IInteractable>().FirstOrDefault(x => x.IsInteractable);
                    if (questInteractable != null)
                    {
                        minDistance = _hits[i].distance;
                    }
                }
            }
        
            return questInteractable != null;
        }

        private void OnInteractButtonClicked()
        {
            if (TryGetInteractable(out IInteractable questInteractable))
            {
                if(_lastInteractable == questInteractable && _lastInteractable.IsInteractable)
                    _lastInteractable?.InteractableOutline?.EnableSelectOutline();
                
                if(questInteractable.TryInteract(out QuestItem tempItem, _item))
                {
                    _bringing.SetItem(QuestItem.None);
                    _item = tempItem;
                    _bringing.SetItem(_item);
                    _lastInteractable = questInteractable;
                }
            }
        }
    }
}