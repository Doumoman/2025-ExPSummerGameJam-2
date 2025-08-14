using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeButtonImage : MonoBehaviour
{
    [SerializeField] Image myImage;
    [SerializeField] Sprite changeImage;

    public void ChangeImage()
    {
        Sprite originalSprite = myImage.sprite;
        myImage.sprite = changeImage;
        StartCoroutine(setOriginalImage(originalSprite));
    }

    IEnumerator setOriginalImage(Sprite origin)
    {
        yield return new WaitForSeconds(0.1f);
        myImage.sprite = origin;
    }
}
