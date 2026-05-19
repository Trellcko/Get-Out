using UnityEngine;

namespace Trellcko.Gameplay.House
{
   public class Wall : MonoBehaviour
   {
      [field: SerializeField] public WallType WallType { get; private set; }
      
   }

   public enum WallType
   {
      Normal,
      WithDoor,
   }
}