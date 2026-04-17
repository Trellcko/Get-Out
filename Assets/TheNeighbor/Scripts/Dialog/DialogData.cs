using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Trellcko.Dialog
{
    [Serializable]
    public class DialogData
    {
        public List<ReplicaData> ReplicaData;
        public UnityAction OnShowed;
        public UnityAction OnHided;
    }
}