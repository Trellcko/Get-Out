using System.Collections.Generic;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.House
{
    public class LightController : MonoBehaviour, ILightController
    {
        [SerializeField] private List<Light> _lights;

        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _redMode;
        
        private IQuestSystem _questSystem;

        public void SetMode(LightMode mode)
        {
            SetColor(GetColor(mode));
        }

        private Color GetColor(LightMode mode) => mode == LightMode.Red ? _redMode : _normalColor;

        private void SetColor(Color color)
        {
            foreach (Light light in _lights)
            {
                light.color = color;
            }
        }
    }
}