using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Hex/Shape")]
public class HexShape : ScriptableObject
{
    [Tooltip("������ Ÿ�� �����µ� (anchor=0,0 ����, axial q,r)")]
    public List<Vector2Int> offsets = new() { Vector2Int.zero };
    public Color color = Color.yellow;

    // �ð� ���� 60�� ȸ��
    public List<Vector2Int> RotatedCW(List<Vector2Int> src)
    {
        var res = new List<Vector2Int>(src.Count);
        foreach (var v in src)
        {
            // cube: (x,y,z) = (q, -q-r, r) �� ȸ��
            int q = v.x, r = v.y;
            int x = q, y = -q - r, z = r;
            // ȸ��: (x,y,z) -> (-z, -x, -y) �� CW 60��
            int rx = -z, ry = -x, rz = -y;
            res.Add(new Vector2Int(rx, rz)); // (q', r')
        }
        return res;
    }
}