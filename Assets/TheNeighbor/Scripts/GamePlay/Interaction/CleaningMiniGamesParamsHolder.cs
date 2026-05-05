using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class CleaningMiniGamesParamsHolder : MiniGamesParamsHolder
    {
        [field: SerializeField] public Texture2D Spot { get; private set; }
        [field: SerializeField] public Color SpotColor { get; private set; } = Color.white;
        [field: SerializeField] public Material Background { get; private set; }
        [field: SerializeField] public float Rotation { get; private set; }
    }
}