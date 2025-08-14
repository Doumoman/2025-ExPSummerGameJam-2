using System;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    #region Singleton
    private static DrawManager _instance;
    public static DrawManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DrawManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("InGameManager");
                    _instance = go.AddComponent<DrawManager>();
                }
            }
            return _instance;
        }
    }
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion
    
    
    public GameObject hexPrefab;

    private int centerCol = 4;
    private int centerRow = 4;

    private float hexSize = 1f;

    void Initialize()
    {
        InGameManager.Instance.OnMapChanged += Build;
    }
    
    void Build()
    {
        Debug.Log("1");

        var _beeHive = InGameManager.Instance._beeHive;
        if (hexPrefab == null) { Debug.LogError("hexPrefab이 비었습니다."); return; }

        int rows = _beeHive.GetLength(0);
        int cols = _beeHive.GetLength(1);

        // 기준 셀( centerRow, centerCol )의 픽셀 좌표 계산
        var centerAxial = OffsetOddR_ToAxial(centerCol, centerRow);
        Vector2 centerPixel = AxialToPixel_FlatTop(centerAxial.q, centerAxial.r, hexSize);
        
        Debug.Log("1");

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                int colorIndex = _beeHive[r, c];
                if (colorIndex < 0) continue; // -1 등은 비어있는 칸으로 처리

                // 1) (row,col) -> (q,r) (odd-r offset 기준)
                var axial = OffsetOddR_ToAxial(c, r);

                // 2) (q,r) -> 픽셀 좌표(Flat-Top)
                Vector2 pixel = AxialToPixel_FlatTop(axial.q, axial.r, hexSize);

                // 3) 기준 셀을 (0,0)으로 옮기기
                Vector2 rel = pixel - centerPixel;

                // 4) 월드 배치
                Vector3 worldPos = new Vector3(rel.x, rel.y, 0f);

                var go = Instantiate(hexPrefab, worldPos, Quaternion.identity, transform);
                ApplyColor(go, colorIndex);
            }
        }
    }
    
    // ----- 좌표 변환(odd-r offset -> axial) -----
    // odd-r: "행 기준 오프셋" 레이아웃(Flat-Top에 일반적)
    // q = col - floor(row/2), r = row
    static (int q, int r) OffsetOddR_ToAxial(int col, int row)
    {
        int q = col - ((row - (row & 1)) / 2); // floor(row/2)
        int rAx = row;
        return (q, rAx);
    }

    // ----- Axial(q,r) -> Pixel(Flat-Top) -----
    // x = size * 3/2 * q
    // y = size * sqrt(3) * (r + q/2)
    static Vector2 AxialToPixel_FlatTop(int q, int r, float size)
    {
        float x = size * 1.5f * q;
        float y = size * Mathf.Sqrt(3f) * (r + q * 0.5f);
        return new Vector2(x, y);
    }
    
    void ApplyColor(GameObject go, int idx)
    {
        Color col = IndexToColor(idx);
        var mr = go.GetComponent<Renderer>();
        if (mr != null) { mr.material.color = col; return; }
        var sr = go.GetComponent<SpriteRenderer>();
        if (sr != null) { sr.color = col; }
    }

    Color IndexToColor(int idx)
    {
        // 필요에 맞게 맵핑하세요
        switch (idx)
        {
            case 0: return Color.white;
            case 1: return Color.red;
            case 2: return Color.green;
            case 3: return Color.blue;
            case 4: return Color.yellow;
            default: return new Color(0.75f, 0.75f, 0.75f); // 기타
        }
    }
}
