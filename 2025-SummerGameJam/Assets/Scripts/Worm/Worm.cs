using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Worm : MonoBehaviour
{
    public void Initialize(WormInfo info)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = info.WormSprite;
    }
}
