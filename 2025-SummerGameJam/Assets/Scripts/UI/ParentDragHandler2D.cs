using UnityEngine;

[RequireComponent(typeof(Transform))]
public class ParentDragHandler2D : MonoBehaviour
{
    [Header("Options")]
    public LayerMask hitMask = Physics2D.DefaultRaycastLayers; // �巡�׸� ����� ���̾�
    public bool useRigidbody2D = true;                          // Rigidbody2D�� �̵����� ����

    bool isDragging;
    Vector3 offset;           // Ŭ�� ������ ������Ʈ �߽��� ����
    float zLock;              // Z ���� (2D)

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        zLock = transform.position.z;
    }

    void Update()
    {
        // ����
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            // �ش� ������ ��� 2D �ݶ��̴� ��ȸ
            var hits = Physics2D.OverlapPointAll(mouseWorld, hitMask);
            foreach (var h in hits)
            {
                var t = h.transform;
                if (t == transform || t.IsChildOf(transform))
                {
                    isDragging = true;

                    // ������ ������Ʈ�� ���� ��ġ
                    var pivot = transform.position;
                    offset = pivot - new Vector3(mouseWorld.x, mouseWorld.y, pivot.z);
                    break;
                }
            }
        }

        // �巡�� ��
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            Vector3 target = new Vector3(mouseWorld.x, mouseWorld.y, zLock) + offset;

            if (useRigidbody2D && rb != null)
                rb.MovePosition(new Vector2(target.x, target.y)); // ���� ��� �̵�
            else
                transform.position = target;                      // Ʈ������ ���� �̵�
        }

        // ����
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}