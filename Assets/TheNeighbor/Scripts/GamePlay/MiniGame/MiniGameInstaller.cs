using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class MiniGameInstaller : MonoInstaller
    {
        [SerializeField] private MiniGamesController _miniGamesController;
        [SerializeField] private MiniGameBadEffect _miniGameBadEffect;

        public override void InstallBindings()
        {
            Container.Bind<MiniGamesController>().FromInstance(_miniGamesController);
            Container.Bind<MiniGameBadEffect>().FromInstance(_miniGameBadEffect);
        }
    }
}