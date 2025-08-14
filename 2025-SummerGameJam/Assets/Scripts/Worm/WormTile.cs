using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WormTile : MonoBehaviour
{
    private float hexSize = 1f;
    
    public GameObject WormPrefab;
    
    public void Initialize(WormInfo wormInfo, Vector2 pos)
    {
        var wormPosList = wormInfo.WormPosList;

        for (int i = 0; i < wormInfo.WormPosList.Count; i++)
        {
            var curPos = wormInfo.WormPosList[i];
            float spawnX = curPos.x % 2 == 0
                ? curPos.y * Mathf.Sqrt(3) / 2f * hexSize
                : (curPos.y + 0.5f) * Mathf.Sqrt(3) / 2f * hexSize;
            float spawnY = curPos.x * 0.75f * hexSize;

            Vector2 spawnPos = new Vector2(spawnX, spawnY);

            Instantiate(WormPrefab, spawnPos, quaternion.identity, transform);
        }

        transform.localScale = new Vector2(0.6f, 0.6f);
        transform.position = pos;
    }

    private Vector2 originPos;
    private Vector3 offset;
    private float zCoord;

    void OnMouseDown()
    {
        transform.localScale = Vector3.one;
        originPos = transform.position;
        
        // 클릭 시 오브젝트까지의 Z좌표 저장
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        // 클릭한 지점과 오브젝트 중심의 거리 계산
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        // 마우스 위치 + offset 만큼 이동
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        transform.position = originPos;
        transform.localScale = new Vector2(0.6f, 0.6f);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord; // Z 고정
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
