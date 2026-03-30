using System;
using Trellcko.Gameplay.Monster;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

public class TempShowAfterQuest : MonoBehaviour
{
  [SerializeField] private QuestInteractable questInteractable;
  [SerializeField] private MonsterFacade monsterFacade;

  private void OnEnable()
  {
    questInteractable.InteractionFinished += OnInteractionFinished;
  }

  private void OnDisable()
  {
    questInteractable.InteractionFinished -= OnInteractionFinished;
  }

  private void OnInteractionFinished()
  {
    monsterFacade.gameObject.SetActive(true);
  }
}
