using System;
using Trellcko.Core.Audio;
using UnityEngine;

namespace Trellcko.Gameplay.MiniGame
{
    [Serializable]
    public class WindowMiniGameData
    {
        public float power;
        public float fallDownSpeed;
        public Material Normal;
        public Material Happy;
        public PlayerSound Laugh = PlayerSound.Chuckle;
    }
}