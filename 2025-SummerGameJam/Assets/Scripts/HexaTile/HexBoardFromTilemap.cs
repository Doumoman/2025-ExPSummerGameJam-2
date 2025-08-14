using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexBoardFromTilemap : MonoBehaviour
{
    [Header("Tilemap & Layout")]
    public Tilemap tilemap;                       // Hexagonal Tilemap�� ����
    public HexOrientation orientation = HexOrientation.PointyTop;
    [Tooltip("Tilemap Layout�� �´� ������ Ÿ�� ���� (Grid Layout > Hexagon ���� ����)")]
    public HexOffsetType offsetType = HexOffsetType.OddR;

    [Header("Cell Prefab & Colors")]
    public BoardCell cellPrefab;                  // ��������Ʈ ���� ������
    public Color emptyColor = new Color(0.2f, 0.22f, 0.26f);
    public Color filledColor = new Color(0.45f, 0.48f, 0.52f);

    [Header("Container (���� ����)")]
    public Transform container;

    // ��Ÿ�� ��ȸ��: axial �� BoardCell
    public Dictionary<Vector2Int, BoardCell> Cells { get; private set; } = new();

    [ContextMenu("Bake From Tilemap")]
    public void BakeFromTilemap()
    {
        if (!tilemap || !cellPrefab)
        {
            Debug.LogError("Tilemap �Ǵ� CellPrefab�� ������ϴ�.");
            return;
        }

        var root = container ? container : transform;
        for (int i = root.childCount - 1; i >= 0; --i)
            DestroyImmediate(root.GetChild(i).gameObject);
        Cells.Clear();

        BoundsInt bounds = tilemap.cellBounds;
        int idSeq = 0;

        // Ÿ���� �����ϴ� ���� ��ĵ
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                var cellPos = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(cellPos)) continue;

                // Tilemap�� Offset ��ǥ�踦 ����, ������ offsetType���� Axial ��ȯ
                var offset = new Vector2Int(x, y);
                var axial = HexOffset.OffsetToAxial(offset, offsetType);

                Vector3 world = tilemap.GetCellCenterWorld(cellPos);
                var cell = Instantiate(cellPrefab, root);
                cell.transform.position = world;
                cell.worldCenter = world;
                cell.axial = axial;
                cell.id = $"cell_{idSeq++}";
                cell.occupied = false;
                cell.SetColor(emptyColor);
                cell.name = $"Hex {axial.x},{axial.y}";
                Cells[axial] = cell;
            }
        }

        Debug.Log($"���� ���� �Ϸ�: {Cells.Count} cells");
    }

    /* ������������������ ��ġ/��ȸ API : HexPiece�� ȣȯ ������������������ */

    // ��ũ�� �� �� ������ ��(Anchor)
    public bool TryScreenToCell(Vector3 screen, Camera cam, out Vector2Int axial, out BoardCell cell)
    {
        Vector3 world = cam.ScreenToWorldPoint(screen);
        // Ÿ�ϸ� �׸��� �� ��ǥ�� ��ȯ (Offset ��ǥ)
        Vector3Int cellPos = tilemap.WorldToCell(world);
        var offset = new Vector2Int(cellPos.x, cellPos.y);
        axial = HexOffset.OffsetToAxial(offset, offsetType);
        return Cells.TryGetValue(axial, out cell);
    }

    public bool CanPlace(List<Vector2Int> shapeOffsets, Vector2Int anchorAxial)
    {
        foreach (var o in shapeOffsets)
        {
            var h = anchorAxial + o;
            if (!Cells.TryGetValue(h, out var c)) return false; // ���� ��
            if (c.occupied) return false;                       // �̹� ����
        }
        return true;
    }

    public void Place(List<Vector2Int> shapeOffsets, Vector2Int anchorAxial, Color pieceColor)
    {
        foreach (var o in shapeOffsets)
        {
            var h = anchorAxial + o;
            var c = Cells[h];
            c.occupied = true;
            c.SetColor(pieceColor);
        }
    }

    // ��Ʈ �ùķ��̼ǿ�: ��Ŀ + �����µ��� '���� �߾�' ���
    public bool TryGetWorldCenters(List<Vector2Int> shapeOffsets, Vector2Int anchorAxial, out List<Vector3> centers)
    {
        centers = new();
        foreach (var o in shapeOffsets)
        {
            var h = anchorAxial + o;
            if (!Cells.TryGetValue(h, out var c)) return false;
            centers.Add(c.worldCenter);
        }
        return true;
    }
}