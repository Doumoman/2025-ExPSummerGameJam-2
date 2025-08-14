using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HexBoardFromTilemap : MonoBehaviour
{
    [Header("Tilemap & Layout")]
    public Tilemap tilemap;                       // Hexagonal Tilemap을 연결
    public HexOrientation orientation = HexOrientation.PointyTop;
    [Tooltip("Tilemap Layout과 맞는 오프셋 타입 선택 (Grid Layout > Hexagon 설정 참고)")]
    public HexOffsetType offsetType = HexOffsetType.OddR;

    [Header("Cell Prefab & Colors")]
    public BoardCell cellPrefab;                  // 스프라이트 포함 프리팹
    public Color emptyColor = new Color(0.2f, 0.22f, 0.26f);
    public Color filledColor = new Color(0.45f, 0.48f, 0.52f);

    [Header("Container (생략 가능)")]
    public Transform container;

    // 런타임 조회용: axial → BoardCell
    public Dictionary<Vector2Int, BoardCell> Cells { get; private set; } = new();

    [ContextMenu("Bake From Tilemap")]
    public void BakeFromTilemap()
    {
        if (!tilemap || !cellPrefab)
        {
            Debug.LogError("Tilemap 또는 CellPrefab이 비었습니다.");
            return;
        }

        var root = container ? container : transform;
        for (int i = root.childCount - 1; i >= 0; --i)
            DestroyImmediate(root.GetChild(i).gameObject);
        Cells.Clear();

        BoundsInt bounds = tilemap.cellBounds;
        int idSeq = 0;

        // 타일이 존재하는 셀만 스캔
        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                var cellPos = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(cellPos)) continue;

                // Tilemap은 Offset 좌표계를 쓰니, 선택한 offsetType으로 Axial 변환
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

        Debug.Log($"보드 생성 완료: {Cells.Count} cells");
    }

    /* ───────── 배치/조회 API : HexPiece와 호환 ───────── */

    // 스크린 → 픽 가능한 셀(Anchor)
    public bool TryScreenToCell(Vector3 screen, Camera cam, out Vector2Int axial, out BoardCell cell)
    {
        Vector3 world = cam.ScreenToWorldPoint(screen);
        // 타일맵 그리드 셀 좌표로 변환 (Offset 좌표)
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
            if (!Cells.TryGetValue(h, out var c)) return false; // 보드 밖
            if (c.occupied) return false;                       // 이미 점유
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

    // 고스트 시뮬레이션용: 앵커 + 오프셋들의 '월드 중앙' 얻기
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