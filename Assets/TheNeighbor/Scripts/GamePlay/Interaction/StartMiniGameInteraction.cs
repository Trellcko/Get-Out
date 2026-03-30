using Trellcko.Gameplay.MiniGame;
using Trellcko.Gameplay.Player;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Interactable
{
    [RequireComponent(typeof(IInteractable))]
    public class StartMiniGameInteraction : MonoBehaviour
    {
        [SerializeField] private MiniGameType _minigameType;
        [SerializeField] private MiniGamesParamsHolder minigamesParamsHolder;
        private QuestInteractable _interactable;
        private MiniGamesController _minigameController;

        [Inject]
        private void Construct(MiniGamesController controller, PlayerFacade playerFacade)
        {
            _minigameController = controller;
        }
        
        private void Awake()
        {
            _interactable = GetComponent<QuestInteractable>();
            _interactable.SetIsMiniGameInteraction();
        }

        private void OnEnable()
        {
            _interactable.InteractionStarted += OnInteractionStarted;
        }

        private void OnDisable()
        {
            _interactable.InteractionStarted -= OnInteractionStarted;
        }

        private void OnInteractionStarted()
        {
            _minigameController.StartMiniGame(_minigameType, _interactable.FinishInteraction, minigamesParamsHolder);
        }
    }
}