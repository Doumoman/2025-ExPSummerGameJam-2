using UnityEngine;

public enum HexOrientation { PointyTop, FlatTop }
public enum HexOffsetType
{
    OddR, EvenR,   // PointyTop(행 오프셋)
    OddQ, EvenQ    // FlatTop(열 오프셋)
}

public static class HexOffset
{
    // Offset(row/col 기반) → Axial(q,r)
    public static Vector2Int OffsetToAxial(Vector2Int off, HexOffsetType type)
    {
        int col = off.x, row = off.y;
        switch (type)
        {
            case HexOffsetType.OddR:  // row-based, odd rows shifted right
                return new Vector2Int(col - (row - (row & 1)) / 2, row);
            case HexOffsetType.EvenR: // row-based, even rows shifted right
                return new Vector2Int(col - (row + (row & 1)) / 2, row);
            case HexOffsetType.OddQ:  // col-based, odd cols shifted up
                return new Vector2Int(col, row - (col - (col & 1)) / 2);
            case HexOffsetType.EvenQ: // col-based, even cols shifted up
                return new Vector2Int(col, row - (col + (col & 1)) / 2);
            default:
                return Vector2Int.zero;
        }
    }

    // Axial(q,r) → Offset(row/col 기반)
    public static Vector2Int AxialToOffset(Vector2Int axial, HexOffsetType type)
    {
        int q = axial.x, r = axial.y;
        switch (type)
        {
            case HexOffsetType.OddR: return new Vector2Int(q + (r - (r & 1)) / 2, r);
            case HexOffsetType.EvenR: return new Vector2Int(q + (r + (r & 1)) / 2, r);
            case HexOffsetType.OddQ: return new Vector2Int(q, r + (q - (q & 1)) / 2);
            case HexOffsetType.EvenQ: return new Vector2Int(q, r + (q + (q & 1)) / 2);
            default: return Vector2Int.zero;
        }
    }
}