using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Trellcko.Gameplay.MiniGame
{
   public class Cookie : MonoBehaviour
   {
      [field: SerializeField] public bool IsGood { get; private set; }
      
      [SerializeField] private float _speed;
      [SerializeField] private Rigidbody _rigidbody;

      private void Start()
      {
         transform.localEulerAngles = new(0, 0, Random.Range(0f, 360f));
         _rigidbody.linearVelocity = new(0, -_speed, 0);
      }
   }
}