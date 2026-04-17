using System;
using System.Collections.Generic;
using System.Linq;
using Trellcko.Gameplay.Events;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.QuestLogic
{
    public class DayEventsHandler : MonoBehaviour
    {
        [SerializeField] private List<DayEventsList> _dayEvents;
        
        private IQuestSystem _questSystem;

        [Inject]
        private void Construct(IQuestSystem questSystem)
        {
            _questSystem = questSystem;
        }

        private void OnEnable()
        {
            _questSystem.CurrentDayList.QuestActivated += OnQuestActivated;
            _questSystem.DayStarted += OnDayStarted;
            _questSystem.DayCompleted += OnDayCompleted;
        }

        private void OnDisable()
        {
            _questSystem.CurrentDayList.QuestActivated -= OnQuestActivated;
            _questSystem.DayStarted -= OnDayStarted;
        }

        private void OnDayStarted()
        {
            _questSystem.CurrentDayList.QuestActivated += OnQuestActivated;
            foreach (DayTriggerData dayTriggerData in _dayEvents[_questSystem.Day]._dayTriggersData)
            {
                dayTriggerData._baseEvent.Init(dayTriggerData._notifier);
            }
        }

        private void OnDayCompleted()
        {
            _questSystem.CurrentDayList.QuestActivated -= OnQuestActivated;
        }

        private void OnQuestActivated()
        {
            if (_dayEvents.Count > _questSystem.Day)
            {
                List<DayTriggerData> data = _dayEvents[_questSystem.Day]._dayTriggersData.Where(x => x.QuestIndex == _questSystem.CurrentDayList.QuestIndex).ToList();
                foreach (DayTriggerData dayTriggerData in data)
                {
                    dayTriggerData._notifier?.StartWatching();
                    dayTriggerData?._baseEvent.MakeVisible();   
                }
            }
        }
    }
    [Serializable]

    public class DayEventsList
    {
        public List<DayTriggerData> _dayTriggersData;
    }
    
    [Serializable]
    public class DayTriggerData
    {
        public int QuestIndex;
        public BaseEvent _baseEvent;
        public Notifier _notifier;
    }
}