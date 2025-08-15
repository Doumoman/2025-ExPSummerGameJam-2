using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCard : MonoBehaviour
{
    [SerializeField] private Image icon;        // 아이템 아이콘
    [SerializeField] private TMP_Text nameText; // 아이템 이름
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text priceText;// 아이템 가격
    [SerializeField] private eItemType itemType;
    [SerializeField] Color boughtColor;
    [SerializeField] Image cardColor;

    private Item boundItem;
    Button myButton;

    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    public void Bind(Item item)
    {
        boundItem = item;

        itemType = item.itemType;
        
        icon.sprite = item.icon;
        description.text = item.itemEffect;

        nameText.text = item.itemName;
        priceText.text = "$" + item.price;
    }

    public void OnClickBuy()
    {
        if (boundItem == null) return;

        if (CoinManager.Instance.UseCoin(boundItem.price) && InGameManager.Instance.CanGetItem())
        {
            cardColor.color = boughtColor;
            
            InGameManager.Instance.GetItem(itemType);

            myButton.interactable = false;
        }
    }
}
