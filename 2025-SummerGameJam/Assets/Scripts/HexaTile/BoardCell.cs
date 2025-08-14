using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BoardCell : MonoBehaviour
{
    [Header("ID / 좌표")]
    public string id;                 // 필요하면 에디터/런타임에서 지정
    public Vector2Int axial;          // (q,r)
    public Vector3 worldCenter;       // 배치 기준점(타일 중앙)

    [Header("상태")]
    public bool occupied;

    SpriteRenderer _sr;
    void Awake() => _sr = GetComponent<SpriteRenderer>();

    public void SetColor(Color c)
    {
        if (!_sr) _sr = GetComponent<SpriteRenderer>();
        _sr.color = c;
    }
}