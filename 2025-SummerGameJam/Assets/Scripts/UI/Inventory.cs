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

        // �ν��Ͻ� ����
        var image = Instantiate(itemImage, gridTransform);
        image.sprite = newItem.icon;

        // ��ư ���̱�(�̹� ������ ����)
        var btn = image.GetComponent<Button>();
        if (btn == null)
        {
            btn = image.gameObject.AddComponent<Button>();
            // Button�� �۵��Ϸ��� targetGraphic�� �ʿ� �� Image�� ����
            btn.targetGraphic = image;
        }

        // ������ ���(ĸ��: �ε���)
        int fixedIndex = newItem.itemIndex;
        btn.onClick.AddListener(() => ItemManager.Instance.OnClickItem(fixedIndex));
    }
}
