using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.House;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Trellcko.Gameplay
{
    public class DayAtmosphereController : MonoBehaviour
    {
        [SerializeField] private Vector2 _secondsAfterStartQuestToShowBlink = new Vector2(1f, 5f);
        
        private IQuestSystem _questSystem;
        private ISoundController _soundController;
        private ILightController _lightController;
        
        private Room _corridorRoom;
        private Room _kitchenRoom;
        private Room _bedRoom;

        private List<Room> _rooms;
        
        private bool _isBlinking;
        
        [Inject]
        private void Construct(ISoundController soundController, 
            ILightController lightController, IQuestSystem questSystem,
            
            [Inject(Id = RoomType.Bedroom)] Room bedRoom,
            [Inject(Id = RoomType.Kitchen)] Room kitchenRoom,
            [Inject(Id = RoomType.Corridor)] Room corridorRoom)
        {
            _questSystem = questSystem;
            _corridorRoom = corridorRoom;
            _kitchenRoom = kitchenRoom;
            _bedRoom = bedRoom;
            _soundController = soundController;
            _lightController = lightController;
            _rooms = new()
            {
                _bedRoom, _kitchenRoom, _corridorRoom
            };
        }

        private void Awake()
        {
            _questSystem.DayStarted += OnDayStarted;
        }

        private void OnDestroy()
        {
            _questSystem.DayStarted -= OnDayStarted;
        }

        private void OnDayStarted()
        {
            _soundController.PlayAmbience(_questSystem.CurrentDayList.Ambience);
            _lightController.SetMode(_questSystem.CurrentDayList.LightMode);
            _questSystem.CurrentDayList.QuestActivated += OnQuestActivated;
            _questSystem.CurrentDayList.AllQuestsCompleted += OnCompleted;
        }

        private void OnCompleted()
        {
            _questSystem.CurrentDayList.QuestActivated -= OnQuestActivated;
            _questSystem.CurrentDayList.AllQuestsCompleted -= OnCompleted;
        }

        private void OnQuestActivated()
        {
            if (_isBlinking) return;
            
            float chance = Random.Range(0, 1f);

            if (_questSystem.CurrentDayList.BlinkChance > chance)
            {
                StartCoroutine(StartBlinkingCorun());
            }
        }

        private IEnumerator StartBlinkingCorun()
        {
            _isBlinking = true;
            yield return new WaitForSeconds(Random.Range(_secondsAfterStartQuestToShowBlink.x,
                _secondsAfterStartQuestToShowBlink.y));
            _lightController.BlinkInRoom(FindRoom(_questSystem.CurrentDayList.CurrentQuest.QuestInteractable));
            _isBlinking = false;
        }

        private Room FindRoom(QuestInteractable currentQuestQuestInteractable) => _rooms.First(x => x.HasQuestInteractable(currentQuestQuestInteractable));
    }
}