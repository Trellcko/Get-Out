using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Trellcko.UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private TextMeshProUGUI _text;

        private Coroutine _typeCorun;

        private Action _onShowed;

        public void Activate()
        {
            _content.SetActive(true);
        }

        public void Deactivate()
        {
            _content.SetActive(false);
        }

        public void ShowText(string text, float delayPerCharacter, float delay = 0, Action showed = null)
        {
            if (_typeCorun != null)
                StopCoroutine(_typeCorun);

            _onShowed = showed;
            _typeCorun = StartCoroutine(TypeText(text, delayPerCharacter, delay));
        }

        private IEnumerator TypeText(string text, float delayPerCharacter, float delay)
        {
            _text.SetText("");
            
            yield return new WaitForSeconds(delay);
            
            foreach (char character in text)
            {
                _text.SetText(_text.text + character);
                yield return new WaitForSeconds(delayPerCharacter);
            }

            _onShowed?.Invoke();
        }
    }
}
