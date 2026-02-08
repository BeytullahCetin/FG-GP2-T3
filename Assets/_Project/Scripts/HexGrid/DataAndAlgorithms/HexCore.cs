using UnityEngine;

namespace FG_GP2_T3
{
    public static class HexCore
    {     
        public static HexCoordinates[] CoreCoordinates = {
            new HexCoordinates(0, -4),
            new HexCoordinates(0, -3),
            new HexCoordinates(1, -4),
            new HexCoordinates(1, -5),
            new HexCoordinates(0, -5),
            new HexCoordinates(-1, -4),
            new HexCoordinates(-1, -3),
            };

        // old Poe location
        // public static HexCoordinates[] CoreCoordinates = {
        //     new HexCoordinates(0, 0),
        //     new HexCoordinates(-1, 0),
        //     new HexCoordinates(0, -1),
        //     new HexCoordinates(1, -1),
        //     new HexCoordinates(1, 0),
        //     new HexCoordinates(0, 1),
        //     new HexCoordinates(-1, 1),
        // };
    }
}