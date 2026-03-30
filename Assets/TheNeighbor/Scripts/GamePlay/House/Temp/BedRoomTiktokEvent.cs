using System;
using Trellcko.Gameplay;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

public class BedRoomTiktokEvent : MonoBehaviour
{
   [SerializeField] private TVController _TVController;
   [SerializeField] private Alarm _alarm;
   [SerializeField] private GameObject _monster;

   private void Awake()
   {
      _alarm.Activate();
      _alarm.InteractionStarted += OnInteracted;
   }

   private void OnInteracted()
   {
      _TVController.TryInteract(out _, QuestItem.None);
      _TVController.InteractionStarted += OnInteractedTv;
   }

   private void OnInteractedTv()
   {
      _monster.SetActive(true);
   }
}
