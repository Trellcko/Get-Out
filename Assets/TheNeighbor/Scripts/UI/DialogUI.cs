using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Trellcko.UI
{
    public class DialogUI : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private TextMeshProUGUI _text;

        private const float Delay = 0.03f;

        private Coroutine _typeCorun;

        private Action _onShowed;
        private Action _onHided;

        public void Activate()
        {
            _content.SetActive(true);
        }

        public void Deactivate()
        {
            _content.SetActive(false);
        }

        public void ShowText(string text, float showDuration = -1, Action showed = null, Action hided = null)
        {
            if (_typeCorun != null)
                StopCoroutine(_typeCorun);

            _onShowed = showed;
            _onHided = hided;
            _typeCorun = StartCoroutine(TypeText(text, showDuration));
        }

        public void HideText()
        {
            _text.SetText("");
            _onHided?.Invoke();
        }

        private IEnumerator TypeText(string text, float showDuration)
        {
            _text.SetText("");

            foreach (char character in text)
            {
                _text.SetText(_text.text + character);
                yield return new WaitForSeconds(Delay);
            }

            _onShowed?.Invoke();

            if (showDuration > 0)
            {
                yield return new WaitForSeconds(showDuration);
                HideText();
            }
        }
    }
}
