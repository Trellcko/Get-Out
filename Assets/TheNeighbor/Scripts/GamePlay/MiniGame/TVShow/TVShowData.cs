using System;
using System.Collections.Generic;
using Trellcko.Dialog;
using UnityEngine;

namespace Trellcko.Gameplay.MiniGame
{
    [Serializable]
    public class TVShowData
    {
        public DialogData DialogData;
        public List<VisualData> VisualData;
    }

    [Serializable]
    public class VisualData
    {
        public Sprite Sprite;
        public Color Color = Color.white;
    }
}