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
        // posList에 맞게 타일을 그려준다
        
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
