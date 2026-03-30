using System;
using System.Collections.Generic;
using Trellcko.Core.Input;
using Trellcko.Core.Physics;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.Player;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class ClosetMiniGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private List<ClosetMiniGameData> _closetMiniGameData;
        [SerializeField] private ClothesInteractable _clothesInteractable;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _miniGameCanvas;
        [SerializeField] private RectTransform _dot;
        public bool IsPlaying { get; private set; }
        public MiniGameType MinigameType => MiniGameType.ClosetMiniGame;

        private PlayerFacade _playerFacade;
        private IInputHandler _inputHandler;
        private IRayGetter _rayGetter;
        private ICursorController _cursorController;

        public event Action<bool, IMiniGame> Finished;

        [Inject]
        private void Construct(PlayerFacade playerFacade, IInputHandler inputHandler, ICursorController cursorController)
        {
            _cursorController = cursorController;
            _inputHandler = inputHandler;
            _playerFacade = playerFacade;
        }

        private void Awake()
        {
            _rayGetter = new RayMouseGetter(_inputHandler, Camera.main);
            
        }

        private void Update()
        {
            if (IsPlaying) 
                _dot.position = _inputHandler.GetMousePosition();
        }

        public void StartGame(MiniGamesParamsHolder param = null)
        {
            IsPlaying = true;
            _miniGameCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
            _cursorController.UnlockCursor();
            _camera.enabled = true;
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
            _clothesInteractable.SetMiniGameData(_closetMiniGameData[0]);
            _clothesInteractable.ClothesRunOut += OnClothesRunOut;
        }

        private void OnClothesRunOut()
        {
            FinishGame(true);
        }


        public void FinishGame(bool success)
        {
            Finished?.Invoke(success, this);
        }

        public void ExitGame()
        {
            IsPlaying = false;
            _cursorController.LockCursor();
            _miniGameCanvas.SetActive(false);
            _mainCanvas.SetActive(true);
            _camera.enabled = false;
            _playerFacade.PlayerMovement.IsEnabled = true;
            _playerFacade.PlayerRotation.IsEnabled = true;
            _playerFacade.Interactable.IsEnabled = true;
        }
    }
}