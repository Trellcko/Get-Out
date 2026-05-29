using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.House
{
    public class HouseInstaller : MonoInstaller
    {
        [SerializeField] private LightController _lightController;
        [SerializeField] private Room _badRoom;
        [SerializeField] private Room _corridorRoom;
        [SerializeField] private Room _kitchenRoom;

        public override void InstallBindings()
        {
            Container.Bind<ILightController>().FromInstance(_lightController).AsSingle();
            
            Container.Bind<Room>().WithId(RoomType.Corridor).FromInstance(_corridorRoom).AsCached();
            Container.Bind<Room>().WithId(RoomType.Kitchen).FromInstance(_kitchenRoom).AsCached();
            Container.Bind<Room>().WithId(RoomType.Bedroom).FromInstance(_badRoom).AsCached();
        }
    }
}