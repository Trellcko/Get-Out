using System;
using System.Collections.Generic;
using System.Linq;
using Trellcko.Core.Input;
using Trellcko.Gameplay.MiniGame;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

public class CleaningMiniGame : MonoBehaviour, IMiniGame
{
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private MopController _mopController;
    [SerializeField] private Texture2D _maskTexture;
    [SerializeField] private Texture2D _originalTexture;
    
    public MiniGameType MinigameType => MiniGameType.CleaningMiniGame;
    public bool IsPlaying { get; private set; }

    private readonly List<Vector2Int> _coloredPixes = new();
    private ICursorController _cursorController;
    
    public event Action<bool, IMiniGame> Finished;

    private const int TextureSize = 256;
    private const float PercentToFinishGame = 0.95f;
    
    [Inject]
    private void Construct(ICursorController cursorController)
    {
        _cursorController = cursorController;
    }
    
    public void StartGame()
    {
        InitTexture();

        _cinemachineCamera.enabled = true;
        IsPlaying = true;
        _cursorController.UnlockCursor();
        _mopController.gameObject.SetActive(true);
        _mopController.MaskUpdated += OnMaskUpdated;
    }

    private void OnMaskUpdated()
    {
        int clearPoints = _coloredPixes.Count(t => _maskTexture.GetPixel(t.x, t.y) == Color.black);

        float percent = (float)clearPoints/ _coloredPixes.Count;
        if (percent > PercentToFinishGame)
        {
            Finished?.Invoke(true, this);
        }
    }

    private void InitTexture()
    {
        Color[] fill = new Color[TextureSize * TextureSize];
        _coloredPixes.Clear();
        
        for(int i=0; i<fill.Length; i++) 
            fill[i] = Color.white;
        
        _maskTexture.SetPixels(fill);

        for (int x = 0; x < TextureSize; x++)
        {
            for (int y = 0; y < TextureSize; y++)
            {
                if(_originalTexture.GetPixel(x, y) != Color.clear)
                    _coloredPixes.Add(new(x, y));
            }
        }
        
        Debug.LogError(_coloredPixes.Count);
        
        _maskTexture.Apply();
        
    }

    public void FinishGame(bool success)
    {
        _mopController.MaskUpdated -= OnMaskUpdated;
        Finished?.Invoke(success, this);
    }

    public void ExitGame()
    {
        _cursorController.LockCursor();
        _cinemachineCamera.enabled = false;
        IsPlaying = false;
        _mopController.gameObject.SetActive(false);
    }
}
