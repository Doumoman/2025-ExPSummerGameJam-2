using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WormTile : MonoBehaviour
{
    private float hexSize = 1f;
    
    public GameObject WormPrefab;
    
    public void Initialize(WormInfo wormInfo)
    {
        var wormPosList = wormInfo.WormPosList;

        for (int i = 0; i < wormInfo.WormPosList.Count; i++)
        {
            var curPos = wormInfo.WormPosList[i];
            float spawnX = curPos.x % 2 == 0 ? curPos.y * Mathf.Sqrt(3) / 2f * hexSize : (curPos.y + 0.5f) * Mathf.Sqrt(3) / 2f * hexSize;
            float spawnY = curPos.x * 0.75f * hexSize;

            Vector2 spawnPos = new Vector2(spawnX, spawnY);

            Instantiate(WormPrefab, spawnPos, quaternion.identity, transform);
        }
        // posList�� �°� Ÿ���� �׷��ش�
        
    }

    private Vector2 originPos;
    private Vector3 offset;
    private float zCoord;

    void OnMouseDown()
    {
        originPos = transform.position;
        
        // Ŭ�� �� ������Ʈ������ Z��ǥ ����
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        // Ŭ���� ������ ������Ʈ �߽��� �Ÿ� ���
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        // ���콺 ��ġ + offset ��ŭ �̵�
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        transform.position = originPos;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord; // Z ����
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
