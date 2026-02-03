using UnityEngine;

public class GameConstants
{
    public static class HexGrid
    {
        //public const int GRID_RADIUS = 5; Switched to serializefield in HexGrid
        public const float OUTER_RADIUS = 10f;
        public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f; // Mathf.Sqrt(3) / 2
    }
}