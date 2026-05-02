using System;
using System.Collections;
using Trellcko.Core.Audio;
using Trellcko.Gameplay;
using Trellcko.Gameplay.MiniGame;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TurnOffLight : MonoBehaviour
{
    [SerializeField] private TVController _TVController;
    [SerializeField] private LookAtPlayer _monster;
    [SerializeField] private Light _light;
    [SerializeField] private GameObject _player;
    private Vector3 _playerPosition = new Vector3(0.037f, -1.248f, 0.695f);

    private void Awake()
    {
        _TVController.TurnOn();
    }

    private void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            _monster.enabled = true;
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            _light.enabled = false;
            _TVController.TurnOff();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _light.enabled = true;
            _monster.transform.parent = _player.transform;
            _monster.transform.localPosition = _playerPosition;
        }
    }
}
