using System;
using System.Collections;
using Trellcko.Core.Audio;
using Trellcko.Core.Input;
using Trellcko.UI;
using UnityEngine;
using Zenject;

namespace Trellcko.Dialog
{
    public class DialogSystem : MonoBehaviour, IDialogSystem
    {
        [SerializeField] private DialogUI _dialogUI;
        [SerializeField] private AudioSource _audioSource;

        private bool _isShowing;
        private DialogData _dialogData;
        private int _counter;
        private IInputHandler _inputHandler;
        private ISoundController _soundController;
        private bool _useAudio;

        public const float DelayStandart = 0.05f;

        [Inject]
        private void Construct(ISoundController soundController, IInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
            _soundController = soundController;
        }
        
        public void ShowDialog(DialogData dialogData)
        {
            if(_isShowing) return;
            _isShowing = true;
            _counter = 0;
            _dialogData = dialogData;
            _dialogData.OnShowed?.Invoke();
            _dialogUI.Activate();

            ShowNextReplica();
        }

        private void ShowNextReplica()
        {
            Action replicaFinished = null;
            _dialogUI.SetStateForTip(false);
            ReplicaData replicaData = _dialogData.ReplicaData[_counter];
            _inputHandler.SpaceClicked -= OnSpaceClicked;
            replicaFinished += () =>
            {
                replicaData?.OnReplicaFinished?.Invoke(_counter);
                _soundController.StopPlayingOtherSound();
                SubscribeToSpace();
            };
            float durationPerCharacter = DelayStandart;
            replicaData.OnReplicaStarted?.Invoke(_counter);
            _counter++;
            
            _dialogUI.ShowText(replicaData.Text, durationPerCharacter,
                replicaData.Delay);
            
            if (replicaData.Audio)
            {
                _audioSource.clip = replicaData.Audio;
                _audioSource.Play();
            //    durationPerCharacter = _audioSource.clip.length / replicaData.Text.Length;
                StartCoroutine(WaitedForFinishingReplica(replicaFinished));
            }
            else
            {
                replicaFinished?.Invoke();
            }
            
        }

        private IEnumerator WaitedForFinishingReplica(Action onFinishing = null)
        {
            yield return new WaitWhile(()=>_audioSource.isPlaying);
            onFinishing?.Invoke();
            _dialogUI.SetStateForTip(true);
        }

        private void SubscribeToSpace()
        {
            _inputHandler.SpaceClicked += OnSpaceClicked;
        }

        private void OnSpaceClicked()
        {
            _inputHandler.SpaceClicked -= OnSpaceClicked;
            if (_counter < _dialogData.ReplicaData.Count)
            {
                ShowNextReplica();
            }
            else
            {
                _dialogUI.Deactivate();
                _isShowing = false;
                _dialogData.OnHided?.Invoke();
            }
        }
    }
}