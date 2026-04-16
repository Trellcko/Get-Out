using System;
using System.Collections;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.Events;
using Trellcko.Gameplay.MiniGame;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class TurnOffLight : MonoBehaviour
{

    [SerializeField] private GameObject _monster;
    [SerializeField] private ClothesInteractable _clothesInteractable;
    [SerializeField] private Light _light;

    private ISoundController _mainSoundController;

    [Inject]
    private void Construct(ISoundController mainSoundController)
    {
        _mainSoundController = mainSoundController;
    }
    
    private void OnEnable()
    {
        _clothesInteractable.Generated += OnGenerated;    
    }

    private void OnGenerated(bool obj)
    {
        if(obj)
            StartCoroutine(TriggerCorun());
    }

    private IEnumerator TriggerCorun()
    {
        _mainSoundController.PlayShockMoment();
        _light.color = Color.red;
        _monster.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        _light.color = Color.white;
        _monster.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            _monster.gameObject.SetActive(!_monster.activeSelf);
        }
    }
}
