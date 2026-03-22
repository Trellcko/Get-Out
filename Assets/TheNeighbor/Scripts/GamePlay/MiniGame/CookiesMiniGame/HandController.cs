using System;
using System.Collections;
using Trellcko.Constants;
using Trellcko.Core.Input;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Zenject;

namespace Trellcko.Gameplay.MiniGame
{
    public class HandController : MonoBehaviour
    {
        [SerializeField] private Volume _volume;
        
        [SerializeField] private Vector3 _handStartPosition;
        [SerializeField] private float _sensitivity = 40;
        [SerializeField] private Vector2 _zLocalBounds;
        [SerializeField] private Texture _hand;
        [SerializeField] private Texture _handClose;
        [SerializeField] private float _handChangeSpriteTime = 1f;
        [SerializeField] private Material _handMaterial;
        
        public event Action<bool> CookieGot;
        
        private IInputHandler _inputHandler;
        private MiniGameBadEffect _miniGameBadEffect;

        [Inject]
        private void Construct(IInputHandler inputHandler, MiniGameBadEffect miniGameBadEffect)
        {
            _miniGameBadEffect = miniGameBadEffect;
            _inputHandler = inputHandler;
        }


        private void Update()
        {
            Vector3 mouseDelta = _inputHandler.GetMouseDelta();
            mouseDelta *= _sensitivity * Time.deltaTime * 1;
            mouseDelta.y = 0;
            
            Vector3 newPosition = transform.position + mouseDelta;
            newPosition = transform.parent.InverseTransformPoint(newPosition);
            newPosition.z  = Mathf.Clamp(newPosition.z, _zLocalBounds.x, _zLocalBounds.y);
            
            transform.localPosition = newPosition;
        }

        public void Reset()
        {
            transform.localPosition = _handStartPosition;
            _handMaterial.SetTexture(ShaderProperties.MainTex, _hand);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Cookie cookie))
            {
                StopAllCoroutines();
                StartCoroutine(ChangeHandCorun());
                
                if(!cookie.IsGood)
                    _miniGameBadEffect.PlayCorpseEffect(_volume);
                
                CookieGot?.Invoke(cookie.IsGood);
                Destroy(cookie.gameObject);
            }
        }

        private IEnumerator ChangeHandCorun()
        {
            _handMaterial.SetTexture(ShaderProperties.MainTex, _handClose);
            yield return new WaitForSeconds(_handChangeSpriteTime);
            _handMaterial.SetTexture(ShaderProperties.MainTex, _hand);
        }
    }
}