using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Transform gridTransform;
    [SerializeField] Image itemImage;

    public void AddItem(Item newItem)
    {
        if (newItem == null) return;

        // 인스턴스 생성
        var image = Instantiate(itemImage, gridTransform);
        image.sprite = newItem.icon;

        // 버튼 붙이기(이미 있으면 재사용)
        var btn = image.GetComponent<Button>();
        if (btn == null)
        {
            btn = image.gameObject.AddComponent<Button>();
            // Button이 작동하려면 targetGraphic이 필요 → Image를 지정
            btn.targetGraphic = image;
        }

        // 리스너 등록(캡쳐: 인덱스)
        int fixedIndex = newItem.itemIndex;
        btn.onClick.AddListener(() => ItemManager.Instance.OnClickItem(fixedIndex));
    }
}
