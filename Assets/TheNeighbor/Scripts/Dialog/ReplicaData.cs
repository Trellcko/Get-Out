using System;
using UnityEngine;
using UnityEngine.Events;

namespace Trellcko.Dialog
{
    [Serializable]
    public class ReplicaData
    {
        public string Text;
        public AudioClip Audio;
        public float Delay;
        public Action<int> OnStartedToShow;
        public Action<int> OnShowedText;
    }
}