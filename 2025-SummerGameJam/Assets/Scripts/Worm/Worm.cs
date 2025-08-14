using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Worm : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    // �巡�� �� ��� �����ϰ�
    // �̹��� ����
    // ��� �������� Ȯ�� (���� ��������, �̹� 
    // ��� �����ϴٸ� Destroy�ϰ� 

    private Vector2 DefaultPos;

    public void OnBeginDrag(PointerEventData eventData)
    {
        DefaultPos = transform.position;

        GetComponent<Image>().raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = DefaultPos;
        // ����ĳ��Ʈ Ÿ�ٵ� ������� �����ش�
        GetComponent<Image>().raycastTarget = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }
}
