using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Trellcko.Gameplay.Player;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Trellcko.Gameplay.MiniGame
{
    public class CookiesMiniGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private Vector3 _handStartPosition;
        [SerializeField] private HandController _handController;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private List<CookiesMiniGameData> _cookiesMiniGameData;
       
        [SerializeField] private GameObject _ui;
        [SerializeField] private GameObject _miniGameUI;
        [SerializeField] private TextMeshProUGUI _countText;

        [SerializeField] private float _spawnTime = 1f;
        [SerializeField] private float _minDistanceBetweenCookies = 1f;
        [SerializeField] private List<Cookie> _goodCookies;
        [SerializeField] private List<Cookie> _badCookies;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _endPoint;

        private Vector3 _previousSpawnPoint;
        private float _totalDistance;
        private float _minStep;
        private float _currentT;
        private int _currentCookies;
        private List<Cookie> _cookies;
        
            public bool IsPlaying { get; private set; }


        private PlayerFacade _playerFacade;

        private Coroutine _spawningCoroutine;

        public MiniGameType MinigameType => MiniGameType.CookiesMiniGame;
        public event Action<bool, IMiniGame> Finished;

        [Inject]
        private void Construct(PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
        }

        private void Awake()
        {
            _totalDistance = Vector3.Distance(_startPoint.position, _endPoint.position);
            _minStep = _minDistanceBetweenCookies / _totalDistance;
        }

        private void OnEnable()
        {
            _handController.CookieGot += OnCookieGot;
        }

        private void OnDisable()
        {
            _handController.CookieGot -= OnCookieGot;
        }

        public void StartGame()
        {
            _ui.SetActive(false);
            _miniGameUI.SetActive(true);
            UpdateText();
            _currentCookies = 0;
            IsPlaying = true;
            _handController.transform.localPosition = _handStartPosition;
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
            _handController.enabled = true;
            _cinemachineCamera.enabled = true;
            _spawningCoroutine = StartCoroutine(SpawningCorun());
        }

        private void UpdateText()
        {
            _countText.SetText($"Cookies {_currentCookies}/{_cookiesMiniGameData[0].needCookies}");
        }

        private IEnumerator SpawningCorun()
        {
            WaitForSeconds wait = new(_spawnTime);
            
            while (true)
            {
                yield return wait;
                Vector3 position = GetSpawnPosition();
                SpawnCookie(position);
            }
        }

        private Cookie SpawnCookie(Vector3 position)
        {
            float chance = Random.Range(0f, 1f);
            List<Cookie> cookies = chance < _cookiesMiniGameData[0].cookiesChance ? _goodCookies : _badCookies;
            
            return Instantiate(cookies[Random.Range(0, cookies.Count)], position, Quaternion.identity);
        }

        private Vector3 GetSpawnPosition()
        {
            float randomStep = Random.Range(_minStep, 1f);   
            _currentT += randomStep;
            _currentT = Mathf.PingPong(_currentT, 1);
            return Vector3.Lerp(_startPoint.position, _endPoint.position, _currentT);
        }

        public void FinishGame(bool success)
        {
            IsPlaying = false;
            StopCoroutine(_spawningCoroutine);
            Finished?.Invoke(success,this);
        }

        private void OnCookieGot(bool isGood)
        {
            if (isGood)
                GoodImpact();
            else
                BadImapact();
        }

        private void BadImapact()
        {
            
        }

        private void GoodImpact()
        {
            _currentCookies++;
            UpdateText();
            if (_currentCookies >= _cookiesMiniGameData[0].needCookies)
            {
                FinishGame(true);
            }
        }

        public void ExitGame()
        {
            _ui.SetActive(true);
            _miniGameUI.SetActive(false);
            _cinemachineCamera.enabled = false;
            _handController.enabled = false;
            _playerFacade.PlayerMovement.IsEnabled = true;
            _playerFacade.PlayerRotation.IsEnabled = true;
            _playerFacade.Interactable.IsEnabled = true;
        }
    }

    [Serializable]
    public class CookiesMiniGameData
    {
        public float cookiesChance;
        public int needCookies;
    }
}