using Trellcko.Gameplay.Interactable;
using UnityEngine;

namespace Trellcko.Gameplay.MiniGame
{
    public class WateringMiniGameParamsHolder : MiniGamesParamsHolder
    {
        [field: SerializeField] public Material FlowerMaterial { get; private set; }
    }
}