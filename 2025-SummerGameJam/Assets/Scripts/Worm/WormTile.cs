using UnityEngine;
using UnityEngine.EventSystems;

public class WormTile : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private float Rad = 1f;
    
    public void Initialize(WormInfo wormInfo)
    {
        var wormPosList = wormInfo.WormPosList;

        for (int i = 0; i < wormInfo.WormPosList.Count; i++)
        {
            
        }
        // posList�� �°� Ÿ���� �׷��ش�
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
