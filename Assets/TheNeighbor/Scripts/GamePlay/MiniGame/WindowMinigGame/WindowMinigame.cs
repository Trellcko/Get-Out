using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Trellcko.Core.Audio;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Player;
using Trellcko.Gameplay.QuestLogic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Trellcko.Gameplay.Interactable;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class WindowMinigame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private List<WindowMiniGameData> _data;
        [SerializeField] private Image _slider;
        [SerializeField] private GameObject _UI;
        [SerializeField] private CinemachineCamera _camera;
        [SerializeField] private MeshRenderer _playerMeshRenderer;
        public bool IsPlaying { get; private set; }

        private IQuestSystem _questSystem;
        private IInputHandler _inputHandler;
        private Coroutine _miniGameCoroutine;
        private PlayerFacade _playerFacade;
        private ISoundController _soundController;

        private int _lastIndex;
        
        public event Action<bool, IMiniGame> Finished;

        public MiniGameType MinigameType => MiniGameType.WindowMiniGame;

        [Inject]
        private void Construct(PlayerFacade playerFacade, IInputHandler inputHandler, ISoundController soundController,
            IQuestSystem questSystem)
        {
            _soundController = soundController;
            _inputHandler = inputHandler;
            _playerFacade = playerFacade;
            _questSystem = questSystem;
        }
        
        public void StartGame(MiniGamesParamsHolder param)
        {
            _slider.fillAmount = 0;
            _soundController.PlayPlayerSound(PlayerSound.Breathing);
            IsPlaying = true;
            _UI.SetActive(true);
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
            _camera.enabled = true;
            
            _inputHandler.SpaceClicked += OnSpaceClicked;

            _playerMeshRenderer.sharedMaterial = _data[_questSystem.Day].Normal;
            
            if(_miniGameCoroutine != null)
                StopCoroutine(_miniGameCoroutine);
            _miniGameCoroutine = StartCoroutine(MiniGameCycle());
            
        }

        public void FinishGame(bool success)
        {
            Finished?.Invoke(success, this);
            StopCoroutine(_miniGameCoroutine);
            IsPlaying = false;
        }

        public void ExitGame()
        {
            _UI.SetActive(false);
            _playerFacade.PlayerMovement.IsEnabled = true;
            _playerFacade.PlayerRotation.IsEnabled = true;
            _playerFacade.Interactable.IsEnabled = true;
            _camera.enabled = false;

            _inputHandler.SpaceClicked -= OnSpaceClicked;

            if(_miniGameCoroutine != null)
                StopCoroutine(_miniGameCoroutine);
        }

        private void OnSpaceClicked()
        {
            _slider.fillAmount += _data[_questSystem.Day].power;
            UpdatePlayerMaterials();
            if (_slider.fillAmount >= 1)
            {
                FinishGame(true);
            }
        }

        private IEnumerator MiniGameCycle()
        {
            while (true)
            {
                _slider.fillAmount -= _data[_questSystem.Day].fallDownSpeed;
                UpdatePlayerMaterials();
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
        }

        private void UpdatePlayerMaterials()
        {
            int index = Mathf.FloorToInt((_slider.fillAmount*100) / (100f / 2));
            index = Mathf.Clamp(index, 0,  1);
            _playerMeshRenderer.sharedMaterial = index == 0? _data[_questSystem.Day].Normal : _data[_questSystem.Day].Happy;
            if (_lastIndex != index)
            {
                _lastIndex = index;
                switch (index)
                {
                    case 0:
                        _soundController.PlayPlayerSound(PlayerSound.Breathing);
                        break;
                    case 1:
                        _soundController.PlayPlayerSound(_data[_questSystem.Day].Laugh);
                        break;
                }
            }
        }
    }
}
