using UnityEngine;
using System;

[Serializable]
public struct HexCoordinates
{
    [SerializeField]
    private int q, r;

    public int Q => q;
    public int R => r;
    public int S => -q - r;

    public int X => Q;
    public int Y => R;
    public int Z => S;

    public HexCoordinates(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    public static HexCoordinates FromWorldPosition(Vector3 position)
    {
        float qFactor = (2f / 3f * position.x) / GameConstants.HexGrid.OUTER_RADIUS;
        float rFactor = (-1f / 3f * position.x + Mathf.Sqrt(3f) / 3f * -position.z) / GameConstants.HexGrid.OUTER_RADIUS;

        return RoundToHex(qFactor, rFactor);
    }

    public static Vector3 ToWorldPosition(HexCoordinates coordinates)
    {
        float xPosition = coordinates.Q * (GameConstants.HexGrid.OUTER_RADIUS * 1.5f);
        float zPosition = -(coordinates.R + coordinates.Q * 0.5f) * (GameConstants.HexGrid.OUTER_RADIUS * Mathf.Sqrt(3f));
        
        return new Vector3(xPosition, 0f, zPosition);
    }

    private static HexCoordinates RoundToHex(float qFloat, float rFloat)
    {
        float sFloat = -qFloat - rFloat;

        int iQ = Mathf.RoundToInt(qFloat);
        int iR = Mathf.RoundToInt(rFloat);
        int iS = Mathf.RoundToInt(sFloat);

        float deltaQ = Mathf.Abs(qFloat - iQ);
        float deltaR = Mathf.Abs(rFloat - iR);
        float deltaS = Mathf.Abs(sFloat - iS);

        if (deltaQ > deltaR && deltaQ > deltaS)
        {
            iQ = -iR - iS;
        }
        else if (deltaR > deltaS)
        {
            iR = -iQ - iS;
        }

        return new HexCoordinates(iQ, iR);
    }

    public override string ToString() => $"({Q}, {R}, {S})";
    public string ToStringOnSeparateLines() => $"{Q}\n{R}\n{S}";
}