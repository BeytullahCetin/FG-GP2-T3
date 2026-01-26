using UnityEngine;

public enum HexDirection { NW, N, NE, SE, S, SW, }

public static class HexDirectionExtensions
{
    public static HexDirection Opposite(this HexDirection direction) => (HexDirection)(((int)direction + 3) % 6);
	public static HexDirection Previous(this HexDirection direction) => direction == HexDirection.NW ? HexDirection.SW : (direction - 1);
	public static HexDirection Next(this HexDirection direction) => direction == HexDirection.SW ? HexDirection.NW : (direction + 1);
}