using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Trellcko.Core.Audio;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Interactable;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Trellcko.Gameplay.MiniGame
{
    public class WateringMiniGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private List<WateringMiniGameData> _wateringMiniGameData;

        [SerializeField] private RectTransform _dot;
        [SerializeField] private RectTransform _fillBar;
        [SerializeField] private TextMeshProUGUI _percentText;

        [SerializeField] private GameObject _gloablUI;
        [SerializeField] private GameObject _miniGameUI;
        
        [SerializeField] private CinemachineCamera _camera;

        [SerializeField] private Transform _waterCan;
        [SerializeField] private ParticleSystem _waterDrops;

        [SerializeField] private float _wateringAngel = 30f;
        [SerializeField] private float _power = 1f;
        [SerializeField] private float _gravity = 0.5f;
        
        public bool IsPlaying { get; private set; }
        public MiniGameType MinigameType => MiniGameType.WateringMiniGame;
        public event Action<bool, IMiniGame> Finished;
        private IInputHandler _inputHandler;
        private ISoundController _soundController;
        
        private Tween _waterCanRotationTween;
        
        private float _velocity;
        private float _fillBarRandomY;

        private bool _isBarWaiting;
        private bool _isWatering;
        
        private float _currentFillTime;

        private const float DotBorder = 300f;
        private const float FillBarBorder = 256f;
        private const float FillAndDotMaxDifference = 80f;
        
        [Inject]
        private void Construct(IInputHandler inputHandler, ISoundController soundController)
        {
            _soundController = soundController;
            _inputHandler = inputHandler;
        }
        
        public void StartGame(MiniGamesParamsHolder param = null)
        {
            _gloablUI.gameObject.SetActive(false);
            _miniGameUI.gameObject.SetActive(true);
            _camera.enabled = true;
            IsPlaying = true;
            _inputHandler.SpaceClicked += OnSpaceClicked;
            GeneratePositionToMove();
        }

        private void Update()
        {
            if(!IsPlaying) return;
            MoveBar();
            MoveDot();
            TryFilling();
        }

        private void TryFilling()
        {
            if (Mathf.Abs(_dot.anchoredPosition.y - _fillBar.anchoredPosition.y) < FillAndDotMaxDifference)
            {
                if (!_isWatering)
                {
                    _isWatering = true;
                     _waterCan.DOKill();
                     _soundController.PlayOtherSound(OtherSound.WaterCan1);
                     _waterCan.DOLocalRotate(new(0, 90, _wateringAngel), 0.5f)
                        .OnComplete(()=>
                        {
                            _waterDrops.Play();
                            _soundController.PlayOtherSound(OtherSound.Watering);
                        });
                }
                _currentFillTime =
                    Mathf.Clamp(_currentFillTime + Time.deltaTime, 0, _wateringMiniGameData[0].TimeToFill);
                _percentText.SetText($"{(100*(_currentFillTime/_wateringMiniGameData[0].TimeToFill)):N0}/100%");
                
                if(_currentFillTime >= _wateringMiniGameData[0].TimeToFill)
                    FinishGame(true);
                return;
            }
            
            if (_isWatering)
            {
                
                _soundController.PlayOtherSound(OtherSound.WaterCan2);
                _isWatering = false;
                _waterCan.DOKill();
                _waterDrops.Stop();
                _waterCan.DOLocalRotate(new(0, 90, 0), 0.5f);
            }
        }

        
        private void MoveBar()
        {
            if(_isBarWaiting) return;
            
            _fillBar.anchoredPosition = new(_fillBar.anchoredPosition.x, 
                Mathf.MoveTowards(_fillBar.anchoredPosition.y, _fillBarRandomY,
                    Time.deltaTime * _wateringMiniGameData[0].Speed));

            if (Mathf.Abs(_fillBar.anchoredPosition.y - _fillBarRandomY) < 1f)
            {
                StartCoroutine(GeneratePositionAfterWaiting(_wateringMiniGameData[0].MinTimeToWait));
            }
        }

        private IEnumerator GeneratePositionAfterWaiting(float time)
        {
            _isBarWaiting = true;
            yield return new WaitForSeconds(time);
            GeneratePositionToMove();
            _isBarWaiting = false;
        }

        private void GeneratePositionToMove()
        {
            float random = Random.Range(-FillBarBorder, FillBarBorder);
            float abs = Mathf.Abs(random - _fillBarRandomY);
            if (abs < _wateringMiniGameData[0].MinDistance)
            {
                int modificator = random < _fillBarRandomY ? -1 : 1;

                random = Mathf.Repeat(random + FillBarBorder + _wateringMiniGameData[0].MinDistance * modificator,
                    FillBarBorder * 2);
                _fillBarRandomY = random - FillBarBorder;
            }
            else
            {
                _fillBarRandomY = random;
            }
        }

        private void MoveDot()
        {
            _velocity += _gravity * Time.deltaTime;

            float newY = _dot.anchoredPosition.y - _velocity * Time.deltaTime;
            newY = Mathf.Clamp(newY, -DotBorder, DotBorder);

            _dot.anchoredPosition = new(_dot.anchoredPosition.x, newY);
        }

        private void OnSpaceClicked()
        {
            _velocity = -_power;
        }

        public void FinishGame(bool success)
        {
            _inputHandler.SpaceClicked -= OnSpaceClicked;
            Finished?.Invoke(success, this);
        }

        public void ExitGame()
        {
            _gloablUI.gameObject.SetActive(true);
            _miniGameUI.gameObject.SetActive(false);
            _camera.enabled = false;
            IsPlaying = false;
        }
    }

    
    [Serializable]
    public class WateringMiniGameData
    {
        public float MinDistance;
        public float MinTimeToWait;
        public float Speed;
        public float TimeToFill;
    }
}