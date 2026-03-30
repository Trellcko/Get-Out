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
using Trellcko.Core.Audio;
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
        [SerializeField] private TextMeshProUGUI _timer;
        [SerializeField] private MeshRenderer _playerMeshRenderer;
        [SerializeField] private Material[] _playerMaterials;
        public bool IsPlaying { get; private set; }

        private QuestSystem _questSystem;
        private IInputHandler _inputHandler;
        private Coroutine _miniGameCoroutine;
        private PlayerFacade _playerFacade;
        private ISoundController _soundController;

        private int _lastIndex;
        
        public event Action<bool, IMiniGame> Finished;

        public MiniGameType MinigameType => MiniGameType.WindowMiniGame;

        [Inject]
        private void Construct(PlayerFacade playerFacade, IInputHandler inputHandler, ISoundController soundController)
        {
            _soundController = soundController;
            _inputHandler = inputHandler;
            _playerFacade = playerFacade;
        }
        
        public void StartGame(MiniGamesParamsHolder param)
        {
            _slider.fillAmount = 0;
            _soundController.PlayPlayerSound(PlayerSound.Crying);
            IsPlaying = true;
            _UI.SetActive(true);
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
            _camera.enabled = true;
            
            _inputHandler.SpaceClicked += OnSpaceClicked;
            
            _playerMeshRenderer.sharedMaterial = _playerMaterials[0];
            
            if(_miniGameCoroutine != null)
                StopCoroutine(_miniGameCoroutine);
            _miniGameCoroutine = StartCoroutine(MiniGameCycle());
            
        }

        public void FinishGame(bool success)
        {
            Finished?.Invoke(success, this);
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
            _slider.fillAmount += _data[0].power;
            UpdatePlayerMaterials();
            if (_slider.fillAmount >= 1)
            {
                FinishGame(true);
            }
        }

        private IEnumerator MiniGameCycle()
        {
            float currentTime = _data[0].time;
            
            while (currentTime > 0)
            {
                _timer.SetText(((int)currentTime).ToString("00"));
                _slider.fillAmount -= _data[0].fallDownSpeed;
                currentTime -= Time.deltaTime;
                UpdatePlayerMaterials();
                yield return null;
            }
            FinishGame(false);
            
        }

        private void UpdatePlayerMaterials()
        {
            int index = Mathf.FloorToInt((_slider.fillAmount*100) / (100f / _playerMaterials.Length));
            index = Mathf.Clamp(index, 0,  _playerMaterials.Length - 1);
            _playerMeshRenderer.sharedMaterial = _playerMaterials[index];
            if (_lastIndex != index)
            {
                _lastIndex = index;
                switch (index)
                {
                    case 0:
                        _soundController.PlayPlayerSound(PlayerSound.Crying);
                        break;
                    case 1:
                        _soundController.PlayPlayerSound(PlayerSound.Whimper);
                        break;
                    case 2:
                        _soundController.PlayPlayerSound(PlayerSound.Breathing);
                        break;
                    case 3:
                        _soundController.PlayPlayerSound(PlayerSound.Chuckle);
                        break;
                }
            }
        }
    }
}
