using System;
using UnityEngine.Events;

namespace Trellcko.Dialog
{
    [Serializable]
    public class ReplicaData
    {
        public string Text;
        public float Duration;
        public Action OnShowedText;
        public Action OnHidedText;
    }
}