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
        if(newItem == null)
        {
            return;
        }
        itemImage.sprite = newItem.icon;

        var image = Instantiate(itemImage, gridTransform);
    }
}
