namespace FG_GP2_T3
{    
    public enum EnemyEventType
    {
        Spawn,
        Damaged,
        Death,
    }

    public class OnEnemyActionEvent : GameEventArgs
    {
        public readonly Enemy Enemy;
        public readonly EnemyEventType EventType;

        public OnEnemyActionEvent(Enemy enemy, EnemyEventType eventType)
        {
            Enemy = enemy;
            EventType = eventType;
        }
    }
}