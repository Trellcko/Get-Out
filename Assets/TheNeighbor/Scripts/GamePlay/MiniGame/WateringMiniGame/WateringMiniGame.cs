using System;
using System.Collections.Generic;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Interactable;
using Unity.Cinemachine;
using UnityEditor.Tilemaps;
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
        
        private float _velocity;
        private float _fillBarRandomY = 0f;

        private const float DotBorder = 300f;
        private const float FillBarBorder = 256f;
        private const float FillAndDotMaxDifference = 80f;
        
        [Inject]
        private void Construct(IInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }
        
        public void StartGame(MiniGamesParamsHolder param = null)
        {
            _gloablUI.gameObject.SetActive(false);
            _miniGameUI.gameObject.SetActive(true);
            _camera.enabled = true;
            IsPlaying = true;
            _inputHandler.SpaceClicked += OnSpaceClicked;
            _fillBarRandomY = Random.Range(-FillBarBorder, FillBarBorder);
        }

        private void Update()
        {
            MoveBar();

            MoveDot();

            TryFilling();
        }

        private void TryFilling()
        {
            if (Mathf.Abs(_dot.anchoredPosition.y - _fillBar.anchoredPosition.y) < FillAndDotMaxDifference)
            {
                
            }
        }

        private void MoveBar()
        {
            _fillBar.anchoredPosition = new(_fillBar.anchoredPosition.x, 
                Mathf.Lerp(_fillBar.anchoredPosition.y, _fillBarRandomY,
                    Time.deltaTime * _wateringMiniGameData[0].Speed));

            if (Mathf.Abs(_fillBar.anchoredPosition.y - _fillBarRandomY) < 1f)
            {
                _fillBarRandomY = Random.Range(-FillBarBorder, FillBarBorder);
            }
        }

        private void MoveDot()
        {
            _velocity += _gravity * Time.deltaTime;

            float newY = _dot.anchoredPosition.y - _velocity * Time.deltaTime;
            newY = Mathf.Clamp(newY, -DotBorder, DotBorder);

            _dot.anchoredPosition = new Vector2(_dot.anchoredPosition.x, newY);
        }

        private void OnSpaceClicked()
        {
            _velocity = -_power;
        }

        public void FinishGame(bool success)
        {
            _inputHandler.SpaceClicked -= OnSpaceClicked;
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
    }
}