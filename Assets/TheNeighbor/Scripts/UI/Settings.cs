using System;
using Trellcko.Core.Input;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

public class Settings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private FillSliderHandleUI _music;
    [SerializeField] private FillSliderHandleUI _sound;
    [SerializeField] private FillSliderHandleUI _mouseIntensity;

    [SerializeField] private Vector2 _minMaxValue = new Vector2(-80, 10);
    [SerializeField] private Vector2 _intensityRange = new Vector2(0.1f, 10);
    private IInputHandler _inputHandler;

    private void OnEnable()
    {
        _music.ValueChanged += UpdateMusic;
        _sound.ValueChanged += UpdateSound;
        _mouseIntensity.ValueChanged += UpdateIntensity;
    }

    private void OnDisable()
    {
        _music.ValueChanged -= UpdateMusic;
        _sound.ValueChanged -= UpdateSound;
        _mouseIntensity.ValueChanged -= UpdateIntensity;
    }

    [Inject]
    private void Construct(IInputHandler handler)
    {
        _inputHandler = handler;
    }

    private void UpdateMusic(float musicValue)
    {
        _audioMixer.SetFloat("Music", Mathf.Lerp(_minMaxValue.x, _minMaxValue.y, musicValue));
    }

    private void UpdateSound(float soundValue)
    {
        _audioMixer.SetFloat("Sounds", Mathf.Lerp(_minMaxValue.x, _minMaxValue.y, soundValue));
    }

    private void UpdateIntensity(float intensityValue)
    {
        _inputHandler.SetMouseIntensity(Mathf.Lerp(_intensityRange.x, _intensityRange.y, intensityValue));
    }
}
