using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class WormTile : MonoBehaviour
{
    private float hexSize = 1f;

    public eWormType WormType;
    
    public GameObject WormPrefab;

    public WormInfo _wormInfo;
    
    public void Initialize(WormInfo wormInfo, Vector2 pos, eWormType type)
    {
        _wormInfo = wormInfo;
        WormType = type;
        
        var wormPosList = wormInfo.WormPosList;

        for (int i = 0; i < wormInfo.WormPosList.Count; i++)
        {
            var curPos = wormInfo.WormPosList[i];
            float spawnX = curPos.y % 2 == 0
                ? curPos.x * Mathf.Sqrt(3) / 2f * hexSize
                : (curPos.x + 0.5f) * Mathf.Sqrt(3) / 2f * hexSize;
            float spawnY = curPos.y * 0.75f * hexSize;

            Vector2 spawnPos = new Vector2(spawnX, spawnY);
            
            Worm worm = Instantiate(WormPrefab, spawnPos, quaternion.identity, transform).GetComponent<Worm>();
            worm.Initialize(wormInfo);
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
        
        // Ŭ�� �� ������Ʈ������ Z��ǥ ����
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        // Ŭ���� ������ ������Ʈ �߽��� �Ÿ� ���
        offset = transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        // ���콺 ��ġ + offset ��ŭ �̵�
        transform.position = GetMouseWorldPos() + offset;
        
        int boardLayer = LayerMask.NameToLayer("Board");
        int layerMask = 1 << boardLayer;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.down, 1, layerMask);
        if (hit.collider != null)
        {
            BoardCell tile = hit.collider.GetComponent<BoardCell>();
            bool canPlace = InGameManager.Instance.CanPlaceWorm(this);
        }   
    }

    void OnMouseUp()
    {
        InGameManager.Instance.TurnOff();
        InGameManager.Instance.PlaceWorm(this);
        
        transform.position = originPos;
        transform.localScale = new Vector2(0.6f, 0.6f);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord; // Z ����
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
