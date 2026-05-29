using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.House
{
   public class Room : MonoBehaviour
   {
      [field: SerializeField] public RoomType RoomType { get; private set; }
      
      [SerializeField] private List<QuestInteractable> _questInteractables;
      
      [SerializeField] private List<Light> _lights;
      public IReadOnlyList<Light> Lights => _lights;
      
      public bool HasQuestInteractable(QuestInteractable questInteractable) => 
         _questInteractables.Contains(questInteractable);

      [Button]
      private void CollectAll()
      {
         _questInteractables = GetComponentsInChildren<QuestInteractable>().ToList();
         _lights = GetComponentsInChildren<Light>().ToList();
      }
   }
}