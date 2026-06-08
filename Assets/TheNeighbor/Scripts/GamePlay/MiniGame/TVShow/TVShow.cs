using System;
using System.Collections;
using System.Collections.Generic;
using Trellcko.Dialog;
using Trellcko.Gameplay.Interactable;
using Trellcko.Gameplay.QuestLogic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class TVShow : MonoBehaviour, IMiniGame
    {
        [SerializeField] private List<TVShowData> _tvShowData;
        [SerializeField] private CinemachineCamera _cinemachineCamera;
        [SerializeField] private TVController _tvController;
        
        [SerializeField] private GameObject _ui;
        [SerializeField] private Image _image;
        [SerializeField] private GameObject _globalUI;
        
        [SerializeField] private Mouth _mouth;

        public bool IsPlaying { get; private set; }
        public MiniGameType MinigameType => MiniGameType.TVShow;
       
        private IDialogSystem _dialogSystem;
        private IQuestSystem _questSystem;

        public event Action<bool, IMiniGame> Finished;

        [Inject]
        private void Construct(IDialogSystem dialogSystem, IQuestSystem questSystem)
        {
            _questSystem = questSystem;
            _dialogSystem = dialogSystem;
        }
        
        public void StartGame(MiniGamesParamsHolder param = null)
        {
            IsPlaying = true;
            _cinemachineCamera.enabled = true;
            _ui.SetActive(true);
            _globalUI.SetActive(false);
            _tvShowData[_questSystem.Day].DialogData.OnHided += OnHided;
            _mouth.Run();

            foreach (ReplicaData replica in _tvShowData[_questSystem.Day].DialogData.ReplicaData)
            {
                replica.OnReplicaStarted += OnReplicaStarted;
                replica.OnReplicaFinished += OnReplicaFinished;
            }
            _dialogSystem.ShowDialog(_tvShowData[_questSystem.Day].DialogData);
        }

        private void OnReplicaFinished(int obj)
        {
            _mouth.Stop();
        }

        private void OnReplicaStarted(int index)
        {
            VisualData visualData = _tvShowData[_questSystem.Day].VisualData[index];
            if (visualData.Sprite)
            {
                TurnOnNoiseForTime();
                _mouth.Hide();
                _image.enabled = true;
                _image.sprite = visualData.Sprite;
                _image.color = visualData.Color;
            }
            else
            {
                if(_mouth.IsHiding)
                    TurnOnNoiseForTime();
                
                _mouth.Show();
                _mouth.Run();
                _image.enabled = false;
            }
        }

        private  void TurnOnNoiseForTime()
        {
            StartCoroutine(TurnOnTvNoiseCorun());
        }

        private IEnumerator TurnOnTvNoiseCorun()
        {
            _tvController.TurnOn();
            yield return new WaitForSeconds(0.5f);
            _tvController.TurnOff();
        }

        private void OnHided()
        {
            FinishGame(true);
        }

        public void FinishGame(bool success)
        {
            _mouth.Stop();
            Finished?.Invoke(success, this);
        }

        public void ExitGame()
        {
            _tvShowData[_questSystem.Day].DialogData.OnHided -= OnHided;
            foreach (ReplicaData replica in _tvShowData[_questSystem.Day].DialogData.ReplicaData)
            {
                replica.OnReplicaStarted -= OnReplicaStarted;
                replica.OnReplicaFinished -= OnReplicaFinished;
            }
            IsPlaying = false;
            _cinemachineCamera.enabled = false;
            _ui.SetActive(false);
            _globalUI.SetActive(true);
        }
    }
}