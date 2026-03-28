using System;
using NaughtyAttributes;
using Trellcko.Gameplay.Interactable;
using UnityEngine;

namespace Trellcko.Gameplay.QuestLogic
{
    public class QuestInteractable : MonoBehaviour, IInteractable
    {
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        [field: SerializeField] public QuestItem NeededItem { get; private set; } = QuestItem.None;
        [field: SerializeField] public QuestItem ReceiveItem { get; private set; } = QuestItem.None;
        
        [SerializeField] private AudioSource _audioSource;

        [SerializeField] private AfterInteractionAction _afterInteractionAction;

        [ShowIf("_showAnimationNameProperty")]
        [SerializeField] private string _animationName;
        
        private bool _showAnimationNameProperty => _afterInteractionAction == AfterInteractionAction.PlayAnimation;
        
        public event Action Interacted;
        
        public bool IsInteractable { get; private set; }
        
        private MeshRenderer _meshRenderer;
        private Collider _collider;
        private Animator _animator;

        private void Awake()
        {
            if (_afterInteractionAction == AfterInteractionAction.PlayAnimation)
            {
                _animator = GetComponent<Animator>();
            }
            _collider = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        [Button]
        public void EnableInteraction()
        {
            IsInteractable = true;
            InteractableOutline.EnableInteractOutline();
        }

        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = ReceiveItem;
            if (!IsInteractable || NeededItem != neededItem)
                return false;

            Interacted?.Invoke();
            if (_audioSource)
                _audioSource?.Play();
            IsInteractable = false;
            InteractableOutline.Disable();

            DoAfterInteractionAction();

            return true;
        }

        private void DoAfterInteractionAction()
        {
            switch (_afterInteractionAction)
            {
                case AfterInteractionAction.None:
                    break;
                case AfterInteractionAction.DisableVisual:
                    _meshRenderer.enabled = false;
                    break;
                case AfterInteractionAction.DisableCollider:
                    _collider.enabled = false;
                    break;
                case AfterInteractionAction.DisableVisualAndCollider:
                {
                    _meshRenderer.enabled = false;
                    _collider.enabled = false;
                }
                    break;
                case AfterInteractionAction.PlayAnimation:
                {
                    _animator.Play(_animationName);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}