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
        [SerializeField] private Material _minigameSpotMaterial;
        [SerializeField] private MeshRenderer _backroundRenderer;
        [SerializeField] private Image _bar;
        [SerializeField] private GameObject _ui;
        [SerializeField] private GameObject _globalUI;

        private Texture2D _maskTexture;
        public MiniGameType MinigameType => MiniGameType.CleaningMiniGame;
        public bool IsPlaying { get; private set; }

        private readonly List<Vector2Int> _coloredPixes = new();
        private ICursorController _cursorController;
        private PlayerFacade _playerFacade;
        private CleaningMiniGamesParamsHolder _cleaningMiniGamesParams;

        public event Action<bool, IMiniGame> Finished;

        private const int TextureSize = 256;
        private const float AnimationDuration = 1f;

        [Inject]
        private void Construct(ICursorController cursorController, PlayerFacade playerFacade)
        {
            _playerFacade = playerFacade;
            _cursorController = cursorController;
        }
        
        private void Awake()
        {
            _maskTexture = new(TextureSize, TextureSize, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };
            _minigameSpotMaterial.SetTexture("_Mask", _maskTexture);
            _mopController.SetTexture(_maskTexture);
        }

        public void StartGame(MiniGamesParamsHolder param = null)
        {
            _cleaningMiniGamesParams = (CleaningMiniGamesParamsHolder)param;
            
            _minigameSpotMaterial.SetTexture("_MainTexture", _cleaningMiniGamesParams.Spot);
            _minigameSpotMaterial.color = _cleaningMiniGamesParams.SpotColor;
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
            Color32[] maskPixels = _maskTexture.GetPixels32();
            
            int clearPoints = _coloredPixes.Select(pixel => pixel.y * TextureSize + pixel.x).Count(index => maskPixels[index].r == 0 && maskPixels[index].g == 0 && maskPixels[index].b == 0);

            float percent = (float)clearPoints / _coloredPixes.Count;
            _bar.fillAmount = percent / _cleaningMiniGamesParams.PercentToFinish;

            if (percent > _cleaningMiniGamesParams.PercentToFinish)
                FinishGame(true);
        }

        private void InitTexture()
        {
            var fill = new Color32[TextureSize * TextureSize];
            _coloredPixes.Clear();

            for (int i = 0; i < fill.Length; i++)
                fill[i] = Color.white;

            _maskTexture.SetPixels32(fill);

            var spot = _cleaningMiniGamesParams.Spot;
            var pixels = spot.GetPixels32();

            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a > 0)
                {
                    int x = i % spot.width;
                    int y = i / spot.width;
                    _coloredPixes.Add(new Vector2Int(x, y));
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