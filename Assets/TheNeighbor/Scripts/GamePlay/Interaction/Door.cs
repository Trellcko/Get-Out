using System;
using DG.Tweening;
using Trellcko.Gameplay.Player;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Interactable
{
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _goCollider;
        [SerializeField] private AudioSource _interactAudio;
        [SerializeField] private AudioSource _knockingSound;
        public event Action InteractionFinished;
        [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
        public bool IsInteractable { get; private set; } = true;

        public event Action InteractionEnabled;
        public event Action InteractionStarted;

        private Vector3 _defaultAngel;

        private const float OpenTime = 3f;
        

        private void Awake()
        {
            _defaultAngel = transform.localEulerAngles;
        }

        public void PlayKnockingSound(bool loop)
        {
            _knockingSound.loop = loop;
            _knockingSound.Play();
        }

        public void StopKnockingSound()
        {
            _knockingSound.Stop();
        }
        
        public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
        {
            getItem = neededItem;
            if (IsInteractable)
            {
                InteractionStarted?.Invoke();
                InteractableOutline.Disable();
                IsInteractable = false;
                Vector3 targetAngel = _defaultAngel;
                targetAngel.y = -115f;
                _interactAudio.Play();
                InteractionStarted?.Invoke();
                Open(targetAngel);
                InteractionFinished?.Invoke();
                return true;
            }

            return false;
        }

        private  void Open(Vector3 targetAngel)
        {
            transform.DOLocalRotate(targetAngel, OpenTime, RotateMode.FastBeyond360)
                .OnUpdate(() =>
                {
                    _rigidbody.MoveRotation(transform.rotation);
                })
                .OnComplete(() =>
                {
                    _goCollider.enabled = false;
                });
        }

        public void ReturnToInitImmediately()
        {
            transform.localRotation = Quaternion.Euler(_defaultAngel);
            IsInteractable = true;
            _goCollider.enabled = true;
            InteractionEnabled?.Invoke();
        }

        public void ReturnToInit()
        {
            if (!IsInteractable && _goCollider.enabled)
                return;

            _interactAudio.Play();
            _goCollider.enabled = true;
            transform.DOLocalRotate(_defaultAngel, OpenTime).OnComplete(() =>
            {
                IsInteractable = true;
                InteractionEnabled?.Invoke();
            });
        }
    }
}
