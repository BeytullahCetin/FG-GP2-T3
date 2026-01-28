
using System;

namespace FG_GP2_T3
{
    public abstract class GameEventArgs : EventArgs { }

    public enum CellEventType
    {
        Click,
        Rotate,
        Place,
        Remove
    }

    public class CellActionEventArgs : GameEventArgs
    {
        public readonly HexCell Cell;
        public readonly CellEventType ActionType;

        public CellActionEventArgs(HexCell cell, CellEventType actionType)
        {
            Cell = cell;
            ActionType = actionType;
        }
    }

    //Add more events bellow
}