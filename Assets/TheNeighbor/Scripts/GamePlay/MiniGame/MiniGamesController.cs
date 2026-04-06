using System;
using System.Collections.Generic;
using System.Linq;
using Trellcko.Gameplay.Interactable;
using Trellcko.UI;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
   public class MiniGamesController : MonoBehaviour
   {
      [SerializeField] private List<GameObject> _minigamesGO;

      private TransitionUI _transitionUI;
      private readonly List<IMiniGame> _minigames = new();
      private Action _onFinished;

      [Inject]
      private void Construct(TransitionUI transitionUI)
      {
         _transitionUI = transitionUI;
      }

      private void Awake()
      {
         foreach (GameObject miniGameGo in _minigamesGO)
         {
            if (miniGameGo.TryGetComponent(out IMiniGame miniGame))
            {
               _minigames.Add(miniGame);
            }
            else
            {
               Debug.LogError($"{miniGameGo.name} has no IMiniGame");
            }
         }
      }

      public void StartMiniGame(MiniGameType miniGameType, Action onFinished, MiniGamesParamsHolder param = null)
      {
         foreach (IMiniGame minigame in _minigames.Where(minigame => minigame.MinigameType == miniGameType))
         {
            _onFinished = onFinished;
            _transitionUI.ShowAndHideUI(-1, ()=> minigame.StartGame(param));
            minigame.Finished += OnFinished;
            return;
         }

         Debug.LogError($"No MiniGame {miniGameType} found");
      }

      private void OnFinished(bool success, IMiniGame minigame)
      {
         minigame.Finished -= OnFinished;
         _transitionUI.ShowAndHideUI(-1, ()=>
         {
            minigame.ExitGame();
            _onFinished();
            _onFinished = null;
         });
      }
   }

   public enum MiniGameType
   {
      WindowMiniGame,
      ClosetMiniGame,
      CookiesMiniGame,
      CleaningMiniGame,
      WateringMiniGame
   }
}