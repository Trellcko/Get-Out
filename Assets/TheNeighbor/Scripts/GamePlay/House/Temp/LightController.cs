using System;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.MiniGame;
using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour
{
    [SerializeField] private GameObject _monster;
    [SerializeField] private CleaningMiniGame _cleaningMiniGame;
    [SerializeField] private Phone _phone;


    private void OnEnable()
    {
        _cleaningMiniGame.Finished += OnFinished;
    }

    private void OnFinished(bool arg1, IMiniGame arg2)
    {
        _phone.enabled = true;
        _monster.SetActive(true);
    }
}
