using UnityEngine;

namespace FG_GP2_T3
{
    public class HexMetrics
    {
        private static Vector3[] corners = {
            new Vector3(-GameConstants.HexGrid.OUTER_RADIUS, 0f, 0f),
            new Vector3(-GameConstants.HexGrid.OUTER_RADIUS * 0.5f, 0f, GameConstants.HexGrid.INNER_RADIUS),
            new Vector3(GameConstants.HexGrid.OUTER_RADIUS * 0.5f, 0f, GameConstants.HexGrid.INNER_RADIUS),
            new Vector3(GameConstants.HexGrid.OUTER_RADIUS, 0f, 0f),
            new Vector3(GameConstants.HexGrid.OUTER_RADIUS * 0.5f, 0f, -GameConstants.HexGrid.INNER_RADIUS),
            new Vector3(-GameConstants.HexGrid.OUTER_RADIUS * 0.5f, 0f, -GameConstants.HexGrid.INNER_RADIUS)
        };

        public static Vector3 GetFirstCorner(HexDirection direction) => corners[(int)direction];
        public static Vector3 GetSecondCorner(HexDirection direction) => corners[((int)direction + 1) % 6];
        public static Vector3 GetEdgeCenter(HexDirection direction) => (GetFirstCorner(direction) + GetSecondCorner(direction)) / 2f;
        public static float GetAngleDegrees(HexDirection direction) => (int)direction.Previous() * 60f;
    }
}
