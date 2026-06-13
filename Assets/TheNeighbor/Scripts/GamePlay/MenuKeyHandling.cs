using System;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Trellcko.Gameplay
{
    public class MenuKeyHandling : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private Button _resumeButton;
        
        private bool IsMainMenuActive => _mainMenu.activeSelf;

        private bool _wasPlayerActive;
        private bool _wasCursorLocked;

        private IInputHandler _inputHandler;
        private PlayerFacade _playerFacade;
        private ICursorController _cursorController;


        [Inject]
        private void Construct(IInputHandler inputHandler, PlayerFacade playerFacade, ICursorController cursorController)
        {
            _cursorController = cursorController;
            _inputHandler = inputHandler;
            _playerFacade = playerFacade;
        }

        private void OnEnable()
        {
            _inputHandler.EscapePressed += OnEscapePressed;
            _resumeButton.onClick.AddListener(OnResumePressed);
        }

        private void OnDisable()
        {
            _inputHandler.EscapePressed -= OnEscapePressed;
            _resumeButton.onClick.RemoveListener(OnResumePressed);
        }

        private void OnResumePressed()
        {
            OnEscapePressed();
        }

        private void OnEscapePressed()
        {
            if (!IsMainMenuActive)
            {
                _wasPlayerActive = _playerFacade.PlayerMovement.IsEnabled;
                
                _playerFacade.Interactable.IsEnabled = IsMainMenuActive;
                _playerFacade.PlayerMovement.IsEnabled = IsMainMenuActive;
                _playerFacade.PlayerRotation.IsEnabled = IsMainMenuActive;
                
                _wasCursorLocked = _cursorController.CursorLocked;
                
                _cursorController.UnlockCursor();
                _cursorController.ShowCursor();
            }
            else
            {
                if(_wasCursorLocked)
                    _cursorController.LockCursor();
                else
                    _cursorController.HideCursor();
                
                _playerFacade.Interactable.IsEnabled = _wasPlayerActive;
                _playerFacade.PlayerMovement.IsEnabled = _wasPlayerActive;
                _playerFacade.PlayerRotation.IsEnabled = _wasPlayerActive;
            }

            _mainMenu.SetActive(!IsMainMenuActive);
        }
    }
}