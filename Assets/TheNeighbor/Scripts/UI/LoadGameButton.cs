using System;
using System.Collections;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Trellcko.UI
{
    [RequireComponent(typeof(Button))]
    public class LoadGameButton : MonoBehaviour
    {
        [SerializeField] private Trellcko.Gameplay.Interactable.Door _door;
        [SerializeField] private GameObject _settings;
        [SerializeField] private GameObject _instructions;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(LoadGameScene);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(LoadGameScene);
        }

        private void LoadGameScene()
        {
            _door.StopKnockingSound();
            _door.InteractionFinished += OnInteractionFinished;
            _door.TryInteract(out _, QuestItem.None);
        }

        private void OnInteractionFinished()
        {
            _door.InteractionFinished -= OnInteractionFinished;
            _settings.gameObject.SetActive(false);
            _instructions.gameObject.SetActive(false);
            StartCoroutine(LoadSceneWithDelay());
        }

        private IEnumerator LoadSceneWithDelay()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(1);
        }
    }
}