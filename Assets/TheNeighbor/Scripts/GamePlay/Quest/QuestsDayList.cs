using System;
using System.Collections.Generic;
using Trellcko.Core.Audio;
using Trellcko.Gameplay.House;
using UnityEngine;

namespace Trellcko.Gameplay.QuestLogic
{
    [Serializable]
    public class QuestsDayList
    {
        [field: SerializeField] public Ambience Ambience { get; set; } = Ambience.InDayTime;
        [field: SerializeField] public LightMode LightMode { get; private set; } = LightMode.Standard;
        [field: SerializeField] public float BlinkChance { get; private set; } = 0f;
        [field: SerializeField] public List<Quest> Quests { get; private set; }
        public Quest CurrentQuest => Quests[QuestIndex];

        public int QuestIndex { get; private set; }

        public event Action AllQuestsCompleted;
        public event Action QuestActivated;
        public event Action BeforeQuestActivated;

        public void Init(int questIndex = 0)
        {
            QuestIndex = questIndex;
            foreach (Quest quest in Quests)
            {
                quest.Completed += OnQuestCompleted;
            }
            Debug.Log("Activated quest " +QuestIndex );
            CurrentQuest.Activate();
            QuestActivated?.Invoke();
        }

        private void OnQuestCompleted()
        {
            CurrentQuest.Completed -= OnQuestCompleted;
            BeforeQuestActivated?.Invoke();
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