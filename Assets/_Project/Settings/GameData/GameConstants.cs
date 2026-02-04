using UnityEngine;

public class GameConstants
{
    public static class Layers
    {
        public static readonly int ENEMY = LayerMask.NameToLayer("Enemy");
        public static readonly int TILE = LayerMask.NameToLayer("Tile");
        public static readonly int TOWER = LayerMask.NameToLayer("Tower");
        public static readonly int PROJECTILE = LayerMask.NameToLayer("Projectile");
        public static readonly int POE = LayerMask.NameToLayer("POE");

        public static readonly int ENEMY_MASK = 1 << ENEMY;
        public static readonly int TILE_MASK = 1 << TILE;
        public static readonly int TOWER_MASK = 1 << TOWER;
        public static readonly int PROJECTILE_MASK = 1 << PROJECTILE;
        public static readonly int POE_MASK = 1 << POE;
    }

    public static class HexGrid
    {
        //public const int GRID_RADIUS = 5; Switched to serializefield in HexGrid
        public const float OUTER_RADIUS = 10f;
        public const float INNER_RADIUS = OUTER_RADIUS * 0.866025404f; // Mathf.Sqrt(3) / 2
    }
}