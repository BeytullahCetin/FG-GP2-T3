namespace FG_GP2_T3
{
    public enum UIEventType
    {
        Open,
        Close
    }

    public class OnUITowerEvent : GameEventArgs
    {
        public readonly TowerData TowerData;
        public readonly UIEventType EventType;

        public OnUITowerEvent(TowerData towerData, UIEventType eventType)
        {
            TowerData = towerData;
            EventType = eventType;
        }
    }

    public class OnMainMenuEvent : GameEventArgs
    {
        public readonly UIEventType EventType;

        public OnMainMenuEvent(UIEventType eventType)
        {
            EventType = eventType;
        }
    }

    public class OnPauseMenuEvent : GameEventArgs
    {
        public readonly UIEventType EventType;

        public OnPauseMenuEvent(UIEventType eventType)
        {
            EventType = eventType;
        }
    }
}