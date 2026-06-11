using System;
using UnityEngine;
using UnityEngine.UI;

namespace Trellcko.UI
{
    [RequireComponent(typeof(Button))]
    public class ChangeUIWindowButton : MonoBehaviour
    {
        [SerializeField] private GameObject _close;
        [SerializeField] private GameObject _open;

        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(ChangeState);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ChangeState);
        }

        private void ChangeState()
        {
            _close?.gameObject.SetActive(false);
            _open?.gameObject.SetActive(true);
        }
    }
}