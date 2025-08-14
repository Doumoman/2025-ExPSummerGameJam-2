using UnityEngine;

public class PlayBtn : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void PlayButton()
    {
        pauseMenu.SetActive(false);
    }
}
