using System;
using NaughtyAttributes;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.House
{
   public class Wall : MonoBehaviour
   {
      [field: SerializeField] public WallType WallType { get; private set; }
      [SerializeField] private GameObject _glitchWall;
      [SerializeField] private GameObject _wall;
      
      [ShowIf(nameof(HasGlitchWall))]
      [SerializeField] private int _dayToShowGlitch;

      private bool HasGlitchWall => _glitchWall;
      
      private IQuestSystem _questSystem;
      private DiContainer _container;
      private GameObject _glitchWallInstance;

      [Inject]
      private void Construct(IQuestSystem questSystem, DiContainer container)
      {
         _container = container;
         _questSystem = questSystem;
      }

      private void Start()
      {
         OnDayStarted();
      }

      private void OnEnable()
      {
         if(!HasGlitchWall) return;
         _questSystem.DayStarted += OnDayStarted;
      }

      private void OnDisable()
      {
         _questSystem.DayStarted -= OnDayStarted;
      }

      private void OnDayStarted()
      {
         if (HasGlitchWall && _questSystem.Day == _dayToShowGlitch)
         {
            _questSystem.DayStarted -= OnDayStarted;
            ShowGlitchWall();
         }
      }

      public void HideGlitchWall()
      {
         _glitchWallInstance?.SetActive(false);
         _wall.SetActive(true);
      }
      
      public void ShowGlitchWall()
      {
         if (!_glitchWallInstance)
         {
            _glitchWallInstance = _container.InstantiatePrefab(_glitchWall, transform.position, transform.rotation, transform);
         }
         else
         {
            _glitchWallInstance.SetActive(true);
         }
         _glitchWallInstance.transform.localScale = Vector3.one;
         _wall.SetActive(false);
      }
   }

   public enum WallType
   {
      Normal,
      WithDoor,
   }
}