using System;
using TheNeighbor.Scripts.Constants;
using Trellcko.Constants;
using Trellcko.Core.Audio;
using Trellcko.Core.Input;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class ClothesDraggable : MonoBehaviour
    {
        [SerializeField] private Material _material;
        
        private Camera _camera;
        private IInputHandler _inputHandler;
        private ISoundController _soundController;
        public event Action<ClothesDraggable> Putted;

        [Inject]
        private void Construct(IInputHandler handler, ISoundController soundController)
        {
            _soundController = soundController;
            _inputHandler = handler;
        }
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        public void UpdateSprite(Sprite sprite)
        {
            _material.SetTexture(ShaderProperties.MainTex, sprite.texture);

            float width = sprite.rect.width / sprite.pixelsPerUnit;
            float height = sprite.rect.height / sprite.pixelsPerUnit;

            transform.localScale = new(width, height, 1f);

            _material.SetTextureScale(ShaderProperties.MainTex, Vector2.one);
        }
        
        private void Update()
        {
            Vector2 mouse = _inputHandler.GetMousePosition();
            float depth = transform.position.z - _camera.transform.position.z;
            Vector3 screen = new(mouse.x, mouse.y, depth);
            Vector3 world = _camera.ScreenToWorldPoint(screen);
            transform.position = new(world.x, world.y, transform.position.z);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ClosetTag _))
            {
                _soundController.PlayOtherSound(OtherSound.Put);
                Putted?.Invoke(this);
                Destroy(gameObject);
            }
        }
    }
}