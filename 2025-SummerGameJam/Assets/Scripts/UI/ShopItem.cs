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

    private bool purchased = false;
    private Color initialCardColor;

    private void Awake()
    {
        myButton = GetComponent<Button>();
        if (cardColor != null) initialCardColor = cardColor.color;
    }

    public void Bind(Item item)
    {
        boundItem = item;

        itemType = item.itemType;
        
        icon.sprite = item.icon;
        description.text = item.itemEffect;

        nameText.text = item.itemName;
        priceText.text = "$" + item.price;

        if (purchased)
            ApplyPurchasedVisual();
        else
            ApplyNormalVisual();
    }

    public void OnClickBuy()
    {
        if (boundItem == null) return;

        if (CoinManager.Instance.UseCoin(boundItem.price) && InGameManager.Instance.CanGetItem())
        {
            InGameManager.Instance.GetItem(itemType);
            purchased = true;
            ApplyPurchasedVisual();
        }
    }

    public void ResetForNewSession()
    {
        purchased = false;
        ApplyNormalVisual();
    }

    void ApplyPurchasedVisual()
    {
        if (cardColor) cardColor.color = boughtColor;
        if (myButton) myButton.interactable = false;
    }

    void ApplyNormalVisual()
    {
        if (cardColor) cardColor.color = initialCardColor;
        if (myButton) myButton.interactable = true;
    }

    public bool IsPurchased => purchased;
}
