using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Trellcko.Gameplay.House
{
    public class LightController : MonoBehaviour, ILightController
    {
        [SerializeField] private List<Light> _gamesLights;
        
        [Header("Light Modes Settings")]
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _redMode;

        [Header("Blink Settings")] 
        [SerializeField] private int _blinkCount = 1;
        [SerializeField] private float _blinkDuration = 0.2f;
        
        private Room _bedRoom;
        private Room _kitchenRoom;
        private Room _corridorRoom;

        [Inject]
        private void Construct(
            [Inject(Id = RoomType.Bedroom)] Room bedRoom,
            [Inject(Id = RoomType.Kitchen)] Room kitchenRoom,
            [Inject(Id = RoomType.Corridor)] Room corridorRoom)
        {
            _corridorRoom = corridorRoom;
            _kitchenRoom = kitchenRoom;
            _bedRoom = bedRoom;
        }

        public void SetMode(LightMode mode)
        {
            SetColor(GetColor(mode));
        }

        private Color GetColor(LightMode mode) => mode == LightMode.Red ? _redMode : _normalColor;

        private void SetColor(Color color)
        {
            foreach (Light light in _gamesLights)
            {
                light.color = color;
            }
            foreach (Light light in _bedRoom.Lights)
            {
                light.color = color;
            }
            foreach (Light light in _kitchenRoom.Lights)
            {
                light.color = color;
            }
            foreach (Light light in _corridorRoom.Lights)
            {
                light.color = color;
            }
        }

        public void BlinkInRoom(Room room)
        {
            Light light = room.Lights[Random.Range(0, room.Lights.Count)];
            Blink(light);
        }

        private void Blink(Light light)
        {
            StartCoroutine(BlinkCoroutine(light, _blinkCount, _blinkDuration));
        }

        private IEnumerator BlinkCoroutine(Light light, int count, float duration)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(duration);
                light.enabled = false;
                yield return new WaitForSeconds(duration);
                light.enabled = true;
            }
        }
    }

    public enum RoomType
    {
        Corridor,
        Bedroom,
        Kitchen,
    }
}