using Trellcko.Dialog;
using Trellcko.Gameplay.Events;
using Trellcko.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.QuestLogic
{
    public class DoorKnockingEvent : BaseEvent
    {
        [SerializeField] private AudioSource _doorAudioSource;
        [SerializeField] private DialogData _dialogData;
        
        private IDialogSystem _dialogSystem;
        private PlayerFacade _playerFacade;

        private void Awake()
        {
            _dialogData.OnHided += UnBlock;
        }

        private void UnBlock()
        {
            _playerFacade.PlayerMovement.IsEnabled = true;
            _playerFacade.PlayerRotation.IsEnabled = true;
            _playerFacade.Interactable.IsEnabled = true;
        }

        [Inject]
        private void Construct(IDialogSystem dialogSystem, PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _dialogSystem = dialogSystem;
        }

        protected override void OnBeforeNotifierInvoked()
        {
            _dialogData.OnHided += _doorAudioSource.Stop;
            _dialogSystem.ShowDialog(_dialogData);
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
        }

        protected override void OnMakeVisible()
        {
            _doorAudioSource.Play();
        }

        protected override void OnNotifierInvokeHandle()
        {
            
        }
    }
}