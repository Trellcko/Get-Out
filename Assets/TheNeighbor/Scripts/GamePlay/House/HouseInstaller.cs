using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.House
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private LightController _lightController;

        public override void InstallBindings() => 
            Container.Bind<ILightController>().FromInstance(_lightController).AsSingle();
    }
}