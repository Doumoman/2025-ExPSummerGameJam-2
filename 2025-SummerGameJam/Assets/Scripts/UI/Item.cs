using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item")]
public class Item : ScriptableObject
{
    public eItemType itemType;
    public string itemName;
    public Sprite icon;
    public int price;
    public string itemEffect;
    [Tooltip("�������� ���� �ε���")]
    public int itemIndex;
}