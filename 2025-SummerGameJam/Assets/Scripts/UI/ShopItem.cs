using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : MonoBehaviour
{
    [SerializeField] private Image icon;        // ������ ������
    [SerializeField] private TMP_Text nameText; // ������ �̸�
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text priceText;// ������ ����
    [SerializeField] Color boughtColor;
    [SerializeField] Image cardColor;

    private Item boundItem;

    public void Bind(Item item)
    {
        boundItem = item;

        icon.sprite = item.icon;
        description.text = item.itemEffect;

        nameText.text = item.itemName;
        priceText.text = "$" + item.price;
    }

    public void OnClickBuy()
    {
        if (boundItem == null) return;

        if (CoinManager.Instance.UseCoin(boundItem.price))
        {
            cardColor.color = boughtColor;
            ItemManager.Instance.AddItem(boundItem);
        }
    }
}
