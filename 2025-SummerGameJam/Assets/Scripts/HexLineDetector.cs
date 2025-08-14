using System.Collections.Generic;
using UnityEngine;

public class HexAllLines
{
    public List<List<Vector2Int>> GetAllLines()
    {
        var allLines = new List<List<Vector2Int>>();

        // 1. 가로줄
        allLines.AddRange(new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new(8,2), new(8,3), new(8,4), new(8,5), new(8,6) },
            new List<Vector2Int> { new(7,1), new(7,2), new(7,3), new(7,4), new(7,5), new(7,6) },
            new List<Vector2Int> { new(6,1), new(6,2), new(6,3), new(6,4), new(6,5), new(6,6), new(6,7) },
            new List<Vector2Int> { new(5,0), new(5,1), new(5,2), new(5,3), new(5,4), new(5,5), new(5,6), new(5,7) },
            new List<Vector2Int> { new(4,0), new(4,1), new(4,2), new(4,3), new(4,4), new(4,5), new(4,6), new(4,7), new(4,8) },
            new List<Vector2Int> { new(3,0), new(3,1), new(3,2), new(3,3), new(3,4), new(3,5), new(3,6), new(3,7) },
            new List<Vector2Int> { new(2,1), new(2,2), new(2,3), new(2,4), new(2,5), new(2,6), new(2,7) },
            new List<Vector2Int> { new(1,1), new(1,2), new(1,3), new(1,4), new(1,5), new(1,6) },
            new List<Vector2Int> { new(0,2), new(0,3), new(0,4), new(0,5), new(0,6) }
        });

        // 2. "/" 대각선 (↗)
        allLines.AddRange(new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new(4,0), new(3,0), new(2,1), new(1,1), new(0,2) },
            new List<Vector2Int> { new(5,0), new(4,1), new(3,1), new(2,2), new(1,2), new(0,3) },
            new List<Vector2Int> { new(6,1), new(5,1), new(4,2), new(3,2), new(2,3), new(1,3), new(0,4) },
            new List<Vector2Int> { new(7,1), new(6,2), new(5,2), new(4,3), new(3,3), new(2,4), new(1,4), new(0,5) },
            new List<Vector2Int> { new(8,2), new(7,2), new(6,3), new(5,3), new(4,4), new(3,4), new(2,5), new(1,5), new(0,6) },
            new List<Vector2Int> { new(8,3), new(7,3), new(6,4), new(5,4), new(4,5), new(3,5), new(2,6), new(1,6) },
            new List<Vector2Int> { new(8,4), new(7,4), new(6,5), new(5,5), new(4,6), new(3,6), new(2,7) },
            new List<Vector2Int> { new(8,5), new(7,5), new(6,6), new(5,6), new(4,7), new(3,7) },
            new List<Vector2Int> { new(8,6), new(7,6), new(6,7), new(5,7), new(4,8) }
        });

        // 3. "\" 대각선 (↖)
        allLines.AddRange(new List<List<Vector2Int>>
        {
            new List<Vector2Int> { new(4,8), new(3,7), new(2,7), new(1,6), new(0,6) },
            new List<Vector2Int> { new(5,7), new(4,7), new(3,6), new(2,6), new(1,5), new(0,5) },
            new List<Vector2Int> { new(6,7), new(5,6), new(4,6), new(3,5), new(2,5), new(1,4), new(0,4) },
            new List<Vector2Int> { new(7,6), new(6,6), new(5,5), new(4,5), new(3,4), new(2,4), new(1,3), new(0, 3) },
            new List<Vector2Int> { new(8,6), new(7,5), new(6,5), new(5,4), new(4,4), new(3,3), new(2,3), new(1, 2), new(0, 2) },
            new List<Vector2Int> { new(8,5), new(7,4), new(6,4), new(5,3), new(4,3), new(3,2), new(2, 2), new(1, 1) },
            new List<Vector2Int> { new(8,4), new(7,3), new(6,3), new(5,2), new(4,2), new(3, 1), new(2, 1) },
            new List<Vector2Int> { new(8,3), new(7,2), new(6,2), new(5,1), new(4, 1), new(3, 0) },
            new List<Vector2Int> { new(8,2), new(7,1), new(6,1), new(5, 0), new(4, 0) }
        });

        return allLines;
    }
}
