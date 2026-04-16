using System.Collections;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.Common;
using Trellcko.Gameplay.Monster;
using Trellcko.Gameplay.Player;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.Events
{
    public class Day5StealEvent : BaseEvent
    {
        [SerializeField] private Transform _monsterPoint;
        [SerializeField] private QuestInteractable _questInteractable;
        [SerializeField] private QuestItem _item;
        
        private ISoundController _soundController;
        private PlayerFacade _playerFacade;
        private MonsterFacade _monsterFacade;
        private IQuestSystem _questSystem;


        [Inject]
        private void Construct(PlayerFacade playerFacade, MonsterFacade monsterFacade, ISoundController soundController,
            IQuestSystem questSystem)
        {
            _soundController = soundController;
            _playerFacade = playerFacade;
            _monsterFacade = monsterFacade;
            _questSystem = questSystem;
        }

        protected override void OnBeforeNotifierInvoked()
        {
            _soundController.PlayShockMoment(true);
        }

        protected override void OnMakeVisible()
        {
            StartCoroutine(StealCorun());
        }

        private IEnumerator StealCorun()
        {
            yield return new WaitForSeconds(1.2f);
            _playerFacade.Interactable.SetItem(QuestItem.None);
            _soundController.PlayMonsterSound(MonsterSound.Laugh);
            _monsterFacade.Bringing.SetItem(_item);
            yield return new WaitForSeconds(3f);
            _monsterFacade.transform.position = _monsterPoint.position;
            _monsterFacade.gameObject.SetActive(true);
        }

        protected override void OnNotifierInvokeHandle()
        {
            _soundController.PlayMonsterSound(MonsterSound.Laugh);
            _monsterFacade.gameObject.SetActive(false);
            _questInteractable.TryInteract(out QuestItem item, _item);
        }
    }
}