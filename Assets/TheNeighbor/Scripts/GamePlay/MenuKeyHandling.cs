using System;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay
{
    public class MenuKeyHandling : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;

        private bool IsMainMenuActive => _mainMenu.activeSelf;
        
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
        }

        private void OnDisable()
        {
            _inputHandler.EscapePressed -= OnEscapePressed;
        }

        private void OnEscapePressed()
        {
            _playerFacade.Interactable.IsEnabled = IsMainMenuActive;
            _playerFacade.PlayerMovement.IsEnabled = IsMainMenuActive;
            _playerFacade.PlayerRotation.IsEnabled = IsMainMenuActive;
            if (!IsMainMenuActive)
            {
                _cursorController.UnlockCursor();
                _cursorController.ShowCursor();
            }
            else
            {
                _cursorController.LockCursor();
            }

            _mainMenu.SetActive(!IsMainMenuActive);
        }
    }
}