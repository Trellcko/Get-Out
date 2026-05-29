namespace Trellcko.Gameplay.House
{
    public interface ILightController
    {
        void SetMode(LightMode mode);
        public void BlinkInRoom(Room room);
    }
}