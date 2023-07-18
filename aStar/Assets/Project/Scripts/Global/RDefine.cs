using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RDefine
{
    public const string TERRAIN_PREF_OCEAN = "Tile_water"; //Terrain_Ocean
    public const string TERRAIN_PREF_PLAIN = "Tile_Plain"; //Terrain_Plain

    public const string OBSTACLE_PREF_PLAIN_CASTLE = "Obstacle_PlainCastle";

    public enum TileStatusColor
    {
        DEFAULT, SELECTED, SEARCH, INACTIVE
    }
}
