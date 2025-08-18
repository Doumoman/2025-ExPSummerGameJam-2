using UnityEngine;

public class HomeBtn : MonoBehaviour
{
    public void HomeButton()
    {
        StageManager.Instance.LoadMenuScene();
    }
}
