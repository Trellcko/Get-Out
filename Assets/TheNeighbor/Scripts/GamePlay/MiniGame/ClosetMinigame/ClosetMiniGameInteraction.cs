using TheNeighbor.Scripts.Constants;
using Trellcko.Core.Input;
using Trellcko.Core.Physics;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class ClosetMiniGameInteraction : MonoBehaviour
    {
        [SerializeField] private ClothesInteractable _clothesInteractable;
        [SerializeField] private InteractableOutline _closetInteractableOutline;
        [SerializeField] private float _rayLength;
        
        private IInputHandler _inputHandler;
        private IRayGetter _rayGetter;

        private Camera _camera;
        private readonly RaycastHit[] _hits = new RaycastHit[5];

        private InteractionState _interactionState = InteractionState.FinishedInteraction;

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
            _clothesInteractable.Reseted += OnReseted;
            _clothesInteractable.RunOut += OnClothesRunOut;
            _clothesInteractable.Generated += OnGenerated;
        }


        private void OnGenerated(bool isCorpse)
        {
            _clothesInteractable.InteractableOutline.Disable();
            _closetInteractableOutline.EnableInteractOutline();
            _interactionState = InteractionState.Dragging;
        }

        private void OnClothesRunOut()
        {
            _clothesInteractable.InteractableOutline.Disable();
            _closetInteractableOutline.Disable();
            _interactionState = InteractionState.FinishedInteraction;
        }

        private void OnReseted()
        {
            _closetInteractableOutline.Disable();
            _clothesInteractable.InteractableOutline.EnableInteractOutline();
            _interactionState = InteractionState.NeedToDrag;
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
            _rayGetter = new RayMouseGetter(_inputHandler ,_camera);
        }
        
        private void OnMoveInputInvoked(Vector2 obj)
        {
            CheckForSelectables();
        }

        private void CheckForSelectables()
        {
            if(_interactionState != InteractionState.NeedToDrag) return;
            if (TryGetInteractable(out ClothesInteractable questInteractable))
            {
                if (questInteractable.IsInteractable)
                {
                    questInteractable.InteractableOutline?.EnableSelectOutline();
                }
            }
            else
            {
                _clothesInteractable.InteractableOutline?.EnableInteractOutline();
            }
        }

        private bool TryGetInteractable(out ClothesInteractable interactable)
        {
            interactable = null;
            Ray ray = _rayGetter.GetRay();
            Debug.DrawRay(ray.origin, ray.direction * _rayLength, Color.red);
            int count = Physics.RaycastNonAlloc(ray, _hits, _rayLength, Layers.Interactable);
            float maxDistance = float.MaxValue;
            for (int i = 0; i < count; i++)
            {
                if (_hits[i].distance < maxDistance)
                {
                    if (_hits[i].transform.TryGetComponent(out interactable))
                    {
                        maxDistance = _hits[i].distance;
                    }
                }
            }
        
            return interactable;
        }

        private void OnInteractButtonClicked()
        {
            if(_interactionState != InteractionState.NeedToDrag) return;
            if (TryGetInteractable(out ClothesInteractable questInteractable))
            {
                questInteractable.TryInteract(out QuestItem tempItem, QuestItem.None);
            }
        }
    }

    public enum InteractionState
    {
        NeedToDrag,
        Dragging,
        FinishedInteraction,
    }
}