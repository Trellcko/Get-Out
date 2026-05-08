using System;
using System.Collections;
using System.Collections.Generic;
using Trellcko.Dialog;
using Trellcko.Gameplay.Player;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Interactable
{
    public class Phone : MonoBehaviour    {
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _ringClip;
        [SerializeField] private AudioClip _hangUpPhoneClip;
        [SerializeField] private AudioClip _hangDownPhoneClip;

        private PlayerFacade _playerFacade;
        
        private IDialogSystem _dialogSystem;
        private DialogData _dialogData;

        [Inject]
        private void Construct(IDialogSystem dialogSystem, PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _dialogSystem = dialogSystem;
        }
        
        
        public void PlayRing()
        {
            _audioSource.clip = _ringClip;
            _audioSource.Play();
            _audioSource.loop = true;
        }

        public void PlayDialog(DialogData dialogData)
        {
            _dialogData = dialogData;
            _audioSource.clip = _hangUpPhoneClip;
            _audioSource.Play();
            _audioSource.loop = false;
            _dialogData.OnHided += PlayHangDownSound;
            _dialogData.OnHided += UnSubscribeFromData;
            _dialogData.OnHided += UnBlockPlayer;
            _dialogData.OnShowed += BlockPlayer;
            _dialogSystem.ShowDialog(_dialogData);
        }

        private void UnSubscribeFromData()
        {
            _dialogData.OnHided -= UnSubscribeFromData;
            _dialogData.OnHided -= PlayHangDownSound;
            _dialogData.OnHided -= UnBlockPlayer;
            _dialogData.OnShowed -= BlockPlayer;
        }
        
        private void BlockPlayer()
        {
            _playerFacade.Interactable.IsEnabled = false;
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
        }

        private void UnBlockPlayer()
        {
            _playerFacade.Interactable.IsEnabled = true;
            _playerFacade.PlayerMovement.IsEnabled = true;
            _playerFacade.PlayerRotation.IsEnabled = true;
        }
        
        private void PlayHangDownSound()
        {
            _audioSource.clip = _hangDownPhoneClip;
            _audioSource.Play();
        }
    }
}