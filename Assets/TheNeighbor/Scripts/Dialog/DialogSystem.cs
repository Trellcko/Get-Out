using System;
using Trellcko.Core.Audio;
using Trellcko.UI;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Trellcko.Dialog
{
    public class DialogSystem : MonoBehaviour, IDialogSystem
    {
        [SerializeField] private DialogUI _dialogUI;

        private bool _isShowing;
        private DialogData _dialogData;
        private int _counter;
        private ISoundController _soundController;

        [Inject]
        private void Construct(ISoundController soundController)
        {
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
            Action hidedAction = null;
            Action showAction = null;
            
            ReplicaData replicaData = _dialogData.ReplicaData[_counter];
            hidedAction += replicaData.OnHidedText;
            _counter++;
            
            showAction += replicaData.OnShowedText;
            showAction += _soundController.StopPlayingOtherSound;
            
            if (_counter < _dialogData.ReplicaData.Count)
            {
                hidedAction += ShowNextReplica;
            }
            else
            {
                hidedAction += () =>
                {
                    _dialogUI.Deactivate();
                    _isShowing = false;
                    _dialogData.OnHided?.Invoke();
                };
            }
            _soundController.PlayOtherSound(OtherSound.TextBlip, true, true);
            _dialogUI.ShowText(replicaData.Text, replicaData.Duration, 
                showAction, hidedAction);
        }
    }
}