using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hex/Shape")]
public class HexShape : ScriptableObject
{
    [Tooltip("조각의 타일 오프셋들 (anchor=0,0 기준, axial q,r)")]
    public List<Vector2Int> offsets = new() { Vector2Int.zero };
    public Color color = Color.yellow;

    // 시계 방향 60° 회전
    public List<Vector2Int> RotatedCW(List<Vector2Int> src)
    {
        var res = new List<Vector2Int>(src.Count);
        foreach (var v in src)
        {
            // cube: (x,y,z) = (q, -q-r, r) → 회전
            int q = v.x, r = v.y;
            int x = q, y = -q - r, z = r;
            // 회전: (x,y,z) -> (-z, -x, -y) 가 CW 60도
            int rx = -z, ry = -x, rz = -y;
            res.Add(new Vector2Int(rx, rz)); // (q', r')
        }
        return res;
    }
}