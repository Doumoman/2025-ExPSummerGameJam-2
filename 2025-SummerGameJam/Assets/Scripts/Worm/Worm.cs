using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Worm : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // 드래그 앤 드롭 가능하게
    // 이미지 세팅
    // 드롭 가능한지 확인 (벽에 막히는지, 이미 
    // 드롭 가능하다면 Destroy하고 

    private Vector2 DefaultPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = transform.position;

        GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = DefaultPos;
        // 레이캐스트 타겟도 원래대로 돌려준다
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
