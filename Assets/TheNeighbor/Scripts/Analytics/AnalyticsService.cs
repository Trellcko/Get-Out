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
            Debug.Log("SendDayStartedEvent");
            
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Start,
                $"Day_{day}"
            );

        }

        public void SendDayFinishedEvent(int day)
        {
            
            Debug.Log("SendDayFinishedEvent");
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Complete,
                $"Day_{day}"
            );
        }

        public void SendQuestStartedEvent(int day, int quest)
        {
            Debug.Log("SendQuestStartedEvent");
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Start,
                $"Day_{day}",
                $"Quest_{quest}"
            );
        }

        public void SendQuestCompleted(int day, int quest)
        {
            Debug.Log("SendQuestFinishedEvent");
            GameAnalytics.NewProgressionEvent(
                GAProgressionStatus.Complete,
                $"Day_{day}",
                $"Quest_{quest}"
            );
        }
    }
}