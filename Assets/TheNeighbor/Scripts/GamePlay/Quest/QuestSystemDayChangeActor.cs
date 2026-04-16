using System;
using System.Collections.Generic;
using Trellcko.Gameplay.Cinematic;
using Trellcko.Gameplay.Player;
using Trellcko.UI;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.QuestLogic
{
    public class QuestSystemDayChangeActor : MonoBehaviour
    {
        [SerializeField] private List<BaseCinematic> _cinematic;

        private TransitionUI _transitionUI;
        private GameUI _gameUI;
        private PlayerFacade _player;
        private IQuestSystem _questSystem;
        private IDayResetting _dayResetting;

        private PlayerMovement PlayerMovement => _player.PlayerMovement;
        private PlayerRotation PlayerRotation => _player.PlayerRotation;

        [Inject]
        private void Construct(IQuestSystem questSystem, IDayResetting dayResetting, PlayerFacade player, TransitionUI transitionUI, GameUI gameUI)
        {
            _dayResetting = dayResetting;
            _player = player;
            _questSystem = questSystem;
            _transitionUI = transitionUI;
            _gameUI = gameUI;
        }

        private void Start()
        {
            _questSystem.DayCompleted += OnDayCompleted;
            _dayResetting.ResetItemsFor(_questSystem.Day);
        }

        private void OnDestroy()
        {
            _questSystem.DayCompleted -= OnDayCompleted;
        }

        private void OnDayCompleted()
        {
            PlayerMovement.IsEnabled = PlayerRotation.IsEnabled = false;

            if (_cinematic[_questSystem.Day])
            {
                ShowCinematic();
            }
            else if(!_questSystem.AreAllQuestsCompleted)
            {
                AnimationStartNextDay();
            }
            else
            {
                FinishGameAnimation();
            }
        }

        private void FinishGameAnimation()
        {
            _transitionUI.ShowFinishGameUI();
        }

        private void OnCinematicCompleted()
        {
            _cinematic[_questSystem.Day].Completed -= OnCinematicCompleted;
            _gameUI.gameObject.SetActive(true);
            AnimationStartNextDay(_cinematic[_questSystem.Day].DisableObjects);
        }

        private void ShowCinematic()
        {
            _transitionUI.ShowUI(-1, () =>
            {
                _gameUI.gameObject.SetActive(false);
                _cinematic[_questSystem.Day].Play();
                _cinematic[_questSystem.Day].Completed += OnCinematicCompleted;
                _transitionUI.HideUI();
            });
        }

        private void AnimationStartNextDay(Action callback = null)
        {
            Debug.Log("Show UI next day");
            _transitionUI.ShowUI(_questSystem.Day + 2, () =>
            {
                
                Debug.Log("Reset next day");
                _dayResetting.ResetItemsFor(_questSystem.Day + 1);
                Debug.Log("Start next day");
                _questSystem.StartNextDay();
                
                Debug.Log("Hide Ui next day");
                _transitionUI.HideUI(() =>
                {
                    PlayerMovement.IsEnabled = PlayerRotation.IsEnabled = true;
                });
                callback?.Invoke();
            });
        }
    }
}