using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trellcko.Core.Input;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class CleaningMiniGame : MonoBehaviour, IMiniGame
    {
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private MopController _mopController;
        [SerializeField] private Texture2D _maskTexture;
        [SerializeField] private Material _minigameSpotMaterial;
        [SerializeField] private MeshRenderer _textRenderer;
        [SerializeField] private MeshRenderer _backroundRenderer;
        
        [SerializeField] private Image _bar;
        [SerializeField] private GameObject _ui;
        [SerializeField] private GameObject _globalUI;

        public MiniGameType MinigameType => MiniGameType.CleaningMiniGame;
        public bool IsPlaying { get; private set; }

        private readonly List<Vector2Int> _coloredPixes = new();
        private ICursorController _cursorController;
        private PlayerFacade _playerFacade;
        private CleaningMiniGamesParamsHolder _cleaningMiniGamesParams;

        public event Action<bool, IMiniGame> Finished;

        private const int TextureSize = 256;
        private const float PercentToFinishGame = 0.9f;
        private const float AnimationDuration = 2f;

        [Inject]
        private void Construct(ICursorController cursorController, PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _cursorController = cursorController;
        }

        public void StartGame(MiniGamesParamsHolder param = null)
        {
            _cleaningMiniGamesParams = (CleaningMiniGamesParamsHolder)param;
            
            _minigameSpotMaterial.mainTexture = _cleaningMiniGamesParams.Spot;
            _textRenderer.sharedMaterial = _cleaningMiniGamesParams.Text;
            _backroundRenderer.sharedMaterial = _cleaningMiniGamesParams.Background;
            _backroundRenderer.transform.rotation = Quaternion.Euler(0, _cleaningMiniGamesParams.Rotation, 0);
            
            InitTexture();
            _bar.fillAmount = 0;
            _playerFacade.PlayerMovement.IsEnabled = false;
            _playerFacade.PlayerRotation.IsEnabled = false;
            _playerFacade.Interactable.IsEnabled = false;
            _globalUI.SetActive(false);
            _ui.gameObject.SetActive(true);
            _cinemachineCamera.enabled = true;
            IsPlaying = true;
            _cursorController.UnlockCursor();
            _mopController.gameObject.SetActive(true);
            _mopController.MaskUpdated += OnMaskUpdated;
        }

        private void OnMaskUpdated()
        {
            int clearPoints = _coloredPixes.Count(t => _maskTexture.GetPixel(t.x, t.y) == Color.black);

            float percent = (float)clearPoints / _coloredPixes.Count;
            _bar.fillAmount = percent / PercentToFinishGame;
            if (percent > PercentToFinishGame)
            {
                FinishGame(true);
            }
        }

        private void InitTexture()
        {
            Color[] fill = new Color[TextureSize * TextureSize];
            _coloredPixes.Clear();

            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.white;

            _maskTexture.SetPixels(fill);

            for (int x = 0; x < TextureSize; x++)
            {
                for (int y = 0; y < TextureSize; y++)
                {
                    if (((Texture2D)_minigameSpotMaterial.mainTexture).GetPixel(x, y) != Color.clear)
                        _coloredPixes.Add(new(x, y));
                }
            }
            _maskTexture.Apply();
        }

        public void FinishGame(bool success)
        {
            _mopController.MaskUpdated -= OnMaskUpdated;
            _mopController.gameObject.SetActive(false);
            StartCoroutine(PlayFinishingAnimation(success));

        }

        public void ExitGame()
        {
            _globalUI.SetActive(true);
            _ui.gameObject.SetActive(false);
            _playerFacade.PlayerMovement.IsEnabled = true;
            _playerFacade.PlayerRotation.IsEnabled = true;
            _playerFacade.Interactable.IsEnabled = true;
            _cursorController.LockCursor();
            _cinemachineCamera.enabled = false;
            IsPlaying = false;
            _mopController.gameObject.SetActive(false);
        }

        private IEnumerator PlayFinishingAnimation(bool success)
        {
            Color[] fill = new Color[TextureSize * TextureSize];
            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.black;
            _maskTexture.SetPixels(fill);
            _maskTexture.Apply();
            yield return new WaitForSeconds(AnimationDuration);
            Finished?.Invoke(success, this);
        }
    }
}