using System;
using System.Collections.Generic;
using Trellcko.Analytics;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.House;
using UnityEngine;
using Zenject;

namespace Trellcko.Gameplay.QuestLogic
{
    [Serializable]
    public class QuestsDayList
    {
        private AnalyticsService _analyticsService;
        private int _currentDay;
        [field: SerializeField] public Ambience Ambience { get; set; } = Ambience.InDayTime;
        [field: SerializeField] public LightMode LightMode { get; private set; } = LightMode.Standard;
        [field: SerializeField] public float BlinkChance { get; private set; } = 0f;
        [field: SerializeField] public List<Quest> Quests { get; private set; }
        public Quest CurrentQuest => Quests[QuestIndex];

        public int QuestIndex { get; private set; }

        public event Action AllQuestsCompleted;
        public event Action QuestActivated;
        public event Action BeforeQuestActivated;
        
        public void Init(AnalyticsService analyticsService, int currentDay, int questIndex = 0)
        {
            _analyticsService = analyticsService;
            _currentDay = currentDay;
            QuestIndex = questIndex;
            foreach (Quest quest in Quests)
            {
                quest.Completed += OnQuestCompleted;
            }
            Debug.Log("Activated quest " +QuestIndex );
            CurrentQuest.Activate();
            _analyticsService.SendQuestStartedEvent(_currentDay, QuestIndex);
            QuestActivated?.Invoke();
        }

        private void OnQuestCompleted()
        {
            CurrentQuest.Completed -= OnQuestCompleted;
            BeforeQuestActivated?.Invoke();
            _analyticsService.SendQuestCompleted(_currentDay, QuestIndex);
            if (QuestIndex == Quests.Count - 1)
            {
                AllQuestsCompleted?.Invoke();
                Debug.Log("All quests completed");
                return;
            }
            QuestIndex++;
            CurrentQuest.Activate();
            QuestActivated?.Invoke();
        }
    }
}