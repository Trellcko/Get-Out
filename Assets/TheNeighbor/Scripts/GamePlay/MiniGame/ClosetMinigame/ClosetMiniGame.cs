using System;
using System.Collections.Generic;
using System.Linq;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.Player;
using Trellcko.Gameplay.QuestLogic;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class ClosetMiniGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private List<ClosetMiniGameData> _closetMiniGameData;
        [SerializeField] private Material _clothes;
        [SerializeField] private Material _corpse;
        [SerializeField] private List<ClothesAnchor> _clothesAnchors;
        [SerializeField] private ClothesInteractable _clothesInteractable;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _miniGameCanvas;
        [SerializeField] private RectTransform _dot;
        public bool IsPlaying { get; private set; }
        public MiniGameType MinigameType => MiniGameType.ClosetMiniGame;

        private PlayerFacade _playerFacade;
        private IInputHandler _inputHandler;
        private ICursorController _cursorController;
        private IQuestSystem _questSystem;

        public event Action<bool, IMiniGame> Finished;

        [Inject]
        private void Construct(PlayerFacade playerFacade, IInputHandler inputHandler,
            ICursorController cursorController, IQuestSystem questSystem)
        {
            _questSystem = questSystem;
            _cursorController = cursorController;
            _inputHandler = inputHandler;
            _playerFacade = playerFacade;
        }

        private void Update()
        {
            if (IsPlaying) 
                _dot.position = _inputHandler.GetMousePosition();
        }

        public void StartGame(MiniGamesParamsHolder param = null)
        {
            IsPlaying = true;
            foreach (var clothesAnchor in _clothesAnchors)
            {
                clothesAnchor.Count = 0;
                clothesAnchor.MeshRenderer.enabled = false;
            }
            _miniGameCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
            _cursorController.UnlockCursor();
            _camera.enabled = true;
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
            _clothesInteractable.SetMiniGameData(_closetMiniGameData[_questSystem.Day]);
            _clothesInteractable.RunOut += OnRunOut;
            _clothesInteractable.Putted += OnPutted;
        }

        private void OnPutted(bool isCorpse)
        {
            ClothesAnchor anchor = _clothesAnchors.FirstOrDefault(x =>
                x.IsCorpse == isCorpse && x.Count is > 0 and < ClothesAnchor.MaxPerStack);

            if (anchor == null)
            {
                anchor = _clothesAnchors.FirstOrDefault(x => x.Count == 0);
                if (anchor == null) return;
                anchor.IsCorpse = isCorpse;
                anchor.MeshRenderer.sharedMaterial = isCorpse ? _corpse : _clothes;
                anchor.MeshRenderer.enabled = true;
            }

            anchor.Count++;
        }

        private void OnRunOut()
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
            _clothesInteractable.RunOut -= OnRunOut;
            _clothesInteractable.Putted -= OnPutted;
        }
    }

    [Serializable]
    public class ClothesAnchor
    {
       public MeshRenderer MeshRenderer;
       public bool IsCorpse;
       public int Count;
       
       public const int  MaxPerStack = 3;
    }
}