namespace FG_GP2_T3
{
    public enum TowerEventType
    {
        Build,
        Fuse,
        Select,
        Deselect,
    }

    public class OnTowerEvent : GameEventArgs
    {
        public readonly TowerBase Tower;
        public readonly TowerEventType EventType;

        public OnTowerEvent(TowerBase tower, TowerEventType eventType)
        {
            Tower = tower;
            EventType = eventType;
        }
    }

    public enum TowerActionType
    {
        Attack,
    }

    public class OnTowerActionEvent : GameEventArgs
    {
        public readonly TowerBase Tower;
        public readonly TowerActionType ActionType;

        public OnTowerActionEvent(TowerBase tower, TowerActionType actionType)
        {
            Tower = tower;
            ActionType = actionType;
        }
    }
}