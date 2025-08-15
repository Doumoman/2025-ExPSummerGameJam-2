using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public eItemType itemType;
    public string itemName;
    public Sprite icon;
    public int price;
    public string itemEffect;
    [Tooltip("아이템의 고유 인덱스")]
    public int itemIndex;
}