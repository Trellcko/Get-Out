using UnityEngine;

namespace Trellcko.Gameplay.MiniGame
{
    public class Mouth : MonoBehaviour
    {
        [SerializeField] private float _timeToChangeSprite;
        [SerializeField] private Texture2D _mouthOpen;
        [SerializeField] private Texture2D _mouthClose;
        [SerializeField] private Material _mouthMaterial;
        [SerializeField] private MeshRenderer _mouthMeshRenderer;
        
        public bool IsHiding => !_mouthMeshRenderer.enabled;
        
        private bool _isOpened;
        private bool _isRunning;
        
        public void Run()
        {
            if (_isRunning) return;
            
            _isRunning = true;
            _isOpened = false;
            _mouthMaterial.mainTexture = _mouthClose;
            
            InvokeRepeating(nameof(ChangeSprite), _timeToChangeSprite, _timeToChangeSprite);
        }

        public void Show()
        {
            _mouthMeshRenderer.enabled = true;
        }

        public void Hide()
        {
            _mouthMeshRenderer.enabled = false;
        }
        
        public void ChangeSprite()
        {
            _mouthMaterial.mainTexture = _isOpened? _mouthClose : _mouthOpen;
            
            _isOpened = !_isOpened;
        }

        public void Stop()
        {
            if(!_isRunning) 
                return;
            _isRunning = false;
            CancelInvoke(nameof(ChangeSprite));
            _mouthMaterial.mainTexture = _mouthClose;
            _isOpened = false;
        }
    }
}