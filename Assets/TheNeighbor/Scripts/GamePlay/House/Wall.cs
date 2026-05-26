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
      [SerializeField] private GlitchWall _glitchWall;
      [SerializeField] private GameObject _wall;
      [SerializeField] private GameObject[] _disableObjects;
      
      [ShowIf(nameof(HasGlitchWall))]
      [SerializeField] private int _dayToShowGlitch;

      private bool HasGlitchWall => _glitchWall;
      
      private IQuestSystem _questSystem;
      private DiContainer _container;
      private GlitchWall _glitchWallInstance;

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
         if (HasGlitchWall && _questSystem.Day >= _dayToShowGlitch)
         {
            _questSystem.DayStarted -= OnDayStarted;
            ShowGlitchWall();
            DisableObjects();
         }
      }

      private void DisableObjects()
      {
         foreach (GameObject disableObject in _disableObjects)
         {
            disableObject.SetActive(false);
         }
      }

      public void HideGlitchWall()
      {
         _glitchWallInstance?.gameObject.SetActive(false);
         _wall.SetActive(true);
      }
      
      public void ShowGlitchWall()
      {
         if (!_glitchWallInstance)
         {
            _glitchWallInstance = _container.InstantiatePrefab(_glitchWall, transform.position, transform.rotation, transform).GetComponent<GlitchWall>();
         }
         else
         {
            _glitchWallInstance.gameObject.SetActive(true);
         }
         _glitchWallInstance.transform.localScale = Vector3.one;
         uint lightLayer = _wall.GetComponentInChildren<MeshRenderer>().renderingLayerMask;
         _glitchWallInstance.SetRendererLayerMask(lightLayer);
         _wall.SetActive(false);
      }
   }

   public enum WallType
   {
      Normal,
      WithDoor,
   }
}