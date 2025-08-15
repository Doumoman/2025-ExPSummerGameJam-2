using UnityEngine;

[RequireComponent(typeof(Transform))]
public class ParentDragHandler2D : MonoBehaviour
{
    [Header("Options")]
    public LayerMask hitMask = Physics2D.DefaultRaycastLayers; // 드래그를 허용할 레이어
    public bool useRigidbody2D = true;                          // Rigidbody2D로 이동할지 여부

    bool isDragging;
    Vector3 offset;           // 클릭 지점과 오브젝트 중심의 차이
    float zLock;              // Z 고정 (2D)

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        zLock = transform.position.z;
    }

    void Update()
    {
        // 시작
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            // 해당 지점의 모든 2D 콜라이더 조회
            var hits = Physics2D.OverlapPointAll(mouseWorld, hitMask);
            foreach (var h in hits)
            {
                var t = h.transform;
                if (t == transform || t.IsChildOf(transform))
                {
                    isDragging = true;

                    // 기준은 오브젝트의 월드 위치
                    var pivot = transform.position;
                    offset = pivot - new Vector3(mouseWorld.x, mouseWorld.y, pivot.z);
                    break;
                }
            }
        }

        // 드래그 중
        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0f;

            Vector3 target = new Vector3(mouseWorld.x, mouseWorld.y, zLock) + offset;

            if (useRigidbody2D && rb != null)
                rb.MovePosition(new Vector2(target.x, target.y)); // 물리 기반 이동
            else
                transform.position = target;                      // 트랜스폼 직접 이동
        }

        // 종료
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}