using UnityEngine;
using Zenject;

namespace Trellcko.Analytics
{
    public class AnalyticsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AnalyticsService>().AsSingle();
        }
    }
}