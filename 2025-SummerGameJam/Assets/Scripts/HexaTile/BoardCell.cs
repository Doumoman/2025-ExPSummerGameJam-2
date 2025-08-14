using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BoardCell : MonoBehaviour
{
    [Header("ID / ��ǥ")]
    public string id;                 // �ʿ��ϸ� ������/��Ÿ�ӿ��� ����
    public Vector2Int axial;          // (q,r)
    public Vector3 worldCenter;       // ��ġ ������(Ÿ�� �߾�)

    [Header("����")]
    public bool occupied;

    SpriteRenderer _sr;
    void Awake() => _sr = GetComponent<SpriteRenderer>();

    public void SetColor(Color c)
    {
        if (!_sr) _sr = GetComponent<SpriteRenderer>();
        _sr.color = c;
    }
}