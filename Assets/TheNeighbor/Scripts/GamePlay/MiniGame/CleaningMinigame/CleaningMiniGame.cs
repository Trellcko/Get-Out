using System;
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
    
    private ICursorController _cursorController;
    public bool IsPlaying { get; private set; }
    public MiniGameType MinigameType => MiniGameType.CleaningMiniGame;
    public event Action<bool, IMiniGame> Finished;

    [Inject]
    private void Construct(ICursorController cursorController)
    {
        _cursorController = cursorController;
    }
    
    public void StartGame()
    {
        ClearTexture();

        _cinemachineCamera.enabled = true;
        IsPlaying = true;
        _cursorController.UnlockCursor();
        _mopController.gameObject.SetActive(true);
    }

    private void ClearTexture()
    {
        Color[] fill = new Color[256 * 256];
        for(int i=0; i<fill.Length; i++) fill[i] = Color.white;
        _maskTexture.SetPixels(fill);
        _maskTexture.Apply();
    }

    public void FinishGame(bool success)
    {
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
