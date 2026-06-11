using GameAnalyticsSDK;
using UnityEngine;

namespace Trellcko.Analytics
{
    public class AnalyticsService
    {
        public AnalyticsService()
        {
            GameAnalytics.Initialize();
        }
        
        public void SendDayStartedEvent(int day)
        {
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Start,
                $"Day_{day}"
            );

        }

        public void SendDayFinishedEvent(int day)
        {
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Complete,
                $"Day_{day}"
            );
        }

        public void SendQuestStartedEvent(int day, int quest)
        {
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Start,
                $"Day_{day}",
                $"Quest_{quest}"
            );
        }

        public void SendQuestCompleted(int day, int quest)
        {
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Complete,
                $"Day_{day}",
                $"Quest_{quest}"
            );
        }
    }
}