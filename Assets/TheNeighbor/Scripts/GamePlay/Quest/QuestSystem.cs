using System;
using System.Collections.Generic;
using Trellcko.Analytics;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.House;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.QuestLogic
{
    public class QuestSystem : MonoBehaviour, IQuestSystem
    {
        [SerializeField] private List<QuestsDayList> _questDays;
        [SerializeField] private int _startDay;
        [SerializeField] private int _startQuestIndex;
        private AnalyticsService _analyticsService;
        public QuestsDayList CurrentDayList => _questDays[Day];
        public int Day { get; private set; }

        public bool AreAllQuestsCompleted => Day == _questDays.Count - 1;
        public event Action DayCompleted;
        public event Action DayStarted;
        public event Action AllDaysCompleted;

        [Inject]
        private void Construct(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        
        private void Awake()
        {
#if UNITY_EDITOR
            Day = _startDay;
#endif
            _questDays[Day].AllQuestsCompleted += OnAllQuestsInDayCompleted;
            
        }

        private void Start()
        {

            StartCurrentDay(_startQuestIndex);
        }

        public void StartNextDay()
        {
            Day++;
            _questDays[Day].AllQuestsCompleted += OnAllQuestsInDayCompleted;
            StartCurrentDay();
        }

        private void StartCurrentDay(int fromQuestIndex = 0)
        {
            _questDays[Day].Init(_analyticsService, Day, fromQuestIndex);
            _analyticsService.SendDayStartedEvent(Day);
            DayStarted?.Invoke();
        }

        private void OnAllQuestsInDayCompleted()
        {
            _questDays[Day].AllQuestsCompleted -= OnAllQuestsInDayCompleted;
            _analyticsService.SendDayFinishedEvent(Day);
            DayCompleted?.Invoke();
            if (AreAllQuestsCompleted)
            {
                AllDaysCompleted?.Invoke();
            }
        }
    }
}