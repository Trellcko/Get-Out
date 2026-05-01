using System;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

public class Alarm : MonoBehaviour, IInteractable
{
    [field: SerializeField] public InteractableOutline InteractableOutline { get; private set; }
    
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _alarm;
    [SerializeField] private AudioClip _turnOffAlarm;

    public event Action InteractionEnabled;
    public event Action InteractionStarted;
    public event Action InteractionFinished;
    public bool IsInteractable { get; private set; }


    public void Activate()
    {
        IsInteractable = true;
        _audioSource.clip = _alarm;
        _audioSource.loop = true;
        _audioSource.Play();
        InteractionEnabled?.Invoke();
    }

    public bool TryInteract(out QuestItem getItem, QuestItem neededItem)
    {
        getItem = neededItem;
        if (!IsInteractable) return false;
        InteractionStarted?.Invoke();
        _audioSource.clip = _turnOffAlarm;
        _audioSource.loop = false;
        _audioSource.Play();
        IsInteractable = false;
        InteractableOutline.Disable();
        InteractionFinished?.Invoke();
        return true;
    }
}
