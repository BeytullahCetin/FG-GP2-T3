namespace FG_GP2_T3
{
    public enum CellEventType
    {
        Click,
        Rotate,
        Place,
        Remove,
        Cancel
    }

    public class OnCellEvent : GameEventArgs
    {
        public readonly HexCell Cell;
        public readonly CellEventType EventType;

        public OnCellEvent(HexCell cell, CellEventType eventType)
        {
            Cell = cell;
            EventType = eventType;
        }
    }
}